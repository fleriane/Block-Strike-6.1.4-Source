using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Linq;

public class Tools : MonoBehaviour
{
    [MenuItem("Tools/More/GUILayer Fix")]
    static void GUILayerFix()
    {
        var sceneGUIDs = AssetDatabase.FindAssets("t:Scene");
        string[] scenePaths = sceneGUIDs.Select(i => AssetDatabase.GUIDToAssetPath(i)).ToArray();

        for (int i = 0; i < scenePaths.Length; i++)
        {
            var scene = EditorSceneManager.OpenScene(scenePaths[i], OpenSceneMode.Additive);

            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);
            
            if (scene != EditorSceneManager.GetActiveScene())
			{
                EditorSceneManager.UnloadSceneAsync(scene);
			}	
        }
    }
	
	[MenuItem("Tools/More/Mesh Fix")]
    static void MeshFix(MenuCommand command)
    {
        for (int i = 0; i < GameObject.FindObjectsOfType<MeshFilter>().Length; i++)
        {
            if (GameObject.FindObjectsOfType<MeshFilter>()[i].gameObject.GetComponent<MeshCollider>() != null)
            {
                GameObject.FindObjectsOfType<MeshFilter>()[i].mesh = GameObject.FindObjectsOfType<MeshFilter>()[i].gameObject.GetComponent<MeshCollider>().sharedMesh;
            } 
        }
    }
}
