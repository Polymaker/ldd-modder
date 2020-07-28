using LDDModder.BrickEditor.Rendering.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Rendering
{
    public class SceneInfo : IDisposable
    {
        public Camera Camera { get; set; }

        public List<ModelBase> Models { get; private set; }
        public List<UIElement> UIElements { get; private set; }

        public SceneInfo()
        {
            Models = new List<ModelBase>();
            UIElements = new List<UIElement>();
        }

        public void Dispose()
        {
            Models.ForEach(x => x.Dispose());
            Models.Clear();
        }
    }
}
