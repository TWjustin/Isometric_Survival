using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; set; }
    
    
    private GameObject selectedPlayer;
    private PlayerController player;
    
    private Vector3 targetPosition;
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
            

            if (hit.collider != null)
            {
                Debug.Log(hit.collider.gameObject.name);
                

                if (hit.collider.gameObject.CompareTag("Player"))
                {
                    if (selectedPlayer != null)
                    {
                        Deselect();
                    }
                    selectedPlayer = hit.collider.gameObject;
                    player = selectedPlayer.GetComponent<PlayerController>();
                    player.selectionFrame.SetActive(true);
                }
                else if (hit.collider.gameObject.CompareTag("Interactable") && player != null)
                {
                    targetPosition = hit.transform.position;
                    Vector3 offset = (transform.position - targetPosition).normalized * offsetDistance;
                    Vector3 destination = targetPosition + offset;

                    player.agent.SetDestination(destination);
                    Deselect();
                }
                else if (player != null)
                {
                    player.agent.SetDestination(hit.point);
                    Deselect();
                }
            }
            else
            {
                Debug.Log("No hit");
            }
            
        }
    }
    
    private void Deselect()
    {
        selectedPlayer = null;
        player.selectionFrame.SetActive(false);
        player = null;
    }
}
