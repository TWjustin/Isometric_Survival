using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; set; }
    
    
    public PlayerController player;

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
                    PlayerController newPlayer = hitObject.GetComponent<PlayerController>();

                    if (!player)
                    {
                        player = newPlayer;
                        player.selectionFrame.SetActive(true);
                    }
                    else if (player != newPlayer)
                    {
                        player.selectionFrame.SetActive(false); //上一個player
                        
                        player = newPlayer;
                        player.selectionFrame.SetActive(true);  //新的player
                    }
                    else if (player == newPlayer)
                    {
                        player.selectionFrame.SetActive(false);
                        player = null;
                    }
                }
                else if (hitObject.CompareTag("Drops"))
                {
                    
                    if (player)
                    {
                        MoveBesideTarget(hitObject.transform.position);
                        
                        player.selectionFrame.SetActive(false);
                        StartCoroutine(WaitForMoving());
                        
                        
                    }
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
    
    private void MoveBesideTarget(Vector3 targetPosition)
    {
        Vector3 offset = (transform.position - targetPosition).normalized * offsetDistance;
        Vector3 destination = targetPosition + offset;
        player.agent.SetDestination(destination);
    }
    
    private void PickUpItem(GameObject g)
    {
        player.inventory.AddItem(g.GetComponent<Item>().item, 1);
        Destroy(g);
        
    }


}
