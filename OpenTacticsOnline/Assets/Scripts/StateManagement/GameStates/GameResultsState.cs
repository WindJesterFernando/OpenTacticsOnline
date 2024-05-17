using UnityEngine;

public class GameResultsState : AbstractGameState
{
    private bool playerWon;
    
    public GameResultsState( bool playerWon)
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
        UIManager.EnableMenuButton();
    }

    public override void OnStateExit()
    {
        UIManager.DisablePopupText();
        UIManager.DisableMenuButton();
    }
}