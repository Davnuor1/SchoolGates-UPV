using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
    [SerializeField] Camera camara;
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
    }

    
}
