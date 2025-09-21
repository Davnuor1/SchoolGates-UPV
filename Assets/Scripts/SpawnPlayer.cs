using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
    [SerializeField] Camera camara;
    [SerializeField] GameObject panelNombreEscena;
    [SerializeField] string textoNombreEscena;
    [SerializeField] TextMeshProUGUI textMeshNombreEscena;
    [SerializeField] SimpleTextTable tablaLanguage;

    // Start is called before the first frame update
    void Start()
    {
        GameObject player=Instantiate(playerPrefab, (GameManager.instance.sceneController.enQuePosicion), Quaternion.identity);
        Vector3 newCameraPosition = new Vector3(player.transform.position.x, player.transform.position.y, camara.transform.position.z);
        camara.transform.position = newCameraPosition;

        CameraFollow cameraFollow = camara.GetComponent<CameraFollow>();
        if (cameraFollow != null)
        {
            
            cameraFollow.target = player.transform;
        }
        Player playerGM = player.GetComponent<Player>();
        GameManager.instance.player = playerGM;

        if (textoNombreEscena!= null)
        {
            Debug.Log("estamos definiendo el nombre de la escena");
            var lang = LocalizationManager.Instance.CurrentLanguage;
            panelNombreEscena.SetActive(true);
            textoNombreEscena = tablaLanguage.Get(textoNombreEscena, lang);
            textMeshNombreEscena.text = textoNombreEscena;
            StartCoroutine(DesactivarPanelDespuesDeTiempo(5f)); // Llama a la corrutina para esperar 5 segundos
        }
    }

    // Corrutina para desactivar el panel después de un tiempo especificado
    private IEnumerator DesactivarPanelDespuesDeTiempo(float segundos)
    {
        yield return new WaitForSeconds(segundos); // Espera los segundos especificados
        panelNombreEscena.SetActive(false); // Desactiva el panel
    }
}

    

