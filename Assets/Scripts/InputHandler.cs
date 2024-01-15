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
    [HideInInspector] public Player currentPlayer;
    
    
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
                    
                    if (currentObject.CompareTag("Player"))
                    {
                        
                        SetIndicatorGemAbove(currentObject, 1.5f);
                        currentPlayer = currentObject.GetComponent<Player>();
                        displayInventory.UpdateDisplay(currentPlayer);
                    }
                    else      // 點到其他東西
                    {
                        SetIndicatorGemAbove(currentObject, 0.65f);

                        if (currentPlayer)
                        {
                            StartCoroutine(currentPlayer.MoveThenExecute(currentObject));
                        }
                        
                    }
                }

            }
            else if (currentObject)
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
        currentObject = null;
        currentPlayer = null;
        indicatorGem.SetActive(false);
    }
    
    
}
