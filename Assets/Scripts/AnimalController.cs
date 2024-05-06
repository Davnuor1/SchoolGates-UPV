using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalController : MonoBehaviour
{
    public float moveSpeed = 1f; // Velocidad de movimiento
    public GameObject heartSprite;
    public LayerMask obstacleLayer; // Capa que contiene los obstáculos
    private Animator anim;
    private bool canInteract = true;

    private Vector3 targetPosition; // Posición objetivo hacia donde se moverá el animal
    private bool isMoving = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
        StartCoroutine(RandomMovement());
    }

    private void Update()
    {
        if (canInteract && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }

        if (isMoving)
        {
            MoveTowardsTarget();
        }
    }

    private IEnumerator RandomMovement()
    {
        while (true)
        {
            int randomDirection = Random.Range(1, 5);

            switch (randomDirection)
            {
                case 1: // Arriba
                    if (!CheckCollision(Vector3.up))
                    {
                        anim.SetTrigger("MoveUp");
                        SetTargetPosition(Vector3.up);
                    }
                    break;
                case 2: // Abajo
                    if (!CheckCollision(Vector3.down))
                    {
                        anim.SetTrigger("MoveDown");
                        SetTargetPosition(Vector3.down);
                    }
                    break;
                case 3: // Derecha
                    if (!CheckCollision(Vector3.right))
                    {
                        anim.SetTrigger("MoveRight");
                        SetTargetPosition(Vector3.right);
                    }
                    break;
                case 4: // Izquierda
                    if (!CheckCollision(Vector3.left))
                    {
                        anim.SetTrigger("MoveLeft");
                        SetTargetPosition(Vector3.left);
                    }
                    break;
            }

            yield return new WaitForSeconds(3f); // Cambia de dirección cada 3 segundos
        }
    }

    private bool CheckCollision(Vector3 direction)
    {
        Collider2D hitCollider = Physics2D.OverlapBox(transform.position + direction, transform.localScale, 0, obstacleLayer);
        return hitCollider != null;
    }

    private void SetTargetPosition(Vector3 direction)
    {
        targetPosition = transform.position + direction;
        isMoving = true;
    }

    private void MoveTowardsTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (transform.position == targetPosition)
        {
            isMoving = false;
            anim.SetTrigger("Idle"); // Cambiar a la animación "idle"
        }
    }

    private void Interact()
    {
        canInteract = false;
        heartSprite.SetActive(true);
        StartCoroutine(ResetInteraction());
    }

    private IEnumerator ResetInteraction()
    {
        yield return new WaitForSeconds(2f); // Tiempo que el corazón estará visible
        heartSprite.SetActive(false);
        canInteract = true;
    }
}
