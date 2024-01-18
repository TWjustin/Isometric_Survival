using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    
    [HideInInspector] public NavMeshAgent agent;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    
    public Inventory inventory;
    public InventoryPanel inventoryPanel;
    
    // public SpriteRenderer heldItemSprite;
    public Image heldItemImage;
    [Header("Do not Edit")]
    public ItemObject heldItem;
    
    [HideInInspector] public bool inProgress = false;
    [HideInInspector] public GameObject actingObject;
    private List<DropItem> actionReward;
    


    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }
    
    private void Update()   // 動畫
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
            
            
            case "TimedEventResource":

                actingObject = target;
                TimedEventResource timedEventResource = target.GetComponent<TimedEventResource>();

                actionReward = DropItemData.CalculateDropItems(timedEventResource);
                if (inventory.CheckSlotAvailable(actionReward))
                {
                    inProgress = true;
                    ObjectUIManager.Instance.SpawnCanvas(this, actingObject.GetComponent<TimedEventResource>());     // canvas的coroutine
                    
                }

                break;
            
            case "DamageBasedResource":
                
                DamageBasedResource damageBasedResource = target.GetComponent<DamageBasedResource>();
                ToolObject tool = heldItem as ToolObject;

                if (tool && tool.toolType == damageBasedResource.toolNeeded)
                {
                    actingObject = target;
                    actionReward = DropItemData.CalculateDropItems(damageBasedResource);
                
                
                    if (inventory.CheckSlotAvailable(actionReward))
                    {
                        inProgress = true;
                        ObjectUIManager.Instance.SpawnHealthBar(actingObject);

                        animator.SetBool("IsUsingTool", true);
                        AnimationClip clip = animator.GetCurrentAnimatorClipInfo(0)[0].clip;
                        float clipLength = clip.length;

                        int damage = tool.strength;
                    
                        while (damageBasedResource.currentHealth > 0)
                        {
                            StartCoroutine(TakeDamage(damageBasedResource, clipLength, damage));
                        }
                    
                        Destroy(damageBasedResource.gameObject);
                        animator.SetBool("IsUsingTool", false);
                    }
                }
                
                break;
            
        }
    }
    
    
    private IEnumerator TakeDamage(DamageBasedResource resource, float clipLength, int damage)
    {
        yield return new WaitForSeconds(clipLength);
        
        resource.currentHealth -= damage;
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
