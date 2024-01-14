using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    public GameObject selectionFrame;
    public bool isSeleced;

    public NavMeshAgent agent;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    
    public InventoryObjects inventory;
    public DisplayInventory displayInventory;
    

    private void Start()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        
    }
    
    
    void OnEnable()
    {
        InputHandler.OnObjectClick += OnObjectClicked;
    }

    void OnDisable()
    {
        InputHandler.OnObjectClick -= OnObjectClicked;
    }
    

    private void Update()
    {
        PlayAnimation();
    }
    

    private void OnObjectClicked(GameObject clickedObject)
    {
        if (PlayerManager.Instance.currentPlayer == this && isSeleced)
        {
            Debug.Log("Execute on " + clickedObject.name);
            StartCoroutine(MoveThenExecute(clickedObject));
        }
    }


    private IEnumerator MoveThenExecute(GameObject target)
    {
        selectionFrame.SetActive(false);
        agent.SetDestination(target.transform.position);
        
        yield return new WaitUntil(() => !agent.hasPath && !agent.pathPending);

        ExecuteClickedObject(target);
        PlayerManager.Instance.currentPlayer = null;
    }
    
    
    private void ExecuteClickedObject(GameObject target)
    {
        switch (target.tag)
        {
            case "Item":
                
                inventory.AddItem(displayInventory, target.GetComponent<Item>().item, 1);
                
                if (!inventory.isFull)
                {
                    Destroy(target);
                }
                
                if (PlayerManager.Instance.currentPlayer == this)
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
