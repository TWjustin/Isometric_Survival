using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    
    [SerializeField] private GameObject body;
    [HideInInspector] public NavMeshAgent agent;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    
    public InventoryObjects inventory;
    public InventoryPanel inventoryPanel;
    
    public bool inProgress = false;
    // public bool inProgress = false;


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
        if (agent.hasPath && agent.remainingDistance > 0.1f)
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


    public IEnumerator MoveThenExecute(GameObject target)
    {
        // 開始走
        agent.SetDestination(target.transform.position);
        

        yield return new WaitUntil(() => !agent.hasPath && !agent.pathPending && agent.remainingDistance < 0.1f);
        
        // 開始執行
        InputHandler.Instance.indicatorGem.SetActive(false);
        ExecuteClickedObject(target);
        
    }
    
    
    private void ExecuteClickedObject(GameObject target)    // 已經移動完
    {
        Debug.Log("Execute On" + target.name);
        
        switch (target.tag)
        {
            case "Item":
                
                Drops dropsScript = target.GetComponent<Drops>();
                
                if (inventory.CheckSlotAvailable(dropsScript.item))
                {
                    Destroy(target);
                    inventory.AddItemToInventory(dropsScript.item, 1);
                    
                    if (InputHandler.Instance.currentPlayer == this)
                    {
                        inventoryPanel.UpdateDisplay(this);
                    }
                }

                break;
            
            
            case "Resource":

                TimedEventResource resourceScript = target.GetComponent<TimedEventResource>();
                
                if (inventory.CheckSlotAvailable(resourceScript.dropItemResult))
                {
                    target.GetComponent<TimedEventResource>().actingPlayer = this;
                    inProgress = true;
                    resourceScript.InstantiateCanvas();     // canvas coroutine
                    
                }

                break;
        }
    }

    public void Harvest(TimedEventResource resource)
    {
        inventory.AddItemToInventory(resource.dropItemResult, resource.dropAmount);
        
        if (InputHandler.Instance.currentPlayer == this)
        {
            inventoryPanel.UpdateDisplay(this);
        }
    }
    
    


    // private void OnApplicationQuit()    // 暫時
    // {
    //     inventory.container.Clear();
    // }
    
}
