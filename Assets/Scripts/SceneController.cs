using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    //public Vector2 posicion;
    [SerializeField] public GameObject playerPrefab;
    public Vector2 enQuePosicion;

    
    public void ChangeScene(int sceneIndex,Vector2 posicionInicio)
    {
        enQuePosicion = posicionInicio;
        SceneManager.LoadScene(sceneIndex);
        //spawnPlayer(posicionInicio, playerPrefab);
        
    }
    public void spawnPlayer(Vector2 posicionInicio, GameObject playerPrefab)
    {
        Instantiate(playerPrefab, (posicionInicio), Quaternion.identity);
    }
}
