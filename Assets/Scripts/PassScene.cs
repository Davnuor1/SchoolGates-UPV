using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassScene : MonoBehaviour
{
    [SerializeField] public Vector2 posicionSiguiente;
    [SerializeField] private Animator ChangeSceneAnimator;
    [SerializeField] public int aQueEscena;
    [SerializeField] public bool portalFinalGate;
    private void OnTriggerEnter2D(Collider2D collision)
    {

        //GameManager.instance.player.ChangePositionPlayer(posicionSiguiente);
        ChangeSceneAnimator.SetTrigger("FadeOut");
        if(portalFinalGate)
        {
            if (UserDataManager.Instance != null)
            {
                UserDataManager.Instance.EndGateSession(true);
            }
        }
        GameManager.instance.sceneController.ChangeScene(aQueEscena, posicionSiguiente);
    }
}
