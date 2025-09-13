using UnityEngine;

public class CinematicTrigger : MonoBehaviour
{
    public CinematicManager cinematicManager; // Referencia al manager de la cinemática
    public CinematicData cinematicData; // Referencia al ScriptableObject de la cinemática

    private void Start()
    {
        var key = cinematicData.name.Substring(0, cinematicData.name.Length - 2);
        if (PlayerPrefs.GetInt("Cinematic_" + key, 0) == 0) // Si no se ha visto
        {
            cinematicManager.gameObject.SetActive(true); // Activar cinemática
        }
    }
}
