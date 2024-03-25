public abstract class AbstractGameState
{
    GameState gameState;

    public virtual void OnStateEnter()
    {
        //UnityEngine.Debug.Log("On Enter: " + GetGameState());
    }
    
    public virtual void OnStateExit()
    {
        //UnityEngine.Debug.Log("On Exit: " + GetGameState());
    }
    
    public virtual void OnStateContinue()
    {
        //UnityEngine.Debug.Log("On Continue: " + GetGameState());
    }
    
    public virtual void OnStatePause()
    {
        //UnityEngine.Debug.Log("On Pause: " + GetGameState());
    }
    
    public virtual void Update()
    {
        //UnityEngine.Debug.Log("On Update: " + GetGameState());
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
