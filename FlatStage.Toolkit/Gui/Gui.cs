namespace FlatStage.Toolkit;

public class Gui : GameObject
{
    public GuiTheme Theme { get; }

    internal Gui(string id) : base(id)
    {
        _interactablesSystem = Behaviors.ActivateSystem<InteractablesSystem>();
        _orderedList = new FastList<GameObject>();
        _guiViewport = new CanvasViewport(Canvas.Width, Canvas.Height)
        {
            BackgroundColor = Color.Transparent
        };
        _statesMap = new Dictionary<int, GuiControlState>();
        Theme = new DefaultGuiTheme(this);
        Width = Canvas.Width;
        Height = Canvas.Height;

        _interactablesSystem.OnInteractableHover += OnNodeHover;
        _interactablesSystem.OnInteractableMouseDown += OnNodeMouseDown;
        _interactablesSystem.OnInteractableMouseUp += OnNodeMouseUp;
        _interactablesSystem.OnInteractableMouseOut += OnNodeMouseOut;
    }

    public void Invalidate()
    {
        _invalidated = true;
    }

    public GuiControlState GetState(GameObject gameObject)
    {
        return _statesMap[gameObject.UId];
    }

    private void UpdateNodeState(GameObject gameObject, GuiControlState state)
    {
        if (_statesMap.TryGetValue(gameObject.UId, out _))
        {
            _statesMap[gameObject.UId] = state;
            Invalidate();
        }
    }

    private void OnNodeMouseOut(GameObject gameObject)
    {
        UpdateNodeState(gameObject, GuiControlState.Idle);
    }

    private void OnNodeMouseUp(GameObject gameObject)
    {
        UpdateNodeState(gameObject, GuiControlState.Hover);
    }

    private void OnNodeMouseDown(GameObject gameObject)
    {
        UpdateNodeState(gameObject, GuiControlState.Active);
    }

    private void OnNodeHover(GameObject gameObject)
    {
        UpdateNodeState(gameObject, GuiControlState.Hover);
    }

    public override void Draw(Canvas canvas)
    {
        if (!Visible)
        {
            return;
        }

        if (DrawBoundaries)
        {
            canvas.DrawRect(GlobalBounds.X, GlobalBounds.Y, GlobalBounds.Width, GlobalBounds.Height, 1, Color.Fuchsia);
        }

        if (_invalidated && Children?.IsEmpty() == false)
        {
            Console.WriteLine("Gui Redraw");
            canvas.SetViewport(_guiViewport);
            var children = Children.ReadOnlySpan;

            foreach (var child in children)
            {
                if (child.Visible)
                {
                    child.Draw(canvas);
                }
            }
            canvas.SetViewport();

            _invalidated = false;
        }

        canvas.Draw(_guiViewport.Texture, Vec2.Zero, Color.White);

        _debugText.Clear();
        _debugText.Append("Hovered: ");
        _debugText.AppendLine(_interactablesSystem.Hovered?.Parent.Id ?? "None");
        _debugText.Append("Active: ");
        _debugText.AppendLine(_interactablesSystem.Active?.Parent.Id ?? "None");
        _debugText.Append("Mouse Locked: ");
        _debugText.AppendLine(_interactablesSystem.MouseLocked?.Parent.Id ?? "None");
        _debugText.Append("MousePos: ");
        _debugText.Append(_interactablesSystem.MouseState.MouseX);
        _debugText.Append(",");
        _debugText.AppendLine(_interactablesSystem.MouseState.MouseY);

        canvas.DrawText(BuiltinContent.Fonts.Monogram, _debugText.ReadOnlySpan, new Vec2(20, 400), Color.Cyan);
    }

    internal void Register(GameObject gameObject)
    {
        Behaviors.AddComponent<Interactable>(gameObject);
        _statesMap[gameObject.UId] = GuiControlState.Idle;

        _orderedList.Add(gameObject);

        _orderedList.Sort((o1, o2) => o2.TreeDepth - o1.TreeDepth);
    }

    internal void RecalculateLayouts()
    {
        for (int i = 0; i < _orderedList.Count; ++i)
        {
            var gameObject = _orderedList[i];
            if (gameObject is Layout layout)
            {
                layout.DoLayout();
            }
        }
    }

    public override void InitFromDefinition(GameObjectDef definition)
    {
        base.InitFromDefinition(definition);

        //var guiDef = (definition as GuiDef)!;

        // if (guiDef.Style != null)
        // {
        //     UpdateStyleSheetFromDefinition(guiDef.Style);
        // }
    }

    // private void UpdateStyleSheetFromDefinition(StyleDef styleDef)
    // {
    //     foreach (var styleRule in styleDef.Rules)
    //     {
    //     }
    // }



    private readonly FastList<GameObject> _orderedList;
    private readonly InteractablesSystem _interactablesSystem;
    private readonly CanvasViewport _guiViewport;
    private bool _invalidated = true;
    private readonly Dictionary<int, GuiControlState> _statesMap;
    private readonly StringBuffer _debugText = new();
}
