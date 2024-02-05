using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero
{
    public Vector2Int coord;
    public int id;
    public int maxSteps;
    public GameObject visualRepresentation;
    
    public int currentHealth, maxHealth;
    public bool isAlly;

    public Hero(int x, int y, int id, int maxSteps, int maxHealth, bool isAlly)
    {
        coord = new Vector2Int(x, y);
        this.id = id;
        this.maxSteps = maxSteps;
        this.maxHealth = maxHealth;
        currentHealth = maxHealth;
        this.isAlly = isAlly;
    }
}


