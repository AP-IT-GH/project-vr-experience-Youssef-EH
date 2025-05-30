using UnityEngine;

public class MeshCombiner : MonoBehaviour
{
    [ContextMenu("Combine Meshes")]
    void Combine()
    {
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        for (int i = 0; i < meshFilters.Length; i++)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
        }

        GameObject combinedObject = new GameObject("CombinedMesh");
        combinedObject.transform.position = Vector3.zero;

        MeshFilter mf = combinedObject.AddComponent<MeshFilter>();
        mf.sharedMesh = new Mesh();
        mf.sharedMesh.CombineMeshes(combine);

        combinedObject.AddComponent<MeshRenderer>().sharedMaterial = meshFilters[0].GetComponent<Renderer>().sharedMaterial;
        combinedObject.AddComponent<MeshCollider>().sharedMesh = mf.sharedMesh;

        // Disable original pieces
        foreach (MeshFilter mfOld in meshFilters)
        {
            if (mfOld.gameObject != this.gameObject)
                mfOld.gameObject.SetActive(false);
        }
    }
}
