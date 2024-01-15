using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public static InputHandler Instance { get; set; }
    
    [SerializeField] private Camera mainCamera;
    [SerializeField] private DisplayInventory displayInventory;
    
    public GameObject indicatorGem;

    private GameObject currentObject;
    
    [Header("不可編輯")]
    public Player currentPlayer;
    public TimedEventResource currentTimedEventObject;
    
    
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
                    TimedEventManager.Instance.CloseWindow();
                    Deselect();
                }
                else
                {
                    currentObject = hit.collider.gameObject;
                    
                    if (currentObject.CompareTag("Player"))  // 本來選其他object或選到東西
                    {
                        
                        currentPlayer = currentObject.GetComponent<Player>();
                        displayInventory.UpdateDisplay(currentPlayer);
                        
                        
                        SetIndicatorGemAbove(currentObject, 1.5f);
                    }
                    else      // 點到其他東西
                    {
                        currentTimedEventObject = currentObject.GetComponent<TimedEventResource>();
                        TimedEventManager.Instance.currentTimedEvent = currentTimedEventObject;
                        SetIndicatorGemAbove(currentObject, 0.65f);
                        

                        if (currentTimedEventObject.inProgress)
                        {
                            TimedEventManager.Instance.OpenWindowAndSetPosition(currentPlayer);
                        }
                        else if (currentPlayer)
                        {
                            StartCoroutine(currentPlayer.MoveThenExecute(currentObject));
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
                
                TimedEventManager.Instance.CloseWindow();
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
