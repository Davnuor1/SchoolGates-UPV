using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // Start is called before the first frame update
   
    
    public float speed;
    public Animator animator; 
    private Vector3 direction;
    public Vector2 lastMotionVector;

    // Update is called once per frame
    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        direction = new Vector3(horizontal,vertical);

        AnimateMovement(direction);
        if (horizontal != 0 || vertical != 0)
        {
            lastMotionVector = new Vector2(horizontal, vertical).normalized;
            animator.SetFloat("lastHorizontal", horizontal);
            animator.SetFloat("lastVertical", vertical);
        }
    }
    private void FixedUpdate()
    {
        transform.position += direction * speed * Time.deltaTime; 
    }
    void AnimateMovement(Vector3 direction)
    {
        if (animator != null )
        {
            if (direction.magnitude> 0)
            {
                animator.SetBool("isMoving", true);
                animator.SetFloat("horizontal",direction.x);
                animator.SetFloat("vertical",direction.y);
            }
            else
            {
                
                //Debug.Log("Stop");
                animator.SetBool("isMoving", false);
            }
            
        }
    }
}
