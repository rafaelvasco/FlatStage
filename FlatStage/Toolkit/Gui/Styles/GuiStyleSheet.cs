using FlatStage.Graphics;
using System.Collections.Generic;

namespace FlatStage.Toolkit;

public class GuiStyleSheet
{
    public Dictionary<int, StateStyleMap> BaseStyles { get; }

    public Dictionary<string, StateStyleMap> CustomStyles { get; }

    public void SetCustomStyle(string elementId, StateStyleMap styles)
    {
        CustomStyles.Add(elementId, styles);
    }

    public GuiStyleSheet()
    {
        BaseStyles = new Dictionary<int, StateStyleMap>()
        {
            {
                GuiButton.STypeId, new StateStyleMap(new(){
                {
                    GuiControlState.Idle, new Style()
                    {
                        BackgroundColor = new Color(30,30,30),
                        BorderColor = new Color(60,60,60),
                        InnerBorderColor = new Color(40,40,40),
                        TextColor = new Color(255,255,255),
                        ShadowColor = new Color(0,0,0),
                    }
                },
                {
                    GuiControlState.Hover, new Style()
                    {
                        BackgroundColor = new Color(40,40,40),
                        BorderColor = new Color(60,60,60),
                        InnerBorderColor = new Color(40,40,40),
                        TextColor = new Color(255,255,255),
                        ShadowColor = new Color(0,0,0),
                    }
                }})
            },
            {
                GuiCheckbox.STypeId, new StateStyleMap(new(){
                {
                    GuiControlState.Idle, new Style()
                    {
                        BackgroundColor = new Color(30,30,30),
                        BorderColor = new Color(60,60,60),
                        InnerBorderColor = new Color(40,40,40),
                        TextColor = new Color(255,255,255),
                        ShadowColor = new Color(0,0,0),
                        CustomElements = new ()
                        {
                            {
                                GuiCheckbox.CheckCustomElementId, Color.DodgerBlue
                            }
                        }
                    }
                },
                {
                    GuiControlState.Hover, new Style()
                    {
                        BackgroundColor = new Color(40,40,40),
                        BorderColor = new Color(60,60,60),
                        InnerBorderColor = new Color(40,40,40),
                        TextColor = new Color(255,255,255),
                        ShadowColor = new Color(0,0,0),
                        CustomElements = new ()
                        {
                            {
                                GuiCheckbox.CheckCustomElementId, Color.DodgerBlue
                            }
                        }
                    }
                }})
            },
            {
                GuiPanel.STypeId, new StateStyleMap(new(){
                {
                    GuiControlState.Idle, new Style()
                    {
                        BackgroundColor = new Color(30,30,30),
                        BorderColor = new Color(60,60,60),
                        InnerBorderColor = new Color(40,40,40),
                        TextColor = new Color(255,255,255),
                        ShadowColor = new Color(0,0,0),
                    }
                },
                {
                    GuiControlState.Hover, new Style()
                    {
                        BackgroundColor = new Color(40,40,40),
                        BorderColor = new Color(60,60,60),
                        InnerBorderColor = new Color(40,40,40),
                        TextColor = new Color(255,255,255),
                        ShadowColor = new Color(0,0,0),
                    }
                }})
            },
            {
                GuiSlider.STypeId, new StateStyleMap(new(){
                {
                    GuiControlState.Idle, new Style()
                    {
                        BackgroundColor = new Color(30,30,30),
                        BorderColor = new Color(60,60,60),
                        InnerBorderColor = new Color(40,40,40),
                        TextColor = new Color(255,255,255),
                        ShadowColor = new Color(0,0,0),
                        CustomElements = new ()
                        {
                            {
                                GuiSlider.ThumbCustomElementId, Color.DodgerBlue
                            }
                        }
                    }
                },
                {
                    GuiControlState.Hover, new Style()
                    {
                        BackgroundColor = new Color(40,40,40),
                        BorderColor = new Color(60,60,60),
                        InnerBorderColor = new Color(40,40,40),
                        TextColor = new Color(255,255,255),
                        ShadowColor = new Color(0,0,0),
                        CustomElements = new ()
                        {
                            {
                                GuiSlider.ThumbCustomElementId, Color.DodgerBlue
                            }
                        }
                    }
                }})
            },
            {
                GuiTextbox.STypeId, new StateStyleMap(new(){
                {
                    GuiControlState.Idle, new Style()
                    {
                        BackgroundColor = new Color(30,30,30),
                        BorderColor = new Color(60,60,60),
                        InnerBorderColor = new Color(40,40,40),
                        TextColor = new Color(255,255,255),
                        ShadowColor = new Color(0,0,0),
                        CustomElements = new ()
                        {
                            {
                                GuiTextbox.CaretCustomElementId, Color.DodgerBlue
                            }
                        }
                    }
                },
                {
                    GuiControlState.Hover, new Style()
                    {
                        BackgroundColor = new Color(40,40,40),
                        BorderColor = new Color(60,60,60),
                        InnerBorderColor = new Color(40,40,40),
                        TextColor = new Color(255,255,255),
                        ShadowColor = new Color(0,0,0),
                        CustomElements = new ()
                        {
                            {
                                GuiTextbox.CaretCustomElementId, Color.DodgerBlue
                            }
                        }
                    }
                }})
            },
            {
                GuiMenuBar.STypeId, new StateStyleMap(new(){
                {
                    GuiControlState.Idle, new Style()
                    {
                        BackgroundColor = new Color(30,30,30),
                        BorderColor = new Color(60,60,60),
                        TextColor = new Color(255,255,255),
                        CustomElements = new ()
                        {
                            {
                                GuiMenuBar.MenuItemCustomElementId, Color.DodgerBlue
                            }
                        }
                    }
                },
                {
                    GuiControlState.Hover, new Style()
                    {
                        BackgroundColor = new Color(30,30,30),
                        BorderColor = new Color(60,60,60),
                        TextColor = new Color(255,255,255),
                        CustomElements = new ()
                        {
                            {
                                GuiMenuBar.MenuItemCustomElementId, Color.DodgerBlue
                            }
                        }
                    }
                }})
            },
            {
                GuiWindow.STypeId, new StateStyleMap(new(){
                {
                    GuiControlState.Idle, new Style()
                    {
                        BackgroundColor = new Color(30,30,30),
                        BorderColor = new Color(60,60,60),
                        InnerBorderColor = new Color(40,40,40),
                        TextColor = new Color(255,255,255),
                        ShadowColor = new Color(0,0,0),
                        CustomElements = new ()
                        {
                            {
                                GuiWindow.TopBarCustomElementId, Color.DodgerBlue
                            }
                        }
                    }
                },
                {
                    GuiControlState.Hover, new Style()
                    {
                        BackgroundColor = new Color(30,30,30),
                        BorderColor = new Color(60,60,60),
                        InnerBorderColor = new Color(40,40,40),
                        TextColor = new Color(255,255,255),
                        ShadowColor = new Color(0,0,0),
                        CustomElements = new ()
                        {
                            {
                                GuiWindow.TopBarCustomElementId, Color.DodgerBlue
                            }
                        }
                    }
                }})
            },
            {
                GuiText.STypeId, new StateStyleMap(new(){
                {
                    GuiControlState.Idle, new Style()
                    {
                        TextColor = new Color(255,255,255),
                    }
                }})
            }
        };

        CustomStyles = new Dictionary<string, StateStyleMap>();
    }

    public StateStyleMap GetControlStyle(GuiControl control)
    {
        if (CustomStyles.TryGetValue(control.Id, out var style))
        {
            return style;
        }

        return BaseStyles[control.TypeId];

    }
}
