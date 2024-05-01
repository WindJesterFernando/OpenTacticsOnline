public class NetworkPlayerController : AbstractController
{
    private Hero activeHero;
    public NetworkPlayerController()
    {
        NetworkClientProcessing.OnMessageReceived += OnMessageReceived;
    }

    private void OnMessageReceived(Message msg)
    {
        if (activeHero == null)
            return;

        if (msg.signifier == NetworkSignifier.CC_ActionUsed)
        {
            int actionIndex = int.Parse(msg.values[0]);
            GridCoord targetCoord = GridCoord.Parse(msg.values[1]); 

            StateManager.PushGameState(new HeroTurnActionState(activeHero.actions[actionIndex], targetCoord));

            activeHero = null;
        }
    }

    public override void DoTurn(Hero activeHero)
    {
        this.activeHero = activeHero;
    }
}