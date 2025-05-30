#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class MeshSaver : MonoBehaviour
{
    [ContextMenu("Save Combined Mesh")]
    public void SaveMesh()
    {
        MeshFilter mf = GetComponent<MeshFilter>();
        if (mf == null || mf.sharedMesh == null)
        {
            Debug.LogWarning("No MeshFilter or mesh found!");
            return;
        }

        string path = "Assets/CombinedMesh.asset";

        Mesh meshToSave = Instantiate(mf.sharedMesh);
        AssetDatabase.CreateAsset(meshToSave, path);
        AssetDatabase.SaveAssets();

        mf.sharedMesh = meshToSave;
        Debug.Log("Mesh saved to " + path);
    }
}
#endif