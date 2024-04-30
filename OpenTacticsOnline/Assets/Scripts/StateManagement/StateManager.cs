using System.Collections.Generic;

public static class StateManager
{
    static Stack<AbstractGameState> gameStateStack;

    public static void Init()
    {
        gameStateStack = new Stack<AbstractGameState>();
    }

    public static void Update()
    {
        if (gameStateStack.Count > 0)
            gameStateStack.Peek().Update();
    }

    public static void PushGameState(AbstractGameState gameState)
    {
        if(gameStateStack.Count > 0)
            gameStateStack.Peek().OnStatePause();
        
        gameStateStack.Push(gameState);
        gameState.OnStateEnter();
    }
    
    public static void PopGameState()
    {
        //Debug.Log("popping " + gameStateStack.Peek().GetGameState());
        
        gameStateStack.Peek().OnStateExit();
        gameStateStack.Pop();
        
        if(gameStateStack.Count > 0)
            gameStateStack.Peek().OnStateContinue();
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
    AttackSelection,
    PerformingMove,
    PerformingAction,
    GameResults,
    SelectActionUI,
    GameRoom,
    LocalRoom,
    // Inventory,
    // Equiping,
}