using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; set; }
    
    
    private PlayerController player;
    
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
                GameObject hitObject = hit.collider.gameObject;
                
                Debug.Log(hitObject.name);
                
                if (hitObject.CompareTag("Player"))
                {
                    PlayerController newPlayer = hitObject.GetComponent<PlayerController>();

                    if (!player)
                    {
                        player = newPlayer;
                        player.selectionFrame.SetActive(true);
                    }
                    else if (player != newPlayer)
                    {
                        Deselect();
                        player = newPlayer;
                        player.selectionFrame.SetActive(true);
                    }
                    else if (player == newPlayer)
                    {
                        Deselect();
                    }
                }
                else if (player)
                {
                    Vector3 targetPosition = hitObject.transform.position;
                    Vector3 offset = (transform.position - targetPosition).normalized * offsetDistance;
                    Vector3 destination = targetPosition + offset;
                    player.agent.SetDestination(destination);
                    
                    if (hitObject.CompareTag("Drops"))
                    {
                        player.inventory.AddItem(hitObject.GetComponent<Item>().item, 1);
                        Destroy(hitObject);
                    }
                    
                    Deselect();
                }

            }
            else
            {
                player.agent.SetDestination(clickPosition);
                Deselect();
            }
            
        }
    }
    
    private void Deselect()
    {
        player.selectionFrame.SetActive(false);
        player = null;
    }

    
}
