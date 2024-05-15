public class SelectActionUIState : AbstractGameState
{
    private Hero actingHero;
    
    public SelectActionUIState(Hero actingHero) 
        : base(GameState.SelectActionUI)
    {
        this.actingHero = actingHero;
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
        UIManager.EnableButtons(actingHero.actions); 
    }
}