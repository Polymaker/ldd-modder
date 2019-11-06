using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.LDD.Meshes
{
    public enum MeshCullingType
    {
        /// <summary>
        /// Specifies which vertices and indices are part of a stud.
        /// </summary>
        MaleStud = 1,
        /// <summary>
        /// Defines which vertices and indices are part of the main model.
        /// </summary>
        MainModel = 2,
        /// <summary>
        /// Specifies which vertices and indices are part of the "cavity" where other bricks connects.
        /// Often includes 3D data that "simplifies" the model (e.g. a flat polygon to fill the cavity).
        /// </summary>
        FemaleStud = 4,
        /// <summary>
        /// Specifies which vertices and indices are part of the tubes where other bricks connects.
        /// Sometimes includes 3D data.
        /// </summary>
        BrickTube = 8,
    }
}
