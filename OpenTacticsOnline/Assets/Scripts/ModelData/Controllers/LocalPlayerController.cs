public class LocalPlayerController : AbstractController
{
    public override void DoTurn(Hero activeHero)
    {
        StateManager.PushGameState(new SelectActionUIState(activeHero));
    }
}