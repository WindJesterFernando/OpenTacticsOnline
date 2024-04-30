public class NetworkPlayerController : AbstractController
{
    private Hero activeHero;
    public NetworkPlayerController()
    {
        NetworkClientProcessing.onMessageReceived += OnMessageReceived;
    }

    private void OnMessageReceived(string msg)
    {
        if (activeHero == null)
            return;

        string[] csv = msg.Split(',');
        int signifier = int.Parse(csv[0]);

        if (signifier == ClientToServerSignifiers.ActionUsed)
        {
            int actionIndex = int.Parse(csv[1]);
            GridCoord targetCoord = new GridCoord(int.Parse(csv[2]), int.Parse(csv[3]));

            StateManager.PushGameState(new HeroTurnActionState(activeHero.actions[actionIndex], targetCoord));

            activeHero = null;
        }
    }

    public override void DoTurn(Hero activeHero)
    {
        this.activeHero = activeHero;
    }
}