using System;
using System.Runtime.InteropServices;

namespace NvARdotNet;

// typedef struct NvAR_FaceMesh {
//   NvAR_Vector3f *vertices;  ///< Mesh 3D vertex positions.
//   size_t num_vertices;
//   NvAR_Vector3u16 *tvi;     ///< Mesh triangle's vertex indices
//   size_t num_triangles;     ///< The number of triangles (previously num_tri_idx)
// } NvAR_FaceMesh;
/// <summary>This structure is returned as an output of the Mesh Tracking feature.</summary>
[StructLayout(LayoutKind.Sequential)]
public unsafe struct FaceMesh
{
    /// <summary>Mesh 3D vertex positions.</summary>
    public Vector3f* Vertices;

    /// <summary>The number of vertices.</summary>
    public int VertexCount
    {
        get => (int)numVerticies.ToUInt32();
        set
        {
            if (value < 0) throw new ArgumentOutOfRangeException(nameof(value));
            numVerticies = new UIntPtr((uint)value);
        }
    }
    private UIntPtr numVerticies;

    /// <summary>Mesh triangle's vertex indices.</summary>
    public Vector3u16* TriangleVertextIndices;

    /// <summary>The number of triangles.</summary>
    public int TriangleCount
    {
        get => (int)numTriangles.ToUInt32();
        set
        {
            if (value < 0) throw new ArgumentOutOfRangeException(nameof(value));
            numTriangles = new UIntPtr((uint)value);
        }
    }
    private UIntPtr numTriangles;
}
