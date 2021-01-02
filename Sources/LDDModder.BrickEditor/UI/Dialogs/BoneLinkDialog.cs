using BrightIdeasSoftware;
using LDDModder.BrickEditor.ProjectHandling;
using LDDModder.Modding;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LDDModder.BrickEditor.UI.Windows
{
    public partial class BoneLinkDialog : Form
    {
        public ProjectManager ProjectManager { get; set; }
        public List<PartBone> Bones { get; set; }

        private List<BoneLinkNode> BoneNodes { get; set; }
        private BoneLinkNode RootBone;

        public BoneLinkDialog()
        {
            InitializeComponent();
        }

        public BoneLinkDialog(ProjectManager projectManager)
        {
            Bones = projectManager.CurrentProject.Bones.ToList();
            ProjectManager = projectManager;

            InitializeComponent();

            HierarchyTreeView.CanExpandGetter = (m) =>
            {
                return (m as BoneLinkNode).ChildBones.Count > 0;
            };

            HierarchyTreeView.ChildrenGetter = (m) =>
            {
                return (m as BoneLinkNode).ChildBones;
            };

            UnassignedTreeView.CanExpandGetter = (m) =>
            {
                return (m as BoneLinkNode).ChildBones.Count > 0;
            };

            UnassignedTreeView.ChildrenGetter = (m) =>
            {
                return (m as BoneLinkNode).ChildBones;
            };

            HierarchyTreeView.TreeColumnRenderer = new CustomTreeRenderer()
            {
                IndentAmount = 8,
                UseTriangles = true
            };
            UnassignedTreeView.TreeColumnRenderer = new CustomTreeRenderer()
            {
                IndentAmount = 8,
                UseTriangles = true
            };

            HierarchyTreeView.DropSink = new CustomDropHandler();
            UnassignedTreeView.DropSink = new CustomDropHandler();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            LoadBoneNodes();

        }

        private void LoadBoneNodes()
        {
            BoneNodes = new List<BoneLinkNode>();

            foreach (var bone in Bones)
            {
                var parentBone = Bones.FirstOrDefault(x => x.BoneID == bone.TargetBoneID);

                BoneNodes.Add(new BoneLinkNode()
                {
                    ID = bone.ID,
                    Name = bone.Name,
                    ParentID = parentBone?.ID,
                    BoneID = bone.BoneID
                });
            }


            var remainingBones = BoneNodes.ToList();

            void LinkChildBones(BoneLinkNode parent)
            {
                var childs = remainingBones.Where(x => x.ParentID == parent.ID).ToList();
                if (!childs.Any())
                    return;
                parent.ChildBones.AddRange(childs);
                remainingBones.RemoveAll(x => x.ParentID == parent.ID);
                foreach (var child in childs)
                    LinkChildBones(child);
            }

            RootBone = BoneNodes.FirstOrDefault(b => b.BoneID == 0);

            if (RootBone != null)
            {
                remainingBones.Remove(RootBone);

                LinkChildBones(RootBone);
                HierarchyTreeView.AddObject(RootBone);
            }

            if (remainingBones.Any())
                UnassignedTreeView.SetObjects(remainingBones);

            HierarchyTreeView.ExpandAll();
        }

        class BoneLinkNode
        {
            public string ID { get; set; }
            public string ParentID { get; set; }
            public string Name { get; set; }
            public int BoneID { get; set; }
            public int TargetBoneID { get; set; }
            public List<BoneLinkNode> ChildBones { get; set; }

            public BoneLinkNode()
            {
                TargetBoneID = -1;
                ChildBones = new List<BoneLinkNode>();
            }

            public IEnumerable<BoneLinkNode> GetHierarchy(bool includeSelf = false)
            {
                if (includeSelf)
                    yield return this;

                foreach (var child in ChildBones)
                {
                    yield return child;
                    foreach (var subchild in child.GetHierarchy(false))
                        yield return subchild;
                }
            }

            public IEnumerable<BoneLinkNode> GetHierarchy2(bool includeSelf = false)
            {
                if (includeSelf)
                    yield return this;

                foreach (var child in ChildBones)
                    yield return child;

                foreach (var child in ChildBones)
                {
                    foreach (var subchild in child.GetHierarchy(false))
                        yield return subchild;
                }
            }

            public void UnlinkChildrens()
            {
                foreach (var child in ChildBones)
                    child.ParentID = string.Empty;
                ChildBones.Clear();
            }
        }

        class CustomTreeRenderer : TreeListView.TreeRenderer
        {
            public int IndentAmount { get; set; }


            public CustomTreeRenderer() : base()
            {
                IndentAmount = 17;
            }

            public override void Render(Graphics g, Rectangle r)
            {
                DrawBackground(g, r);
                var branch = TreeListView.TreeModel.GetBranch(base.RowObject);
                Rectangle rectangle = ApplyCellPadding(r);
                Rectangle rectangle2 = rectangle;
                rectangle2.Offset((branch.Level - 1) * IndentAmount - (17 - IndentAmount) / 2, 0);
                rectangle2.Width = 17;
                rectangle2.Height = 17;
                rectangle2.Y = AlignVertically(rectangle, rectangle2);
                int glyphMidVertical = rectangle2.Y + rectangle2.Height / 2;
                if (IsShowLines)
                {
                    DrawLines(g, r, LinePen, branch, glyphMidVertical);
                }
                if (branch.CanExpand && IsShowGlyphs)
                {
                    DrawExpansionGlyph(g, rectangle2, branch.IsExpanded);
                }
                int num = branch.Level * IndentAmount;
                rectangle.Offset(num, 0);
                rectangle.Width -= num;
                DrawImageAndText(g, rectangle);
                //PIXELS_PER_LEVEL = IndentAmount;
                //base.Render(g, r);
                //PIXELS_PER_LEVEL = 17;
            }

            protected override void DrawLines(Graphics g, Rectangle r, Pen p, TreeListView.Branch br, int glyphMidVertical)
            {
                bool mustRevert = IndentAmount != PIXELS_PER_LEVEL;
                PIXELS_PER_LEVEL = IndentAmount;
                base.DrawLines(g, r, p, br, glyphMidVertical);
                if (mustRevert)
                    PIXELS_PER_LEVEL = 17;
            }

            protected override void HandleHitTest(Graphics g, OlvListViewHitTestInfo hti, int x, int y)
            {
                PIXELS_PER_LEVEL = IndentAmount;
                base.HandleHitTest(g, hti, x, y);
                PIXELS_PER_LEVEL = 17;
            }
        }

        class CustomDropHandler : BrightIdeasSoftware.SimpleDropSink
        {
            protected override Rectangle CalculateDropTargetRectangle(OLVListItem item, int subItem)
            {
                var baseRect = base.CalculateDropTargetRectangle(item, subItem);
                if (item.IndentCount > 0 && 
                    item.ListView is TreeListView tlv &&
                    tlv.TreeColumnRenderer is CustomTreeRenderer ctr)
                {
                    int wrongIndent = item.IndentCount * tlv.SmallImageSize.Width;
                    int correctIndent = item.IndentCount * ctr.IndentAmount;
                    int diff = wrongIndent - correctIndent;
                    baseRect.X -= diff;
                    baseRect.Width += diff;
                }
                return baseRect;
            }

        }

        private void BonesTreeViews_ModelCanDrop(object sender, BrightIdeasSoftware.ModelDropEventArgs e)
        {
            var draggedBones = e.SourceModels.OfType<BoneLinkNode>().ToList();
            var allBones = draggedBones.SelectMany(x => x.GetHierarchy(true)).Distinct().ToList();
            var targetBone = e.TargetModel as BoneLinkNode;

            if (e.SourceListView == HierarchyTreeView && 
                sender == HierarchyTreeView && RootBone != null)
            {
                if (e.SourceModels.Contains(RootBone))
                {
                    e.Effect = DragDropEffects.None;
                    return;
                }
            }

            if (e.SourceListView == UnassignedTreeView && sender == UnassignedTreeView)
            {
                e.Effect = DragDropEffects.None;
                return;
            }

            if (targetBone != null && allBones.Contains(targetBone))
            {
                e.Effect = DragDropEffects.None;
                return;
            }

            if (e.DropTargetLocation == DropTargetLocation.Item)
                e.Effect = DragDropEffects.Move;
            else if(e.DropTargetLocation == DropTargetLocation.Background ||
                e.DropTargetLocation == DropTargetLocation.None)
            {
                if (sender == UnassignedTreeView)
                    e.Effect = DragDropEffects.Move;
                else if (RootBone == null && e.SourceModels.Count == 1)
                    e.Effect = DragDropEffects.Move;
            }
        }

        private void BonesTreeViews_ModelDropped(object sender, BrightIdeasSoftware.ModelDropEventArgs e)
        {
            void UnlinkBone(BoneLinkNode node)
            {
                node.ParentID = string.Empty;
                var parentBone = BoneNodes.FirstOrDefault(x => x.ChildBones.Contains(node));
                if (parentBone != null)
                {
                    parentBone.ChildBones.Remove(node);
                    HierarchyTreeView.RefreshObject(parentBone);
                }
            }

            var draggedBones = e.SourceModels.OfType<BoneLinkNode>().ToList();

            if (sender == UnassignedTreeView)
            {
                var allBones = draggedBones.SelectMany(x => x.GetHierarchy(true)).Distinct().ToList();
                
                foreach (BoneLinkNode node in allBones)
                {
                    //node.ParentID = string.Empty;
                    node.UnlinkChildrens();
                }

                foreach (BoneLinkNode node in draggedBones)
                    UnlinkBone(node);

                UnassignedTreeView.AddObjects(allBones);
            }
            else if (sender == HierarchyTreeView)
            {
                if (RootBone == null)
                {
                    RootBone = e.SourceModels[0] as BoneLinkNode;
                    HierarchyTreeView.AddObject(RootBone);
                }
                else if (e.TargetModel is BoneLinkNode targetBone)
                {

                    foreach (BoneLinkNode node in draggedBones)
                    {
                        if (string.IsNullOrEmpty(node.ParentID))
                            UnassignedTreeView.RemoveObject(node);

                        UnlinkBone(node);
                        targetBone.ChildBones.Add(node);
                        node.ParentID = targetBone.ID;
                        
                    }
                    HierarchyTreeView.RefreshObject(targetBone);
                }
            }


        }

        private void ApplyButton_Click(object sender, EventArgs e)
        {
            if (RootBone == null)
                return;

            int currentID = 0;
            ProjectManager.ClearBoneConnections();

            foreach (var boneNode in RootBone.GetHierarchy2(true))
            {
                boneNode.BoneID = currentID++;
                foreach (var child in boneNode.ChildBones)
                    child.TargetBoneID = boneNode.BoneID;

                var bone = Bones.First(x => x.ID == boneNode.ID);
                bone.BoneID = boneNode.BoneID;

                if (boneNode.TargetBoneID >= 0)
                    bone.TargetBoneID = boneNode.TargetBoneID;
                else
                    bone.TargetBoneID = -1;
            }

            ProjectManager.CurrentProject.Bones.Sort(x => x.BoneID);

            DialogResult = DialogResult.OK;
        }

        private void HierarchyTreeView_Expanded(object sender, TreeBranchExpandedEventArgs e)
        {
            AdjustColumnWidth();
        }

        private void AdjustColumnWidth()
        {
            int maxIndent = 0;
            foreach (var test in HierarchyTreeView.ExpandedObjects)
            {
                int itemIndex = HierarchyTreeView.IndexOf(test);
                if (itemIndex == -1)
                    continue;
                maxIndent = Math.Max(maxIndent, HierarchyTreeView.GetItem(itemIndex).IndentCount);
            }
            olvColumn1.Width = (maxIndent * 8) + 100;
        }

        private void HierarchyTreeView_Collapsed(object sender, TreeBranchCollapsedEventArgs e)
        {
            AdjustColumnWidth();
        }
    }
}
