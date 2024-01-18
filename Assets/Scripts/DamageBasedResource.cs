using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBasedResource : Resource
{
    public ToolType toolNeeded;
    
    public int maxHealth;
    [HideInInspector] public int currentHealth;


    private void Start()
    {
        currentHealth = maxHealth;
    }

    
    
}
