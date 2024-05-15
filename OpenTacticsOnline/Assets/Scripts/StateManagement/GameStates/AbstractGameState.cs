public abstract class AbstractGameState
{
    GameState gameState;

    public virtual void OnStateEnter()
    {

    }
    
    public virtual void OnStateExit()
    {

    }
    
    public virtual void OnStateContinue()
    {

    }
    
    public virtual void OnStatePause()
    {

    }
    
    public virtual void Update()
    {

    }
    
    public GameState GetGameState()
    {
        return gameState;
    }

    protected AbstractGameState(GameState gameState)
    {
        this.gameState = gameState;
    }
}