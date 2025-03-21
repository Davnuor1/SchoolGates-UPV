using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class Challenge03Manager : MonoBehaviour
{
    public List<GameObject> obstaculos; // Lista de objetos
    public GameObject puente;
    public List<GameObject> vallas; // Lista de objetos
    public float fadeDuration = 1f; // Duración del fade
    public Animator changeSceneAnimator; // Asigna el Animator del canvas de fade
    public GameObject bloqueo01;
    public GameObject bloqueo02;
    public GameObject bloqueo03;
    public GameObject portalSalida;
    // Start is called before the first frame update
    private void Awake()
    {
        Lua.RegisterFunction("EndCharla01", this, SymbolExtensions.GetMethodInfo(() => EndCharla01()));
        Lua.RegisterFunction("EndCharla02", this, SymbolExtensions.GetMethodInfo(() => EndCharla02()));
        Lua.RegisterFunction("EndCharla03", this, SymbolExtensions.GetMethodInfo(() => EndCharla03()));
    }
    private void OnDestroy()
    {
        Lua.UnregisterFunction("EndCharla01");
        Lua.UnregisterFunction("EndCharla02");
        Lua.UnregisterFunction("EndCharla03");
    }
    public void EndCharla01()
    {
        StartCoroutine(Charla01());
    }
    public void EndCharla02()
    {
        StartCoroutine(Charla02());
    }
    public void EndCharla03()
    {
        StartCoroutine(Charla03());
    }
    private IEnumerator Charla01()
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

        foreach (GameObject obstaculo in obstaculos)
        {
            obstaculo.SetActive(false);
            //Aqui habria que activar flores
        }
        bloqueo01.SetActive(false);
            //---------------------------
            // Activar el FadeIn
            if (changeSceneAnimator != null)
        {
            changeSceneAnimator.SetTrigger("FadeIn");
        }

        yield return new WaitForSeconds(0.5f);
    }
    private IEnumerator Charla02()
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

        puente.SetActive(true);
        bloqueo02.SetActive(false);

        //---------------------------
        // Activar el FadeIn
        if (changeSceneAnimator != null)
        {
            changeSceneAnimator.SetTrigger("FadeIn");
        }

        yield return new WaitForSeconds(0.5f);
    }
    private IEnumerator Charla03()
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

        foreach (GameObject valla in vallas)
        {
            valla.SetActive(false);
            
        }
        bloqueo03.SetActive(false);
        portalSalida.SetActive(true);

        //---------------------------
        // Activar el FadeIn
        if (changeSceneAnimator != null)
        {
            changeSceneAnimator.SetTrigger("FadeIn");
        }

        yield return new WaitForSeconds(0.5f);
    }
}
