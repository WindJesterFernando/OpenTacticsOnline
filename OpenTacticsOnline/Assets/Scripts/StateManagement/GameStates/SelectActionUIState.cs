public class SelectActionUIState : AbstractGameState
{
    private Hero actingHero;
    
    public SelectActionUIState(Hero actingHero) 
    {
        this.actingHero = actingHero;
    }

    public override void OnStateEnter()
    {
        // construct list of actions based on hero role
       EnableButtons();
    }

    public override void OnStateContinue()
    {
        EnableButtons();
    }

    public override void OnStatePause()
    {
        UIManager.DisableActionButtons();
    }
    
    public override void OnStateExit()
    {
        UIManager.DisableActionButtons();
    }

    private void EnableButtons()
    {
        UIManager.EnableActionButtons(actingHero.actions); 
    }
}