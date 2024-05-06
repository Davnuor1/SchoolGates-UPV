using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceNodeType
{
    Undefined,
    Tree,
    Ore
}

[CreateAssetMenu(menuName ="Data/Tool action/Gather Resource Node")]
public class GatherResourceNode : ToolAction
{
    [SerializeField] float sizeInteractableArea = 1f;
    [SerializeField] List<ResourceNodeType> canHitNodesOfType;
    [SerializeField] string animationName;
    public override bool OnApply(Vector2 worldPoint,Vector2Int toolDirection, Animator animator)
    {
        
        Collider2D[] colliders = Physics2D.OverlapCircleAll(worldPoint, sizeInteractableArea);
        foreach (Collider2D c in colliders)
        {
            ToolHit hit = c.GetComponent<ToolHit>();
            if (hit != null)
            {
                if (hit.CanBeHit(canHitNodesOfType) == true)
                {
                    
                    animator.SetInteger("toolDirHorizontal", toolDirection.x);
                    animator.SetInteger("toolDirVertical", toolDirection.y);
                    animator.SetTrigger(animationName);
                    hit.Hit();

                    
                    return true;
                }
                
            }
        }
        return false;
    }
}
