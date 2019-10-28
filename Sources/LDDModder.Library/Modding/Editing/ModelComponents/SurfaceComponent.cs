using LDDModder.LDD.Files;
using LDDModder.LDD.Meshes;
using LDDModder.LDD.Primitives.Connectors;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace LDDModder.Modding.Editing
{
    [XmlInclude(typeof(PartModel)), 
        XmlInclude(typeof(MaleStudModel)), 
        XmlInclude(typeof(FemaleStudModel)), 
        XmlInclude(typeof(BrickTubeModel))]
    public abstract class SurfaceComponent : PartComponent
    {
        public const string NODE_NAME = "Component";

        public abstract ModelComponentType ComponentType { get; }

        public ComponentCollection<ModelMesh> Geometries { get; }

        public SurfaceComponent()
        {
            Geometries = new ComponentCollection<ModelMesh>(this);
        }

        public static SurfaceComponent FromLDD(MeshFile mesh, MeshCulling culling)
        {
            var geom = mesh.GetCullingGeometry(culling, false);
            SurfaceComponent component = null;


            switch (culling.Type)
            {
                case MeshCullingType.MainModel:
                    {
                        component = new PartModel();
                        break;
                    }

                case MeshCullingType.Stud:
                    {
                        var item = new MaleStudModel();

                        if (culling.Studs.Count == 1)
                        {
                            item.Stud = new StudReference(culling.Studs[0]);
                            //item.ConnectionIndex = culling.Studs[0].ConnectorIndex;
                        }
                        else
                            Debug.WriteLine("Stud culling does not reference a stud!");

                        component = item;
                        break;
                    }

                case MeshCullingType.FemaleStud:
                    {
                        var item = new FemaleStudModel();

                        if (culling.ReplacementMesh != null)
                            item.ReplacementGeometries.Add(new ModelMesh(culling.ReplacementMesh));
                        
                        foreach (var studInfo in culling.Studs)
                            item.Studs.Add(new StudReference(studInfo));

                        component = item;
                        break;
                    }

                case MeshCullingType.Tube:
                    {
                        var item = new BrickTubeModel();

                        if (culling.Studs.Count == 1)
                        {
                            item.TubeStud = new StudReference(culling.Studs[0]);
                        }
                        else
                            Debug.WriteLine("Tube culling does not reference a stud!");

                        if (culling.AdjacentStuds.Any())
                        {
                            foreach (var fIdx in culling.AdjacentStuds[0].FieldIndices)
                            {
                                item.AdjacentStuds.Add(new StudReference(culling.AdjacentStuds[0].ConnectorIndex,
                                    fIdx.Index, fIdx.Value2, fIdx.Value4));
                            }
                        }

                        component = item;
                        break;
                    }
            }
            
            if (component is PartCullingModel cullingComponent)
            {
                var connectorRef = culling.Studs.FirstOrDefault() ?? culling.AdjacentStuds.FirstOrDefault();
                cullingComponent.ConnectionIndex = connectorRef != null ? connectorRef.ConnectorIndex : -1;
            }

            if (component != null)
            {
                component.Geometries.Add(new ModelMesh(geom));
            }
            return component;
        }

        public virtual IEnumerable<ModelMesh> GetAllMeshes()
        {
            return Geometries;
        }

        #region Xml Serialization

        public override XElement SerializeToXml()
        {
            var elem = SerializeToXmlBase(NODE_NAME);
            elem.Add(new XAttribute("Type", ComponentType.ToString()));

            var geomElem = elem.AddElement("Geometries");

            foreach (var geom in Geometries)
                geomElem.Add(geom.SerializeToXml());

            return elem;
        }

        protected internal override void LoadFromXml(XElement element)
        {
            base.LoadFromXml(element);
            var geomElem = element.Element("Geometries");

            if (geomElem != null)
            {
                foreach (var elem in geomElem.Elements(ModelMesh.NODE_NAME))
                {
                    var mesh = new ModelMesh();
                    mesh.LoadFromXml(elem);
                    Geometries.Add(mesh);
                }
            }
        }

        public static SurfaceComponent FromXml(XElement element)
        {
            SurfaceComponent component = null;
            var componentType = element.ReadAttribute<ModelComponentType>("Type");
            switch (componentType)
            {
                case ModelComponentType.Part:
                    component = new PartModel();
                    break;
                case ModelComponentType.MaleStud:
                    component = new MaleStudModel();
                    break;
                case ModelComponentType.FemaleStud:
                    component = new FemaleStudModel();
                    break;
                case ModelComponentType.BrickTube:
                    component = new BrickTubeModel();
                    break;
            }

            if (component != null)
                component.LoadFromXml(element);

            return component;
        }

        #endregion
    }
}
