using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StateManager
{
    static Stack<AbstractGameState> gameStateStack;

    public static void Init()
    {
        gameStateStack = new Stack<AbstractGameState>();
    }

    public static void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            PopGameState();
            //Debug.Log("fasdfsdfgdsgd");
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            PushGameState(new SelectSaveFileState());
            //Debug.Log("fasdfsdfgdsgd");
        }


        if (gameStateStack.Count > 0)
            gameStateStack.Peek().Update();
    }

    public static void PushGameState(AbstractGameState gameState)
    {
        gameStateStack.Push(gameState);
        gameState.OnStateEnter();
    }

    public static void PopGameState()
    {
        gameStateStack.Peek().OnStateExit();
        gameStateStack.Pop();
    }

    public static void PopGameStateUntilStateIs(GameState gameState)
    {
        while(gameStateStack.Peek().GetGameState() != gameState)
            PopGameState();
    }
    
}

public enum GameState
{
    Title,
    SelectSaveFile,
    MainPlay,
    MoveSelection,
    PerformingMove,
    Inventory,
    Equiping,

}