using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class ReplaceWithPrefab : MonoBehaviour
{
    [MenuItem("Tools/Replace Objects with Prefab in Specific Scene")]
    static void ReplaceObjectsWithPrefab()
    {
        // Seleccionar la escena específica
        string scenePath = "Assets/Scenes/Gate of People.unity"; // Cambia esto según sea necesario

        // Abre la escena
        if (!EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single).IsValid())
        {
            Debug.LogError("No se pudo abrir la escena especificada: " + scenePath);
            return;
        }

        // Aquí debes especificar el tag de los objetos que deseas reemplazar
        string tagToReplace = "ArbolPantano (7)"; // Cambia esto según sea necesario

        // Carga el prefab desde el proyecto
        string prefabPath = "Assets/Prefabs/ArbolPantano (7).prefab"; // Cambia esto según sea necesario
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

        if (prefab == null)
        {
            Debug.LogError("Prefab no encontrado en la ruta especificada: " + prefabPath);
            return;
        }

        // Encuentra todos los objetos en la escena con el tag especificado
        GameObject[] objectsToReplace = GameObject.FindGameObjectsWithTag(tagToReplace);

        if (objectsToReplace.Length == 0)
        {
            Debug.LogWarning("No se encontraron objetos con el tag especificado: " + tagToReplace);
            return;
        }

        foreach (GameObject obj in objectsToReplace)
        {
            // Guarda la posición y rotación del objeto original
            Vector3 position = obj.transform.position;
            Quaternion rotation = obj.transform.rotation;

            // Crea una instancia del prefab en la posición y rotación del objeto original
            GameObject newObject = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            newObject.transform.position = position;
            newObject.transform.rotation = rotation;

            // Destruye el objeto original
            DestroyImmediate(obj);
        }

        // Guarda la escena después de hacer los cambios
        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());

        Debug.Log("Reemplazo completado en la escena " + scenePath);
    }
}
