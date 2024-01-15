using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractGameState
{
    GameState gameState;

    public virtual void OnStateEnter()
    {
        Debug.Log("On Enter: " + GetGameState());
    }
    public virtual void OnStateExit()
    {
        Debug.Log("On Exit: " + GetGameState());
    }
    public virtual void Update()
    {
        Debug.Log("On Update: " + GetGameState());
    }
    public GameState GetGameState()
    {
        return gameState;
    }

    public AbstractGameState(GameState gameState)
    {
        this.gameState = gameState;
    }
}
