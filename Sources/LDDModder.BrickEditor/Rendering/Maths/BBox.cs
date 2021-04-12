using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Rendering
{
    public struct BBox
    {
        public Vector3 Extents;
        public Vector3 Center;

        public static readonly BBox Empty = new BBox();

        //public Vector3 Center => ((Max - Min) / 2f) + Min;

        public float SizeX => Max.X - Min.X;
        public float SizeY => Max.Y - Min.Y;
        public float SizeZ => Max.Z - Min.Z;

        public Vector3 Size
        {
            get => Extents * 2f;
            set => Extents = value * 0.5f;
        }

        public bool IsEmpty => Size == Vector3.Zero;

        #region Calculated bounds

        public Vector3 Min
        {
            get { return Center - Extents; }
            set
            {
                SetMinMax(value, Max);
            }
        }

        public Vector3 Max
        {
            get { return Center + Extents; }
            set
            {
                SetMinMax(Min, value);
            }
        }

        /// <summary>
        /// Minimum X
        /// </summary>
        public float Left
        {
            get { return Min.X; }
            set
            {
                SetMinMax(new Vector3(value, Bottom, Back), Max);
            }
        }

        /// <summary>
        /// Maximum X
        /// </summary>
        public float Right
        {
            get { return Max.X; }
            set
            {
                SetMinMax(Min, new Vector3(value, Top, Front));
            }
        }

        /// <summary>
        /// Maximum Y
        /// </summary>
        public float Top
        {
            get { return Max.Y; }
            set
            {
                SetMinMax(Min, new Vector3(Right, value, Front));
            }
        }

        /// <summary>
        /// Minimum Y
        /// </summary>
        public float Bottom
        {
            get { return Min.Y; }
            set
            {
                SetMinMax(new Vector3(Left, value, Back), Max);
            }
        }

        public float Front
        {
            get { return Max.Z; }
            set
            {
                SetMinMax(Min, new Vector3(Right, Top, value));
            }
        }

        public float Back
        {
            get { return Min.Z; }
            set
            {
                SetMinMax(new Vector3(Left, Bottom, value), Max);
            }
        }

        #endregion

        public static BBox FromMinMax(Vector3 min, Vector3 max)
        {
            var box = new BBox();
            box.SetMinMax(min, max);
            return box;
        }

        public static BBox FromCenterSize(Vector3 center, Vector3 size)
        {
            return new BBox
            {
                Center = center,
                Extents = size / 2f
            };
        }

        public static BBox FromVertices(IEnumerable<Vector3> vertices)
        {
            Vector3 minPos = new Vector3(99999f);
            Vector3 maxPos = new Vector3(-99999f);

            foreach (var v in vertices)
            {
                minPos = Vector3.ComponentMin(minPos, v);
                maxPos = Vector3.ComponentMax(maxPos, v);
            }

            return FromMinMax(minPos, maxPos);
        }

        public void SetMinMax(Vector3 min, Vector3 max)
        {
            Extents = (max - min) * 0.5f;
            Center = min + Extents;
        }

        public Vector3[] GetCorners()
        {
            var corners = new Vector3[8];
            var axes = new Vector3[]
            {
                new Vector3(1,0,1),
                new Vector3(-1,0,1),
                new Vector3(-1,0,-1),
                new Vector3(1,0,-1),
            };

            for (int i = 0; i < 4; i++)
            {
                var offset = Extents * axes[i];
                corners[(i * 2)] = Center + offset + (Vector3.UnitY * Extents.Y);
                corners[(i * 2) + 1] = Center + offset + (Vector3.UnitY * -Extents.Y);
            }

            return corners;
        }
    
        public static BBox Combine(IEnumerable<BBox> boundingBoxes)
        {
            Vector3 minPos = new Vector3(99999f);
            Vector3 maxPos = new Vector3(-99999f);

            foreach (var bbox in boundingBoxes)
            {
                minPos = Vector3.ComponentMin(minPos, bbox.Min);
                maxPos = Vector3.ComponentMax(maxPos, bbox.Max);
            }

            return FromMinMax(minPos, maxPos);
        }

        public static BBox Transform(Matrix4 transform, BBox box)
        {
            var corners = box.GetCorners();
            for (int i = 0; i < 8; i++)
                corners[i] = Vector3.TransformPosition(corners[i], transform);
            return FromVertices(corners);
        }
    }
}
