using FlatStage.Graphics;

namespace FlatStage.Toolkit;
public class GuiWindow : GuiContainer
{
    internal static new readonly int STypeId;

    static GuiWindow()
    {
        STypeId = ++SBTypeId;
    }

    internal override int TypeId => STypeId;

    public override Size SizeHint => new(400, 300);

    private readonly int _topBarHeight;

    public GuiWindow(string id, Gui gui, GuiContainer? parent = null) : base(id, gui, parent)
    {
        _topBarHeight = Gui.Skin.StyleSheet.GetProperty<int>(this, DefaultStyleProperties.WindowTopBarHeight);

        AddInteraction<GuiDragInteraction>();
        Padding = 1;
        Hidden = true;

        var windowLayout = new GuiVerticalLayout($"{id}_mainLayout", gui, this)
        {
            Anchor = GuiAnchoring.Fill,
            Interactive = false
        };

        var topBar = new GuiPanel($"{id}_topBar", gui, windowLayout)
        {
            Height = _topBarHeight,
            Interactive = false,
        };

        var topLayout = new GuiHorizontalLayout($"{id}_topLayout", gui, topBar)
        {
            Anchor = GuiAnchoring.Fill,
            LayoutMode = GuiLayoutMode.AlignEnd,
            Padding = 2,
            Interactive = false
        };

        _windowTitle = new GuiText($"{id}_titleLabel", gui, topLayout)
        {
            Interactive = false,
            Anchor = GuiAnchoring.Fill
        };

        _closeButton = new GuiButton($"{id}_closeButton", gui, topLayout)
        {
            Label = "X",
            Width = _topBarHeight
        };

        _mainContainer = new GuiContainer($"{id}_mainContainer", gui, windowLayout)
        {
            Padding = 10,
            Height = this.Height - _topBarHeight

        };

        _closeButton.OnClick += (_) => { this.Hidden = true; };

        var topBarBgColor = Color.DodgerBlue;
        var exitButtonBgColor = Color.FromHex("e63946");

        Gui.Skin.StyleSheet.SetProperty(topBar, GuiControlState.Idle, DefaultStyleProperties.BackgroundColor, topBarBgColor);
        Gui.Skin.StyleSheet.SetProperty(topBar, GuiControlState.Idle, DefaultStyleProperties.BorderColor, topBarBgColor * 0.3f);
        Gui.Skin.StyleSheet.SetProperty(topBar, GuiControlState.Idle, DefaultStyleProperties.InnerBorderColor, topBarBgColor * 1.5f);

        Gui.Skin.StyleSheet.SetProperty(_closeButton, GuiControlState.Idle, DefaultStyleProperties.BackgroundColor, exitButtonBgColor);
        Gui.Skin.StyleSheet.SetProperty(_closeButton, GuiControlState.Idle, DefaultStyleProperties.BorderColor, exitButtonBgColor * 0.3f);
        Gui.Skin.StyleSheet.SetProperty(_closeButton, GuiControlState.Idle, DefaultStyleProperties.InnerBorderColor, exitButtonBgColor * 2.0f);

        Gui.Skin.StyleSheet.SetProperty(_closeButton, GuiControlState.Hover, DefaultStyleProperties.BackgroundColor, exitButtonBgColor * 1.5f);
        Gui.Skin.StyleSheet.SetProperty(_closeButton, GuiControlState.Hover, DefaultStyleProperties.BorderColor, exitButtonBgColor * 0.5f);
        Gui.Skin.StyleSheet.SetProperty(_closeButton, GuiControlState.Hover, DefaultStyleProperties.InnerBorderColor, exitButtonBgColor * 2.0f);

        Gui.Skin.StyleSheet.SetProperty(_closeButton, GuiControlState.Active, DefaultStyleProperties.BackgroundColor, exitButtonBgColor * 0.7f);
        Gui.Skin.StyleSheet.SetProperty(_closeButton, GuiControlState.Active, DefaultStyleProperties.BorderColor, exitButtonBgColor * 0.9f);
        Gui.Skin.StyleSheet.SetProperty(_closeButton, GuiControlState.Active, DefaultStyleProperties.InnerBorderColor, exitButtonBgColor * 0.5f);

    }

    internal override void InitFromDefinition(GuiControlDef definition)
    {
        if (definition is GuiWindowDef windowDef)
        {
            _windowTitle.SetText(windowDef.Title);
            _windowTitle.HorizontalAlign = windowDef.TitleHorizontalAlignment;
            _windowTitle.VerticalAlign = windowDef.TitleVerticalAlignment;
            Padding = windowDef.Padding;

            if (windowDef.Children != null)
            {
                for (var i = 0; i < windowDef.Children.Length; ++i)
                {
                    Gui.CreateFromDefinition(Gui, windowDef.Children[i], this._mainContainer);
                }
            }
        }
    }

    internal override void Draw(Canvas canvas, GuiSkin skin)
    {
        skin.DrawWindow(canvas, this);

        base.Draw(canvas, skin);
    }

    private readonly GuiText _windowTitle;
    private readonly GuiButton _closeButton;
    private readonly GuiContainer _mainContainer;
}
