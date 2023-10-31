using System.Collections.Generic;
using FlatStage.Engine.Toolkit.Definitions.Gui;
using FlatStage.Graphics;

namespace FlatStage.Toolkit;

public delegate void OnMenuItemClickHandler(GuiMenuItem menuItem);

public class GuiMenuItem
{
    public required string Id { get; init; }

    public event OnMenuItemClickHandler? OnClick;

    public required string Label { get; init; }

    public bool IsExpanded { get; internal set; } = false;

    public bool HasSubMenus { get; init; } = false;

    internal Rect ItemRect { get; init; }

    public GuiMenuItem? Parent { get; init; }

    public required List<GuiMenuItem> Children { get; init; }

    public int Level { get; private set; }

    public bool IsHovered { get; set; }

    internal void ProcessLevel()
    {
        Level = 0;
        CalculateLevel(this);
    }

    private void CalculateLevel(GuiMenuItem item)
    {
        if (item.Parent == null)
        {
            return;
        }

        Level++;

        CalculateLevel(item.Parent);
    }

    internal bool TriggerClick()
    {
        if (OnClick != null)
        {
            OnClick.Invoke(this);
            return true;
        }

        return false;
    }

}

public class GuiMenuBar : GuiControl
{
    internal static readonly int STypeId;

    static GuiMenuBar()
    {
        STypeId = ++SBTypeId;
    }

    internal override int TypeId => STypeId;

    internal const string MenuItemCustomElementId = "menuItem";

    public override Size SizeHint => new(Canvas.Width, 30);

    private const int SubMenuPadding = 50;

    public GuiMenuBar(string id, Gui gui, GuiContainer? parent = null) : base(id, gui, parent)
    {
        MouseMoveEventBehavior = GuiMouseMoveEventBehavior.MouseOver;
    }

    internal List<GuiMenuItem> MenuItems => _menuItems;

    public void SetEventHandler(string menuId, OnMenuItemClickHandler action)
    {
        if (_menuItemsById.TryGetValue(menuId, out var menuItem))
        {
            menuItem.OnClick += action;
            return;
        }

        FlatException.Throw($"Could not find a menu with Id: {menuId}");

    }

    internal override void InitFromDefinition(GuiControlDef definition)
    {
        base.InitFromDefinition(definition);

        if (definition is GuiMenuBarDef menuBarDef)
        {
            int currentMenuItemX = 0;

            for (var i = 0; i < menuBarDef.MenuItems.Length; ++i)
            {
                var menuItem = menuBarDef.MenuItems[i];

                var menuItemLabelSize = Gui.Skin.MeasureText(menuItem.Label);

                var itemRect = new Rect(currentMenuItemX, 0, (int)(menuItemLabelSize.X + 20), Height);

                PopulateMenu(menuItem, itemRect, null);

                currentMenuItemX += itemRect.Width;
            }
        }
    }

    private void PopulateMenu(GuiMenuItemDef guiMenuItemDef, Rect itemRect, GuiMenuItem? parent = null)
    {
        var menuItem = new GuiMenuItem()
        {
            Id = guiMenuItemDef.Id,
            Label = guiMenuItemDef.Label,
            HasSubMenus = guiMenuItemDef.SubMenuItems != null,
            IsExpanded = false,
            ItemRect = itemRect,
            Parent = parent,
            Children = new List<GuiMenuItem>()
        };

        parent?.Children.Add(menuItem);

        menuItem.ProcessLevel();

        _menuItemsById.Add(menuItem.Id, menuItem);

        if (guiMenuItemDef.SubMenuItems != null)
        {
            int subMenuWidth = 0;

            int subMenuIndex = 0;

            foreach (var subMenuItemDef in guiMenuItemDef.SubMenuItems)
            {
                var subMenuItemLabelSize = Gui.Skin.MeasureText(subMenuItemDef.Label);

                if (subMenuItemLabelSize.X > subMenuWidth)
                {
                    subMenuWidth = (int)(subMenuItemLabelSize.X + (SubMenuPadding * 2));
                }
            }

            foreach (var subMenuItemDef in guiMenuItemDef.SubMenuItems)
            {
                var subItemRect = new Rect(itemRect.X + itemRect.Width, itemRect.Y + (itemRect.Height * subMenuIndex), subMenuWidth, Height);

                if (menuItem.Level == 0)
                {
                    subItemRect.X = itemRect.X;
                    subItemRect.Y += itemRect.Height;
                }

                subMenuIndex++;

                PopulateMenu(subMenuItemDef, subItemRect, menuItem);
            }
        }

        _menuItems.Add(menuItem);
    }

