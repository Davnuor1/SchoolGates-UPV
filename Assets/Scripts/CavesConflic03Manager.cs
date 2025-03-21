using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class CavesConflic03Manager : MonoBehaviour
{
    public GameObject fuego;
    public float fadeDuration = 1f; // Duración del fade
    public Animator changeSceneAnimator; // Asigna el Animator del canvas de fade
    public GameObject portalSalida;

    private void Awake()
    {
        Lua.RegisterFunction("activarFuego", this, SymbolExtensions.GetMethodInfo(() => activarFuego()));
    }

    private void OnDestroy()
    {
        Lua.UnregisterFunction("activarFuego");
       
    }

    public void activarFuego()
    {
        StartCoroutine(Fuego());
    }
    private IEnumerator Fuego()
    {
        //Debug.Log(");

        // Activar el FadeOut
        if (changeSceneAnimator != null)
        {
            changeSceneAnimator.SetTrigger("FadeOut");
        }

        // Esperar a que se complete el fade
        yield return new WaitForSeconds(fadeDuration);
        //HACER COSAS----------------

        fuego.SetActive(true);
        portalSalida.SetActive(true);
        //Aqui activar portal o mural rupestre

        //---------------------------
        // Activar el FadeIn
        if (changeSceneAnimator != null)
        {
            changeSceneAnimator.SetTrigger("FadeIn");
        }

        yield return new WaitForSeconds(0.5f);
    }
}
