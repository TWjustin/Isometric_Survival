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
    
    public Inventory inventory;
    public InventoryPanel inventoryPanel;
    
    public bool inProgress = false;
    [HideInInspector] public TimedEventResource actingObject;
    private List<DropItem> actionReward;
    


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
                
                ItemToPickUp itemScript = target.GetComponent<ItemToPickUp>();
                
                DropItem dropItem = new DropItem(itemScript.item, 1);
                actionReward.Clear();
                actionReward.Add(dropItem);
                
                if (inventory.CheckSlotAvailable(actionReward))
                {
                    Destroy(target);
                    inventory.AddItemToInventory(actionReward);
                    
                    if (InputHandler.Instance.currentPlayer == this)
                    {
                        inventoryPanel.UpdateDisplay(this);
                    }
                }

                break;
            
            
            case "Resource":

                actingObject = target.GetComponent<TimedEventResource>();
                TimedEventResource resourceScript = target.GetComponent<TimedEventResource>();
                
                actionReward = resourceScript.CalculateDropItems();
                if (inventory.CheckSlotAvailable(actionReward))
                {
                    inProgress = true;
                    ObjectUIManager.Instance.SpawnCanvas(this);     // canvas的coroutine
                    
                }

                break;
        }
    }

    public void GetReward()
    {
        inProgress = false;
        actingObject = null;
        
        inventory.AddItemToInventory(actionReward);
        ObjectUIManager.Instance.SpawnPopupInARow(this, actionReward);
        
        if (InputHandler.Instance.currentPlayer == this)
        {
            inventoryPanel.UpdateDisplay(this);
        }
        
        InputHandler.Instance.currentPlayer = null;     // 防止他繼續行動
    }
    
    


    // private void OnApplicationQuit()    // 暫時
    // {
    //     inventory.container.Clear();
    // }
    
}
