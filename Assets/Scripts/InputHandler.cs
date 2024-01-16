using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public static InputHandler Instance { get; set; }
    
    [SerializeField] private Camera mainCamera;
    [SerializeField] private InventoryPanel inventoryPanel;
    
    public GameObject indicatorGem;

    
    [Header("Do not Edit")]
    [SerializeField] private GameObject currentObject;
    public Player currentPlayer;


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
            Vector3 clickPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            clickPosition.z = 0f;
            RaycastHit2D hit = Physics2D.Raycast(clickPosition, Vector2.zero);

            if (hit.collider)
            {

                if (hit.collider.gameObject == currentObject)
                {
                    
                    Deselect();
                }
                else
                {
                    currentObject = hit.collider.gameObject;
                    
                    if (currentObject.CompareTag("Player"))  // 本來選其他object或選到東西
                    {
                        
                        currentPlayer = currentObject.GetComponent<Player>();
                        inventoryPanel.UpdateDisplay(currentPlayer);
                        
                        
                        SetIndicatorGemAbove(currentObject, 1.5f);
                        
                    }
                    else      // 點到其他東西
                    {
                        
                        SetIndicatorGemAbove(currentObject, 0.65f);
                        

                        if (currentPlayer)
                        {
                            StartCoroutine(currentPlayer.MoveThenExecute(currentObject));   // coroutine
                        }
                    }
                }

            }
            else if (currentObject)     // 點到空白處則取消選取
            {
                if (currentPlayer)
                {
                    currentPlayer.agent.SetDestination(clickPosition);
                }
                

                Deselect();
                
            }
            
        }
    }
    
    private void SetIndicatorGemAbove(GameObject target, float yOffset)
    {
        indicatorGem.SetActive(true);
        Vector3 indicatorPosition = target.transform.position;
        indicatorPosition.y += yOffset;
        indicatorGem.transform.position = indicatorPosition;
    }

    public void Deselect()
    {
        indicatorGem.SetActive(false);
        currentObject = null;
        currentPlayer = null;
    }
    
    
}
