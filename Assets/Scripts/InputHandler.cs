using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputHandler : MonoBehaviour
{
    public static InputHandler Instance { get; set; }
    
    [SerializeField] private Camera mainCamera;
    [SerializeField] private InventoryPanel inventoryPanelScript;
    
    public GameObject indicatorGem;
    [HideInInspector] public GameObject playerInfoPanel;

    
    [Header("Do not Edit")]
    [SerializeField] private GameObject currentObject;  // 不用去掉
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
        playerInfoPanel.SetActive(false);
    }


    void Update()
    {
        // 检测是否有点击事件发生在 UI 上
        if (EventSystem.current.IsPointerOverGameObject())
        {
            // UI 上的点击，不执行后面的逻辑
            return;
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 clickPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            clickPosition.z = 0f;
            RaycastHit2D hit = Physics2D.Raycast(clickPosition, Vector2.zero);

            if (hit.collider)
            {
                GameObject clickedObject = hit.collider.gameObject;
                
                

                if (clickedObject == currentObject)
                {
                    // Deselect
                    indicatorGem.SetActive(false);
                    playerInfoPanel.SetActive(false);
                    currentObject = null;
                    currentPlayer = null;   // 目前OK
                }
                else
                {
                    currentObject = clickedObject;
                    
                    if (clickedObject.CompareTag("Player"))  // 本來選其他object或選到東西
                    {

                        currentPlayer =  clickedObject.GetComponent<Player>();
                        playerInfoPanel.SetActive(true);
                        inventoryPanelScript.UpdateDisplay(currentPlayer);
                        
                        
                        
                        SetIndicatorGemAbove(clickedObject, 1.5f);
                        
                    }
                    else      // 點到其他東西
                    {
                        
                        SetIndicatorGemAbove(clickedObject, 0.65f);
                        

                        if (currentPlayer)
                        {
                            if (!currentPlayer.inProgress)
                            {
                                StartCoroutine(currentPlayer.MoveThenExecute(clickedObject));   // coroutine
                            }
                            else
                            {
                                Debug.Log("This player is busy");
                            }
                            
                            
                        }
                    }
                }

            }
            else     // 點到空白處則取消選取
            {
                if (currentPlayer)
                {
                    if (!currentPlayer.inProgress)
                    {
                        currentPlayer.agent.SetDestination(clickPosition);
                    }
                    
                }
                
                // Deselect
                indicatorGem.SetActive(false);
                playerInfoPanel.SetActive(false);
                currentObject = null;
                currentPlayer = null;
                
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


}
