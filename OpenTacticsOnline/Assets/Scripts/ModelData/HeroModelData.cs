using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HeroJobClasses
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
    public HeroJobClasses jobClass;
    public int maxSteps;
    public GameObject visualRepresentation;
    
    public int currentHealth, maxHealth;
    public bool isAlly;

    public Hero(int x, int y, HeroJobClasses jobClass, int maxSteps, int maxHealth, bool isAlly)
    {
        coord = new Vector2Int(x, y);
        this.jobClass = jobClass;
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


