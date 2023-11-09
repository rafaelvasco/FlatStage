using FlatStage.Graphics;
using System.Collections.Generic;

namespace FlatStage.Toolkit;
public class GuiTree : GuiControl
{
    public class GuiTreeNode
    {
        public required string Id { get; init; }

        public required string Label { get; set; }

        internal Rect Rect { get; set; }

        public bool Expanded { get; set; }

        public bool Hovered { get; internal set; }

        public GuiTreeNode? Parent { get; init; }

        internal bool HasChildren { get; init; }

        public int Level { get; private set; }

        public GuiTreeNode()
        {
        }

        internal void ProcessLevel()
        {
            Level = 0;
            CalculateLevel(this);
        }

        private void CalculateLevel(GuiTreeNode item)
        {
            if (item.Parent == null)
            {
                return;
            }

            Level++;

            CalculateLevel(item.Parent);
        }

    }

    internal static readonly int STypeId;

    public List<GuiTreeNode> Nodes => _nodes;

    static GuiTree()
    {
        STypeId = ++SBTypeId;
    }

    internal override int TypeId => STypeId;

    public GuiTree(string id, Gui gui, GuiContainer? parent) : base(id, gui, parent)
    {
        MouseMoveEventBehavior = GuiMouseMoveEventBehavior.MouseOver;
    }

    internal override void InitFromDefinition(GuiControlDef definition)
    {
        base.InitFromDefinition(definition);

        if (definition is GuiTreeDef treeDef)
        {
            for (var i = 0; i < treeDef.Nodes.Length; ++i)
            {
                var rootNodeDef = treeDef.Nodes[i];
                PopulateTreeNode(rootNodeDef, null);
            }
        }

        RecalculateNodeRects();
    }

    internal override void Draw(Canvas canvas, GuiSkin skin)
    {
        skin.DrawTree(canvas, this);
    }

    private void PopulateTreeNode(GuiTreeNodeDef nodeDef, GuiTreeNode? parent = null)
    {
        var treeNode = new GuiTreeNode()
        {
            Id = nodeDef.Id,
            Label = nodeDef.Label,
            Parent = parent,
            Expanded = false,
            HasChildren = nodeDef.ChildNodes != null
        };

        treeNode.ProcessLevel();
        _nodesById.Add(treeNode.Id, treeNode);

        _nodes.Add(treeNode);

        if (nodeDef.ChildNodes != null)
        {
            foreach (var childNodeDef in nodeDef.ChildNodes)
            {
                PopulateTreeNode(childNodeDef, treeNode);
            }
        }
    }

    protected override bool ProcessMouseMove(GuiMouseState mouseState)
    {
        var (localX, localY) = ToLocalPos(mouseState.MouseX, mouseState.MouseY);

        foreach (var node in _nodes)
        {
            if (node.Level > 0 && !node.Parent!.Expanded)
            {
                continue;
            }

            if (node.Rect.Contains(localX, localY) && _hoveredNode != node)
            {
                if (_hoveredNode != null)
                {
                    _hoveredNode.Hovered = false;
                }

                _hoveredNode = node;
                _hoveredNode.Hovered = true;
                return true;
            }
            else if (!node.Rect.Contains(localX, localY) && node == _hoveredNode)
            {
                _hoveredNode.Hovered = false;
                _hoveredNode = null;
                return true;
            }
        }

        return false;
    }

    protected override bool ProcessMouseButton(GuiMouseState mouseState)
    {
        if (mouseState.MouseButtonDown == false)
        {
            if (_hoveredNode != null && _hoveredNode.HasChildren)
            {
                ToggleNode(_hoveredNode);
                return true;
            }
        }

        return false;
    }

    protected override void ProcessMouseExited()
    {
        if (_hoveredNode != null)
        {
            _hoveredNode.Hovered = false;
            _hoveredNode = null;
        }
    }

    private void ToggleNode(GuiTreeNode node)
    {
        node.Expanded = !node.Expanded;

        if (!node.Expanded)
        {
            foreach (var nodeIterate in _nodes)
            {
                if (nodeIterate.Parent == node)
                {
                    nodeIterate.Expanded = false;
                }
            }
        }

        RecalculateNodeRects();
    }

    private void RecalculateNodeRects()
    {
        int currentNodeY = 0;
        var nodeIndicatorSize = Gui.Skin.StyleSheet.GetProperty<int>(this, DefaultStyleProperties.TreeNodeIndicatorSize);
        var nodeElementsMargin = Gui.Skin.StyleSheet.GetProperty<int>(this, DefaultStyleProperties.TreeNodeElementsMargin);
        var nodeSpacing = Gui.Skin.StyleSheet.GetProperty<int>(this, DefaultStyleProperties.TreeNodeSpacing);

        foreach (var node in _nodes)
        {
            CalculateNodeRect(node, ref currentNodeY, nodeIndicatorSize, nodeElementsMargin, nodeSpacing);
        }
    }

    private void CalculateNodeRect(GuiTreeNode node, ref int currentNodeY, int nodeIndicatorSize, int nodeElementsMargin, int nodeSpacing)
    {
        var labelSize = Gui.Skin.MeasureText(node.Label);

        if (node.Parent != null && node.Parent.Expanded)
        {
            node.Rect = new Rect(node.Parent.Rect.X + nodeIndicatorSize, currentNodeY, (int)(nodeIndicatorSize + labelSize.X + (3 * nodeElementsMargin)), nodeIndicatorSize + nodeSpacing);
            currentNodeY += node.Rect.Height;
        }
        else if (node.Parent == null)
        {
            node.Rect = new Rect(0, currentNodeY, (int)(nodeIndicatorSize + labelSize.X + (3 * nodeElementsMargin)), nodeIndicatorSize + nodeSpacing);
            currentNodeY += node.Rect.Height;

        }
    }

    public override Size SizeHint => new(200, 400);

    private GuiTreeNode? _hoveredNode = null;
    private readonly List<GuiTreeNode> _nodes = new();
    private readonly Dictionary<string, GuiTreeNode> _nodesById = new();
}
