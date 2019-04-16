using UnityEngine;

public class UpdateMeshCollider : MonoBehaviour
{

    Mesh mesh;

    MeshCollider meshCollider;

    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        meshCollider = GetComponent<MeshCollider>();
        if (!mesh || !meshCollider)

        {
            Debug.LogError("Assign a mesh in the inspector");
            return;
        }
    }

    public void RecalculateBounds()
    {
        meshCollider.sharedMesh = mesh;
        Debug.Log("Shared mesh recalculated");
    }
}
