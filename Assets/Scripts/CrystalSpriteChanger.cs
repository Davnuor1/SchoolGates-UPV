using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalSpriteChanger : MonoBehaviour
{
    public List<GameObject> crystalsAzules; // Lista de objetos de cristales
    public Sprite emergedSpriteAzul; // Sprite totalmente emergido
    public Sprite halfEmergedSpriteAzul; // Sprite medio emergido
    public Sprite submergedSpriteAzul; // Sprite totalmente sumergido

    public List<GameObject> crystalsRojos; // Lista de objetos de cristales
    public Sprite emergedSpriteRojo; // Sprite totalmente emergido
    public Sprite halfEmergedSpriteRojo; // Sprite medio emergido
    public Sprite submergedSpriteRojo; // Sprite totalmente sumergido

    public GameObject redCrystals;
    public GameObject blueCrystals;
    


    public void AnimarAzulesSumerger()
    {
        StartCoroutine(SumergerAzules());
    }
    public void AnimarAzulesEmerger()
    {
        StartCoroutine(EmergerAzules());
    }
    public void AnimarRojosSumerger()
    {
        StartCoroutine(SumergerRojos());
    }
    public void AnimarRojosEmerger()
    {
        StartCoroutine(EmergerRojos());
    }


    IEnumerator SumergerAzules()
    {
        // Cambiar a sprite medio emergido después de 0.2 segundos
        yield return new WaitForSeconds(0.25f);
        ChangeSpritesAzules(halfEmergedSpriteAzul);

        // Cambiar a sprite totalmente sumergido después de otros 0.2 segundos
        yield return new WaitForSeconds(0.25f);
        ChangeSpritesAzules(submergedSpriteAzul);
        ChangeLayerAzules(11);
        ChangeColliderAzules(false);

        //blueCrystals.SetActive(false);

    }

    IEnumerator EmergerAzules()
    {
        //blueCrystals.SetActive(true);
        ChangeColliderAzules(true);
        ChangeLayerAzules(12);
        // Cambiar a sprite medio emergido después de 0.2 segundos
        yield return new WaitForSeconds(0.25f);
        ChangeSpritesAzules(halfEmergedSpriteAzul);

        // Cambiar a sprite totalmente sumergido después de otros 0.2 segundos
        yield return new WaitForSeconds(0.25f);
        ChangeSpritesAzules(emergedSpriteAzul);
        
    }
    IEnumerator SumergerRojos()
    {
        // Cambiar a sprite medio emergido después de 0.2 segundos
        yield return new WaitForSeconds(0.25f);
        ChangeSpritesRojos(halfEmergedSpriteRojo);

        // Cambiar a sprite totalmente sumergido después de otros 0.2 segundos
        yield return new WaitForSeconds(0.25f);
        ChangeSpritesRojos(submergedSpriteRojo);
        ChangeColliderRojos(false);
        ChangeLayerRojos(11);
        //redCrystals.SetActive(false);
    }

    IEnumerator EmergerRojos()
    {
        //redCrystals.SetActive(true);
        ChangeColliderRojos(true);
        ChangeLayerRojos(12);
        // Cambiar a sprite medio emergido después de 0.2 segundos
        yield return new WaitForSeconds(0.25f);
        ChangeSpritesRojos(halfEmergedSpriteRojo);

        // Cambiar a sprite totalmente sumergido después de otros 0.2 segundos
        yield return new WaitForSeconds(0.25f);
        ChangeSpritesRojos(emergedSpriteRojo);
        
    }

    private void ChangeSpritesAzules(Sprite newSprite)
    {
        foreach (GameObject crystal in crystalsAzules)
        {
            SpriteRenderer renderer = crystal.GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                renderer.sprite = newSprite;
            }
        }
    }
    private void ChangeColliderAzules(bool Estado)
    {
        foreach (GameObject crystal in crystalsAzules)
        {

            BoxCollider2D collider2D = crystal.GetComponent<BoxCollider2D>();
            if (collider2D != null)
            {
                collider2D.enabled = Estado;
            }
        }
    }
    private void ChangeLayerAzules(int numLayer)
    {
        foreach (GameObject crystal in crystalsAzules)
        {
            SpriteRenderer renderer = crystal.GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                renderer.sortingOrder = numLayer;
            }
        }
    }

    private void ChangeSpritesRojos(Sprite newSprite)
    {
        foreach (GameObject crystal in crystalsRojos)
        {
            SpriteRenderer renderer = crystal.GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                renderer.sprite = newSprite;
            }
        }
    }

    private void ChangeColliderRojos(bool Estado)
    {
        foreach (GameObject crystal in crystalsRojos)
        {
            
            BoxCollider2D collider2D = crystal.GetComponent<BoxCollider2D>();
            if (collider2D != null)
            {
                collider2D.enabled = Estado;
            }
        }
    }
    private void ChangeLayerRojos(int numLayer)
    {
        foreach (GameObject crystal in crystalsRojos)
        {
            SpriteRenderer renderer = crystal.GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                renderer.sortingOrder = numLayer;
            }
        }
    }


}
