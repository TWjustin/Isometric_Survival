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


    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = body.GetComponent<Animator>();
        spriteRenderer = body.GetComponent<SpriteRenderer>();
        
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }


    public IEnumerator MoveThenExecute(GameObject target)
    {
        // 開始走
        agent.SetDestination(target.transform.position);
        PlayWalkingAnimation();

        yield return new WaitUntil(() => !agent.hasPath && !agent.pathPending);
        
        // 開始執行
        animator.SetBool("IsWalking", false);
        ExecuteClickedObject(target);
        
        // 結束執行
        InputHandler.Instance.Deselect();
    }
    
    
    private void ExecuteClickedObject(GameObject target)    // 已經移動完
    {
        Debug.Log("Execute On" + target.name);
        
        switch (target.tag)
        {
            case "Item":
                
                Item itemScript = target.GetComponent<Item>();
                
                if (inventory.CheckSlotAvailable(itemScript.item))
                {
                    Destroy(target);
                    inventory.AddItemToInventory(itemScript.item, 1);
                    
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
                    resourceScript.InstantiateCanvas();     // coroutine
                    
                }

                break;
        }
    }
    

    private void PlayWalkingAnimation()
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


    // private void OnApplicationQuit()    // 暫時
    // {
    //     inventory.container.Clear();
    // }
    
}
