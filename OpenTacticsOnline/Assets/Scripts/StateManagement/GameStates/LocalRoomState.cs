using UnityEngine;
using Color = UnityEngine.Color;

public class LocalRoomState : AbstractGameState
{
     AIContoller aiController= new AIContoller();
     LocalPlayerController localPlayerController = new LocalPlayerController();

    public LocalRoomState()
    {
    }

    public override void OnStateEnter()
    {
        BattleGridModelData.Init();
        StateManager.PopGameState();
        StateManager.PushGameState(new MainBattleState());
    }

    public override void OnStateExit()
    {
        BattleGridModelData.Init();
    
        //TODO Add some functionality to customize this and send the data about what we choose to the other client
        AddHero(new Hero(2, 2, HeroRole.BlackMage, 6, 20, true));
        AddHero(new Hero(3, 2, HeroRole.RedMage, 6, 20, true));
        AddHero(new Hero(3, 3, HeroRole.WhiteMage, 6, 20, true));
    
        AddHero(new Hero(15, 7, HeroRole.Fighter, 8, 20, false));
        AddHero(new Hero(15, 6, HeroRole.Monk, 8, 20, false));
        AddHero(new Hero(15, 5, HeroRole.Thief, 8, 20, false));

        Camera.main.backgroundColor = Color.blue;
    }
    private void AddHero(Hero hero)
    {
        if (hero.isAlly)
        {
            hero.controller = localPlayerController;
        }
        else
        {
            hero.controller = aiController;
        }
        BattleGridModelData.AddHero(hero);
    }
}