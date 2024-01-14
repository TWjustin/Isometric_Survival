using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; set; }
    
    public Player currentPlayer;   // 當前玩家

    public DisplayInventory displayInventory;
    
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
    
    
    void OnEnable()
    {
        InputHandler.OnPlayerClick += OnPlayerClicked;
    }

    void OnDisable()
    {
        InputHandler.OnPlayerClick -= OnPlayerClicked;
    }

    private void OnPlayerClicked(GameObject clickedObject)
    {
        Player newPlayer = clickedObject.GetComponent<Player>();

        if (currentPlayer == newPlayer)
        {
            currentPlayer.isSeleced = false;
            currentPlayer.selectionFrame.SetActive(false);
        }
        else
        {
            SwitchPlayer(newPlayer);
        }
    }
    

    private void SwitchPlayer(Player newPlayer)
    {
        if (currentPlayer)
        {
            currentPlayer.isSeleced = false; //上一個player
            currentPlayer.selectionFrame.SetActive(false);
        }

        currentPlayer = newPlayer; // 切换当前玩家
        
        Debug.Log("Switched to " + currentPlayer.name);    //新的player
        currentPlayer.isSeleced = true;
        currentPlayer.selectionFrame.SetActive(true);
        
        displayInventory.UpdateDisplay(currentPlayer);
    }
}
