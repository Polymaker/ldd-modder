using Newtonsoft.Json;

namespace LDDModder.BrickEditor.Settings
{
    public class ViewportDisplaySettings
    {
        [JsonProperty("meshes.visible")]
        public bool ShowPartModels { get; set; }
        [JsonProperty("meshes.render_mode")]
        public Rendering.MeshRenderMode PartRenderMode { get; set; }
        [JsonProperty("collisions.visible")]
        public bool ShowCollisions { get; set; }
        [JsonProperty("collisions.visible")]
        public bool ShowConnections { get; set; }
        //[JsonProperty("collisions.visible")]
        //public bool RestoreDefaultsOnProjectChange { get; set; }
    }
}
