using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public static event Action<GameObject> OnPlayerClick;
    public static event Action<GameObject> OnObjectClick;
    
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

                if (hitObject.CompareTag("Player"))
                {
                    OnPlayerClick?.Invoke(hitObject);   // 触发事件，通知其他脚本鼠标点击
                }
                else
                {
                    OnObjectClick?.Invoke(hitObject);   // 触发事件，通知其他脚本鼠标点击
                }

            }
            
        }
    }
}
