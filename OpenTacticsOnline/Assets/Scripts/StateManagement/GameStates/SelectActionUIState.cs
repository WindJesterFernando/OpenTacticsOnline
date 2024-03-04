using System.Collections.Generic;

public class SelectActionUIState : AbstractGameState
{
    private Hero owner;
    
    public SelectActionUIState(Hero owner) 
        : base(GameState.SelectActionUI)
    {
        this.owner = owner;
    }

    public override void OnStateEnter()
    {
        // construct list of actions based on hero
        List<TurnAction> actions = new List<TurnAction>();
        actions.Add(new AttackTurnAction());
        actions.Add(new MoveTurnAction());
        UIManager.EnableButtons(actions);
    }

    public override void OnStateExit()
    {
        UIManager.DisableButtons();
    }
}