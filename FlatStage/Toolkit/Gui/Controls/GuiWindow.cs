using FlatStage.Graphics;
using System.Collections.Generic;

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

    internal const int TopBarHeight = 40;

    internal const string TopBarCustomElementId = "topBar";

    public GuiWindow(string id, Gui gui, GuiContainer? parent = null) : base(id, gui, parent)
    {
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
            Height = TopBarHeight,
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
            Width = TopBarHeight
        };

        _mainContainer = new GuiContainer($"{id}_mainContainer", gui, windowLayout)
        {
            Padding = 10,
            Height = this.Height - TopBarHeight

        };

        _closeButton.OnClick += (_) => { this.Hidden = true; };

        var topBarBgColor = Color.DodgerBlue;
        var exitButtonBgColor = Color.FromHex("e63946");

        Gui.Skin.StyleSheet.SetCustomStyle(topBar.Id, new(
            new Dictionary<GuiControlState, Style>
            {
                {
                    GuiControlState.Idle, new Style()
                    {
                        BackgroundColor = topBarBgColor,
                        BorderColor = topBarBgColor * 1.5f,
                        InnerBorderColor = new Color(40,40,40),
                        TextColor = new Color(255,255,255),
                        ShadowColor = new Color(0,0,0),
                    }
                },
                {
                    GuiControlState.Hover, new Style()
                    {
                        BackgroundColor = topBarBgColor * 1.5f,
                        BorderColor = topBarBgColor * 2f,
                        InnerBorderColor = new Color(40,40,40),
                        TextColor = new Color(255,255,255),
                        ShadowColor = new Color(0,0,0),
                    }
                }
            }
        ));

        Gui.Skin.StyleSheet.SetCustomStyle(_closeButton.Id, new(
            new Dictionary<GuiControlState, Style>
            {
                {
                    GuiControlState.Idle, new Style()
                    {
                        BackgroundColor = exitButtonBgColor,
                        BorderColor = exitButtonBgColor * 2f,
                        InnerBorderColor = new Color(40,40,40),
                        TextColor = new Color(255,255,255),
                        ShadowColor = new Color(0,0,0),
                    }
                },
                {
                    GuiControlState.Hover, new Style()
                    {
                        BackgroundColor = exitButtonBgColor * 1.5f,
                        BorderColor = exitButtonBgColor * 2f,
                        InnerBorderColor = new Color(40,40,40),
                        TextColor = new Color(255,255,255),
                        ShadowColor = new Color(0,0,0),
                    }
                }
            }
        ));

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
