using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public GameObject selectionFrame;

    public NavMeshAgent agent;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    
    public InventoryObjects inventory;

    private void Start()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    private void Update()
    {
        if (agent.hasPath)
        {
            animator.SetBool("IsWalking", true);
            
            Vector3 moveDirection = agent.velocity.normalized;
            animator.SetFloat("yVelocity", moveDirection.y);
            if (moveDirection.x > 0)
            {
                spriteRenderer.flipX = false;
            }
            else if (moveDirection.x < 0)
            {
                spriteRenderer.flipX = true;
            }
        }
        else
        {
            animator.SetBool("IsWalking", false);
        }
    }
    
    
    private void OnApplicationQuit()    // 暫時
    {
        inventory.container.Clear();
    }
    
}
