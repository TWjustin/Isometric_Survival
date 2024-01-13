using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private static PlayerManager Instance { get; set; }
    
    public Player player;   // 當前玩家
    
    public GameObject inventoryPanel;

    public GameObject hitObject;
    public float offsetDistance = 1.0f;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Update()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            
            Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            clickPosition.z = 0f;
            RaycastHit2D hit = Physics2D.Raycast(clickPosition, Vector2.zero);
            

            if (hit.collider)
            {
                hitObject = hit.collider.gameObject;
                
                Debug.Log(hitObject.name);
                
                if (hitObject.CompareTag("Player"))
                {
                    Player newPlayer = hitObject.GetComponent<Player>();

                    if (player == newPlayer)
                    {
                        player.selectionFrame.SetActive(false);
                        player = null;
                    }
                    else
                    {
                        SwitchPlayer(newPlayer);
                    }
                    
                }
                else if (player)
                {
                    player.agent.SetDestination(hitObject.transform.position);
                    
                    player.selectionFrame.SetActive(false);
                    StartCoroutine(WaitForMoving());
                }

            }
            else
            {
                if (player)
                {
                    player.agent.SetDestination(clickPosition);
                    player.selectionFrame.SetActive(false);
                    
                    StartCoroutine(WaitForMoving());
                    
                }
                
            }
            
        }
    }
    
    private IEnumerator WaitForMoving()
    {
        yield return new WaitUntil(() => !player.agent.hasPath && !player.agent.pathPending);
        
        if (hitObject.CompareTag("Drops"))
        {
            PickUpItem(hitObject);
        }
        player = null;
    }

    private void PickUpItem(GameObject itemObject)
    {
        player.inventory.AddItem(itemObject.GetComponent<Item>().item, 1);
        if (!player.inventory.isFull)
        {
            Destroy(itemObject);
            inventoryPanel.GetComponent<DisplayInventory>().UpdateDisplay(player.inventory);
        }
    }

    private void SwitchPlayer(Player newPlayer)
    {
        if (player)
        {
            player.selectionFrame.SetActive(false); //上一個player
        }

        player = newPlayer; // 切换当前玩家
        Debug.Log("Switched to " + player.name);

        inventoryPanel.GetComponent<DisplayInventory>().UpdateDisplay(player.inventory);

        player.selectionFrame.SetActive(true);  //新的player
    }
}
