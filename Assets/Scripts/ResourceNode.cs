using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class ResourceNode : ToolHit
{
    [SerializeField] Item pickUpDrop;
    [SerializeField] int dropCount = 5;
    [SerializeField] ResourceNodeType nodeType;
    public override void Hit()
    {
        Vector2 position = this.transform.position;
        GameManager.instance.player.DropItemPosition(pickUpDrop, dropCount,position);
        Destroy(gameObject);
    }
    public override bool CanBeHit(List<ResourceNodeType> canBeHit)
    {
        return canBeHit.Contains(nodeType);
    }
}
