using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassScene : MonoBehaviour
{
    [SerializeField] public Vector2 posicionSiguiente;
    [SerializeField] private Animator ChangeSceneAnimator;
    [SerializeField] public int aQueEscena;
    private void OnTriggerEnter2D(Collider2D collision)
    {

        //GameManager.instance.player.ChangePositionPlayer(posicionSiguiente);
        ChangeSceneAnimator.SetTrigger("FadeOut");
        GameManager.instance.sceneController.ChangeScene(aQueEscena, posicionSiguiente);
    }
}
