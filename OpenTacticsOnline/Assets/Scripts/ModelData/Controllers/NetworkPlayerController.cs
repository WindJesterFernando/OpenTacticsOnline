using System.Collections.Generic;

public class NetworkPlayerController : AbstractController
{
    private Hero activeHero;
    public NetworkPlayerController()
    {
        NetworkClientProcessing.SetMessageReceiver(OnMessageReceived);
    }

    private void OnMessageReceived(Message msg)
    {
        if (msg.signifier == NetworkSignifier.S_OpponentDisconnected)
        {
            List<Hero> opponentHeroes = BattleGridModelData.GetOpponentHeroes();

            foreach (Hero hero in opponentHeroes)
            {
                hero.ModifyHealth(-999999);
            }
            
            StateManager.PopGameStateUntilStateIs(GameState.MainPlay);
            NetworkClientProcessing.ClearMessageReceiver();
        }   
        

        if (msg.signifier == NetworkSignifier.CC_ActionUsed)
        {
            if (activeHero == null)
                return;

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