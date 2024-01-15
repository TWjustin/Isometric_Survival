using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public static InputHandler Instance { get; set; }
    
    public GameObject indicatorGem;
    
    [HideInInspector]
    public Player currentPlayer;
    
    // [SerializeField]
    // private DisplayInventory playerInventory;
    
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

    private void Start()
    {
        indicatorGem.SetActive(false);
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
                GameObject clickedObject = hit.collider.gameObject;

                if (clickedObject.CompareTag("Player"))
                {
                    SelectPlayer(clickedObject);
                }
                else if (currentPlayer)
                {
                    indicatorGem.SetActive(true);
                    
                    Vector3 gemPosition = new Vector3(clickedObject.transform.position.x, clickedObject.transform.position.y + 0.65f, 0f);
                    indicatorGem.transform.position = gemPosition;

                    if (currentPlayer.isSelected)   // 雙重認證
                    {
                        StartCoroutine(currentPlayer.MoveThenExecute(clickedObject));
                    }
                }

            }
            
        }
    }
    
    private void SelectPlayer(GameObject clickedObject)
    {
        Player newPlayer = clickedObject.GetComponent<Player>();

        if (currentPlayer == newPlayer)     // 取消選取
        {
            currentPlayer.isSelected = false;
            indicatorGem.SetActive(false);
            currentPlayer = null;
        }
        else    // 選取新的player
        {
            if (currentPlayer)  // 舊的player
            {
                currentPlayer.isSelected = false;
            }

            currentPlayer = newPlayer; // 切换当前玩家
        
            Debug.Log("Switched to " + currentPlayer.name);
            currentPlayer.isSelected = true;
        
            Vector3 gemPosition = new Vector3(currentPlayer.transform.position.x, currentPlayer.transform.position.y + 1.5f, 0f);
            indicatorGem.SetActive(true);
            indicatorGem.transform.position = gemPosition;
        
            currentPlayer.displayInventory.UpdateDisplay(currentPlayer);
        }
    }
    
}
