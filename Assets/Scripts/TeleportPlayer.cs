using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPlayer : MonoBehaviour
{
    [SerializeField] public Vector2 posicionSiguiente;
    [SerializeField] private Animator ChangeSceneAnimator;
    [SerializeField] public Transform posicionSiguienteObject;
    [SerializeField] public float offsetY;
    [SerializeField] public float offsetX;


    private void OnTriggerEnter2D(Collider2D collision)
    {

        //GameManager.instance.player.ChangePositionPlayer(posicionSiguiente);
        ChangeSceneAnimator.SetTrigger("FadeOut");
        if (posicionSiguienteObject == null)
        {
            GameManager.instance.sceneController.Teleport(posicionSiguiente);
        }
        else
        {
            Vector3 nuevaPosicion = posicionSiguienteObject.position;
            nuevaPosicion.y += offsetY;
            nuevaPosicion.x += offsetX;
            GameManager.instance.sceneController.Teleport(nuevaPosicion);
        }
        ChangeSceneAnimator.SetTrigger("FadeIn");
    }
}
