using UnityEngine;

public class GameResultsState : AbstractGameState
{
    private bool playerWon;
    
    public GameResultsState( bool playerWon) : base(GameState.GameResults)
    {
        this.playerWon = playerWon;
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        Debug.Log($"Player won: {playerWon}");
    }

    public override void Update()
    {
        if(Input.GetMouseButtonDown(MouseButton.Left))
        {
            StateManager.PopGameStateUntilStateIs(GameState.Title);
        }
    }
}