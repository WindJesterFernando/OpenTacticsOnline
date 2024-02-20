using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HeroRole
{
    BlackMage,
    RedMage,
    WhiteMage,
    Fighter,
    Monk,
    Thief
}

public class Hero
{
    public Vector2Int coord;
    public HeroRole role;
    public int maxSteps;
    public GameObject visualRepresentation;
    
    public int currentHealth, maxHealth;
    public bool isAlly;

    public Hero(int x, int y, HeroRole heroRole, int maxSteps, int maxHealth, bool isAlly)
    {
        coord = new Vector2Int(x, y);
        this.role = heroRole;
        this.maxSteps = maxSteps;
        this.maxHealth = maxHealth;
        currentHealth = maxHealth;
        this.isAlly = isAlly;
    }

    public bool IsAlive()
    {
        return currentHealth > 0;
    }
}


