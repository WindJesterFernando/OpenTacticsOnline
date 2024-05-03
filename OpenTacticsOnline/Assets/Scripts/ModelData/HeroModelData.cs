using System;
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
/// <summary>
/// something here <br/>
/// <para>more comments</para>
/// </summary>
public class Hero
{
    public GridCoord coord;
    public HeroRole role;
    public int maxSteps;
    private UnityEngine.GameObject visualRepresentation;

    public AbstractController controller;
    
    public int currentHealth { get; private set; }
    
    public int maxHealth;

    public bool isAlly;
    public List<TurnAction> actions;

    public Hero(int x, int y, HeroRole heroRole, int maxSteps, int maxHealth, bool isAlly)
    {
        coord = new GridCoord(x, y);
        this.role = heroRole;
        this.maxSteps = maxSteps;
        this.maxHealth = maxHealth;
        currentHealth = maxHealth;
        this.isAlly = isAlly;
        CreateTurnActions();
        //UnityEngine.Debug.Log(this.maxHealth);
    }

    public bool IsAlive()
    {
        return currentHealth > 0;
    }

    public void SetVisualRepresentation(GameObject visuals)
    {
        visualRepresentation = visuals;
    }

    public GameObject GetVisualRepresentation()
    {
        return visualRepresentation;
    }

    private void CreateTurnActions()
    {
        actions = new List<TurnAction>();
        actions.Add(new MoveTurnAction(this));
        actions.Add(new AttackTurnAction(this));
        switch (role)
        {
            case HeroRole.BlackMage:
                actions.Add(new AttackTurnAction(this, 10, "Magic Missile"));
                break;
            case HeroRole.RedMage:
                actions.Add(new AttackTurnAction(this, 10, "Magic Missile"));
                break;
            case HeroRole.WhiteMage:
                actions.Add(new HealTurnAction(this, 5, "Healing"));
                actions.Add(new ReviveTurnAction(this));
                break;
            case HeroRole.Fighter:
                break;
            case HeroRole.Monk:
                break;
            case HeroRole.Thief:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        actions.Add(new PassTurnAction(this));
    }

    public void ModifyHealth(int deltaHealth)
    {
        currentHealth += deltaHealth;
        if (currentHealth < 0)
            currentHealth = 0;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
        
    }

    public void DoTurn()
    {
        controller.DoTurn(this);
    }
}


