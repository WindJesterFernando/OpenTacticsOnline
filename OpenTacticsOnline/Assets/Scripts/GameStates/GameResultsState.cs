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
    }

    public override void Update()
    {
        Debug.Log($"Player won: {playerWon}");
    }
}