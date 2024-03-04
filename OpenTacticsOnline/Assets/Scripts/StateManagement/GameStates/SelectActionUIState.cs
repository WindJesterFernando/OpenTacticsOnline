using System.Collections.Generic;

public class SelectActionUIState : AbstractGameState
{
    private Hero o;
    
    public SelectActionUIState(Hero o) 
        : base(GameState.SelectActionUI)
    {
        this.o = o;
    }

    public override void OnStateEnter()
    {
        // construct list of actions based on hero
       EnableButtons();
    }

    public override void OnStateContinue()
    {
        EnableButtons();
    }

    public override void OnStatePause()
    {
        UIManager.DisableButtons();
    }
    
    public override void OnStateExit()
    {
        UIManager.DisableButtons();
    }

    private void EnableButtons()
    {
        List<TurnAction> actions = new List<TurnAction>();
        actions.Add(new AttackTurnAction(o));
        actions.Add(new MoveTurnAction(o));
        UIManager.EnableButtons(actions); 
    }
}