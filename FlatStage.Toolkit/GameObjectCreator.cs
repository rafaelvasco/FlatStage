namespace FlatStage.Toolkit;

public delegate GameObject CreatorMethod(GameObjectDef definition, GameObject? parent = null);

internal static class GameObjectCreator
{
    private static readonly Dictionary<Type, CreatorMethod> _definitionCreators;

    public static GameObject CreateFromDefinition(GameObjectDef definition, GameObject? parent = null)
    {
        if (_definitionCreators.TryGetValue(definition.GetType(), out var creator))
        {
            return creator(definition, parent);
        }

        FlatException.Throw($"No creation method defined for this Definition: {definition.GetType()}");

        return default!;
    }


    static void RegisterOnGuiIfHasGuiRoot(GameObject gameObject)
    {
        var gui = FindRootGui(gameObject);
        gui?.Register(gameObject);
    }

    static GameObjectCreator()
    {
        _definitionCreators = new Dictionary<Type, CreatorMethod>()
        {
            {
                typeof(GameObjectDef), (definition, parent) =>
                {
                    var gameObject = new GameObject(definition.Id);
                    gameObject.InitFromDefinition(definition);
                    parent?.Add(gameObject);

                    if (definition.Children == null) return gameObject;

                    foreach (var childDef in definition.Children)
                    {
                        CreateFromDefinition(childDef, gameObject);
                    }

                    return gameObject;
                }
            },
            {
                typeof(SpriteDef), (definition, parent) =>
                {
                    var spriteDef = (definition as SpriteDef)!;

                    var sprite = new Sprite(spriteDef.Id);
                    sprite.InitFromDefinition(spriteDef);
                    parent?.Add(sprite);

                    RegisterOnGuiIfHasGuiRoot(sprite);

                    return sprite;
                }
            },
            {
                typeof(TextDef), (definition, parent) =>
                {
                    var textDef = (definition as TextDef)!;

                    var text = new Text(textDef.Id);
                    text.InitFromDefinition(textDef);

                    parent?.Add(text);

                    RegisterOnGuiIfHasGuiRoot(text);

                    return text;
                }
            },
            {
                typeof(LayoutDef), (definition, parent) =>
                {
                    var layoutDef = (definition as LayoutDef)!;

                    var gui = FindRootGui(parent);

                    if (gui == null)
                    {
                        FlatException.Throw("GuiControl must be inside a Gui Tree");
                        return null!;
                    }

                    switch (layoutDef.Mode)
                    {
                        case LayoutMode.Horizontal:
                            var horizontalLayout = new HorizontalLayout(layoutDef.Id, gui);
                            horizontalLayout.InitFromDefinition(layoutDef);
                            parent?.Add(horizontalLayout);

                            if (layoutDef.Children == null) return horizontalLayout;

                            foreach (var childDef in layoutDef.Children)
                            {
                                CreateFromDefinition(childDef, horizontalLayout);
                            }

                            return horizontalLayout;

                        case LayoutMode.Vertical:

                            var verticalLayout = new VerticalLayout(layoutDef.Id, gui);

                            parent?.Add(verticalLayout);

                            verticalLayout.InitFromDefinition(layoutDef);

                            if (layoutDef.Children == null) return verticalLayout;

                            foreach (var childDef in layoutDef.Children)
                            {
                                CreateFromDefinition(childDef, verticalLayout);
                            }
                            return verticalLayout;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            },
            {
                typeof(GuiDef), (definition, parent) =>
                {
                    var guiDef = (definition as GuiDef)!;

                    var gui = new Gui(guiDef.Id);
                    gui.InitFromDefinition(guiDef);

                    parent?.Add(gui);

                    if (guiDef.Children == null) return gui;

                    foreach (var childDef in guiDef.Children)
                    {
                        CreateFromDefinition(childDef, gui);
                    }

                    return gui;
                }
            },
            {
                typeof(GuiButtonDef), (definition, parent) =>
                {
                    var buttonDef = (definition as GuiButtonDef)!;

                    var gui = FindRootGui(parent);

                    if (gui == null)
                    {
                        FlatException.Throw("GuiControl must be inside a Gui Tree");
                        return null!;
                    }

                    var button = new GuiButton(buttonDef.Id, gui);
                    button.InitFromDefinition(buttonDef);
                    parent?.Add(button);

                    return button;
                }
            }
        };
    }

    private static Gui? FindRootGui(GameObject? node)
    {
        if (node == null)
        {
            return null;
        }

        Gui? gui = null;

        if (node is Gui parentGui)
        {
            gui = parentGui;
        }
        else
        {
            var parentSearch = node.Parent;

            while (parentSearch != null)
            {
                if (parentSearch is Gui parentGuiSearch)
                {
                    gui = parentGuiSearch;
                    break;
                }

                parentSearch = parentSearch.Parent;
            }
        }

        return gui;
    }
}
