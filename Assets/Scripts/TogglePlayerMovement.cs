using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TogglePlayerMovement : MonoBehaviour
{
    private GameObject player;
    private PlayerMovement playerMovement;
    private Animator playerAnimator;
    public bool desactivarNPC;
    public GameObject NpcADesactivar;
    public bool mantenerMovimientoDesactivado;
    void Start()
    {
        // Intentar encontrar al jugador al inicio
        FindPlayer();
    }
    void Update()
    {
        // Si no se ha encontrado el jugador, intentar encontrarlo
        if (player == null)
        {
            FindPlayer();
            return;  // No seguir ejecutando si aún no hay jugador
        }
    }
        void FindPlayer()
    {
        // Buscar al jugador por su tag
        player = GameObject.FindGameObjectWithTag("Player");
        playerAnimator = player.GetComponent<Animator>();

        if (player == null)
        {
            Debug.LogWarning("Jugador no encontrado. Asegúrate de que el objeto jugador tiene el tag 'Player'.");
        }
    }
    private void OnEnable()
    {
        // Suscribirse a los eventos del Dialogue System
        if (DialogueManager.instance != null)
        {
            DialogueManager.instance.conversationStarted += OnConversationStart;
            DialogueManager.instance.conversationEnded += OnConversationEnd;
        }
    }

    private void OnDisable()
    {
        // Desuscribirse de los eventos para evitar problemas de referencia
        if (DialogueManager.instance != null)
        {
            DialogueManager.instance.conversationStarted -= OnConversationStart;
            DialogueManager.instance.conversationEnded -= OnConversationEnd;
        }
    }
    private void OnConversationStart(Transform actor)
    {
        if (player != null)
        {
            DialogueManager.instance.GetComponentInChildren<StandardUIQuestTracker>().HideTracker();
            GameManager.instance.uiManager.canToggle = false;
            playerAnimator.SetBool("moving", false);
            playerMovement = player.GetComponent<PlayerMovement>();
            playerMovement.enabled = false; // Desactiva el movimiento del jugador

        }
    }
    private void OnConversationEnd(Transform actor)
    {
        if (player != null)
        {
            
            DialogueManager.instance.GetComponentInChildren<StandardUIQuestTracker>().ShowTracker();
            GameManager.instance.uiManager.canToggle = true;
            if (mantenerMovimientoDesactivado)
            {
                
            } else
            {
                playerMovement = player.GetComponent<PlayerMovement>();
                playerMovement.enabled = true; // Desactiva el movimiento del jugador
            }
            
            if (desactivarNPC)
            {
                //this.GetComponentInParent<SpriteRenderer>;
                if (Vector2.Distance(transform.position, player.transform.position) <= 2f)
                {
                    NpcADesactivar.SetActive(false);
                    Debug.Log(NpcADesactivar.name + "Desactivado");
                }
                
            }

        }
    }
}
