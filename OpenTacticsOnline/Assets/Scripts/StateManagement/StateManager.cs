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
    TargetSelection,
    PerformingAction,
    GameResults,
    SelectActionUI,
    GameRoom,
    LocalRoom,
}