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

}



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

public class TitleGameState : AbstractGameState
{
    public TitleGameState()
        : base(GameState.Title)
    {

    }

    public override void OnStateEnter()
    {
        Debug.Log("OVERRIDED On Enter: " + GetGameState());
    }

    // public override void OnStateExit()
    // {

    // }

    // public override void Update()
    // {

    // }

}

public class SelectSaveFileState : AbstractGameState
{
    public SelectSaveFileState()
        : base(GameState.SelectSaveFile)
    {

    }


    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            StateManager.PopGameState();
            StateManager.PushGameState(new MainPlayState());
        }
    }


}

public class MainPlayState : AbstractGameState
{
    public MainPlayState()
        : base(GameState.MainPlay)
    {

    }
}


public enum GameState
{
    Title,
    SelectSaveFile,
    MainPlay,
    Inventory,
    Equiping,

}