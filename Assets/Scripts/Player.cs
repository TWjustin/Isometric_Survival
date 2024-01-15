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
    public DisplayInventory displayInventory;


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
        InputHandler.Instance.Deselect();   // 不要TimedEventManager.Instance.CloseWindow();
    }
    
    
    private void ExecuteClickedObject(GameObject target)    // 已經移動完
    {
        Debug.Log("Execute On" + target.name);
        
        switch (target.tag)
        {
            case "Item":
                
                Item itemScript = target.GetComponent<Item>();
                
                if (inventory.CheckSlotAvailable(displayInventory, itemScript.item))
                {
                    inventory.AddItemToInventory(displayInventory, itemScript.item, 1);
                    Destroy(target);
                }

                if (InputHandler.Instance.currentPlayer == this)    // 不要用else if，跟上面是分開的
                {
                    displayInventory.UpdateDisplay(this);
                }
                break;
            
            
            case "Resource":

                TimedEventResource resourceScript = target.GetComponent<TimedEventResource>();
                
                if (inventory.CheckSlotAvailable(displayInventory, resourceScript.dropItemResult))
                {
                    
                    TimedEventManager.Instance.OpenWindowAndSetPosition(this);
                    resourceScript.StartTimer(); // coroutine
                    
                    inventory.AddItemToInventory(displayInventory, resourceScript.dropItemResult, resourceScript.dropAmount);
                    displayInventory.UpdateDisplay(this);
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
