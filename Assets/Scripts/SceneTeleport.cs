using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTeleport : MonoBehaviour
{
    [SerializeField] public Vector2 posicionSiguiente;
    [SerializeField] private Animator ChangeSceneAnimator;
    [SerializeField] public int aQueEscena;
    private void OnTriggerEnter2D(Collider2D collision)
    {

        //GameManager.instance.player.ChangePositionPlayer(posicionSiguiente);
        GameManager.instance.sceneController.ChangeScene(aQueEscena, posicionSiguiente);
    }
}
