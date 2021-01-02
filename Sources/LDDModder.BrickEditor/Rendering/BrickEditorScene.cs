using LDDModder.BrickEditor.Rendering.UI;
using LDDModder.Modding;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Rendering
{
    public class BrickEditorScene : IDisposable
    {
        public Camera Camera { get; set; }

        public Vector2 ViewSize { get; private set; }

        public OpenTK.GLControl GLControl { get; private set; }

        public ThreadSafeList<ModelBase> Models { get; private set; }

        public List<UIElement> UIElements { get; private set; }

        public BrickEditorScene()
        {
            Models = new ThreadSafeList<ModelBase>();
            UIElements = new List<UIElement>();
        }

        public void BindGL(OpenTK.GLControl control)
        {
            GLControl = control;
            GLControl.SizeChanged += GLViewSizeChanged;
        }

        private void GLViewSizeChanged(object sender, EventArgs e)
        {
            UpdateViewSize();
        }

        private void UpdateViewSize()
        {
            ViewSize = new Vector2(GLControl.Width, GLControl.Height);
        }

        public void Dispose()
        {
            Models.ForEach(x => x.Dispose());
            Models.Clear();

            if (GLControl != null)
            {
                GLControl.SizeChanged -= GLViewSizeChanged;
            }
        }

        #region Model Management

        public PartElementModel GetElementModel(PartElement element)
        {
            return Models.OfType<PartElementModel>().FirstOrDefault(x => x.Element == element);
        }

        #endregion


    }
}
