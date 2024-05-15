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
        string resultsText;
        if (playerWon)
        {
            resultsText = "You Won!";
        }
        else
        {
            resultsText = "You Lost!";
        }
        UIManager.EnablePopupText(resultsText);
    }

    public override void Update()
    {
        if(Input.GetMouseButtonDown(MouseButton.Left))
        {
            StateManager.PopGameStateUntilStateIs(GameState.Title);
        }
    }

    public override void OnStateExit()
    {
        UIManager.DisablePopupText();
    }
}