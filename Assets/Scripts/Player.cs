using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    [HideInInspector]
    public bool isSelected;     // 雙重認證

    [SerializeField]
    private GameObject body;
    private NavMeshAgent agent;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    
    public InventoryObjects inventory;
    public DisplayInventory displayInventory;
    

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = body.GetComponent<Animator>();
        spriteRenderer = body.GetComponent<SpriteRenderer>();
        
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }
    

    private void Update()
    {
        PlayAnimation();
    }


    public IEnumerator MoveThenExecute(GameObject target)
    {
        
        agent.SetDestination(target.transform.position);
        
        yield return new WaitUntil(() => !agent.hasPath && !agent.pathPending);

        isSelected = false;     // 雙重認證
        ExecuteClickedObject(target);
    }
    
    
    private void ExecuteClickedObject(GameObject target)
    {
        switch (target.tag)
        {
            case "Item":
                
                if(inventory.CheckThenAddItem(displayInventory, target.GetComponent<Item>().item, 1))
                {
                    Destroy(target);
                    InputHandler.Instance.indicatorGem.SetActive(false);
                }

                if (InputHandler.Instance.currentPlayer == this)
                {
                    displayInventory.UpdateDisplay(this);
                }
                break;
            
        }
    }


    private void PlayAnimation()
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


    // private void OnApplicationQuit()    // 暫時
    // {
    //     inventory.container.Clear();
    // }
    
}