    protected override bool ProcessMouseMove(GuiMouseState mouseState)
    {
        var (localX, localY) = ToLocalPos(mouseState.MouseX, mouseState.MouseY);

        var refresh = false;

        foreach (var item in _menuItems)
        {
            if (item.Level == 0 || (item.Parent != null && item.Parent.IsExpanded))
            {
                if (CheckHovered(item, localX, localY))
                {
                    refresh = true;
                    break;
                }
            }
        }

        return refresh;
    }

    private bool CheckHovered(GuiMenuItem item, int localX, int localY)
    {
        if (item.ItemRect.Contains(localX, localY))
        {
            if (_hoveredItem != item)
            {
                ProcessHover(item, true);
                return true;
            }
        }
        else
        {
            if (item == _hoveredItem)
            {
                ProcessHover(item, false);
                return true;
            }
        }

        return false;
    }

    private void ProcessHover(GuiMenuItem item, bool hovered)
    {
        if (hovered)
        {
            if (_hoveredItem != null)
            {
                _hoveredItem.IsHovered = false;
                _hoveredItem = null;

            }

            item.IsHovered = true;
            _hoveredItem = item;

            if (item.HasSubMenus && MouseFocused)
            {
                ExpandItem(item, true);
            }
        }
        else
        {
            item.IsHovered = false;
            if (item.IsExpanded)
            {
                ExpandItem(item, false);
            }

            _hoveredItem = null;
        }
    }

    protected override bool ProcessMouseButton(GuiMouseState mouseState)
    {
        if (_hoveredItem == null)
        {
            Gui.MouseFocus(null);
            return true;
        }

        if (mouseState.MouseButtonDown == false)
        {

            if (_hoveredItem.Level == 0)
            {
                if (_hoveredItem.HasSubMenus)
                {
                    if (!_hoveredItem.IsExpanded)
                    {
                        ExpandItem(_hoveredItem, true);
                        Gui.MouseFocus(this);
                    }
                    else
                    {
                        ExpandItem(_hoveredItem, false);

                        if (!IsExpanded)
                        {
                            Gui.MouseFocus(null);
                        }
                    }
                }
                else
                {
                    _hoveredItem.TriggerClick();
                    CloseAllItems();
                }
            }
            else
            {
                if (!_hoveredItem.HasSubMenus)
                {
                    _hoveredItem.TriggerClick();
                    CloseAllItems();
                }
            }

        }

        return true;

    }

    protected override void ProcessMouseFocusChanged(bool focused)
    {
        if (!focused)
        {
            CloseAllItems();
        }
    }

    private void ExpandItem(GuiMenuItem item, bool expanded)
    {
        item.IsExpanded = expanded;

        if (expanded)
        {
            CloseSameLevelItems(item);
        }
    }

    private void CloseSameLevelItems(GuiMenuItem item)
    {
        for (var i = 0; i < _menuItems.Count; ++i)
        {
            var menuItem = _menuItems[i];

            if (menuItem != item && menuItem.Level == item.Level)
            {
                menuItem.IsExpanded = false;

                if (menuItem.Children.Count > 0)
                {
                    foreach (var child in menuItem.Children)
                    {
                        CloseSameLevelItems(child);
                    }
                }
            }
        }
    }

    private void CloseAllItems()
    {
        for (var i = 0; i < _menuItems.Count; ++i)
        {
            _menuItems[i].IsExpanded = false;
        }
    }

    protected override void ProcessMouseExited()
    {
        if (IsExpanded)
        {
            return;
        }

        if (_hoveredItem != null)
        {
            ProcessHover(_hoveredItem, false);
        }

        Gui.MouseFocus(null);
    }

    public override bool ContainsPoint(int x, int y)
    {
        return base.ContainsPoint(x, y) || IsExpanded;
    }

    internal override void Draw(Canvas canvas, GuiSkin skin)
    {
        skin.DrawMenuBar(canvas, this);
    }

    internal bool IsExpanded
    {
        get
        {
            for (int i = 0; i < _menuItems.Count; ++i)
            {
                if (_menuItems[i].IsExpanded)
                {
                    return true;
                }
            }

            return false;
        }
    }

    private GuiMenuItem? _hoveredItem = null;

    private readonly List<GuiMenuItem> _menuItems = new();
    private readonly Dictionary<string, GuiMenuItem> _menuItemsById = new();
}
