using System.Collections.Generic;

public class MainBattleState : AbstractGameState
{
    public MainBattleState()
    {
    }

    public override void OnStateEnter()
    {
        GridVisuals.CreateBattleGridVisuals(BattleGridModelData.GetBattleGridTiles());
        BattleSystemModelData.RandomlyOrderTurns();
    }

    public override void OnStateContinue()
    {
        base.OnStateContinue();
        BattleSystemModelData.AdvanceCurrentHeroTurnIndex();
    }

    public override void Update()
    {
        Hero nextHero = BattleSystemModelData.GetActiveHero();

        if (!nextHero.IsAlive())
        {
            nextHero.GetVisualRepresentation().GetComponent<FrameAnimator>().StartAnimation(AnimationKey.KnockedOut);
            
            BattleSystemModelData.AdvanceCurrentHeroTurnIndex();
            return;
        }
        
        if (CheckBattleEndConditions()) 
            return;

        nextHero.DoTurn();
    }

    private static bool CheckBattleEndConditions()
    {
        if (IsTeamDead(BattleGridModelData.GetAllyHeroes()))
        {
            StateManager.PushGameState(new GameResultsState(false));
            return true;
        }

        if (IsTeamDead(BattleGridModelData.GetOpponentHeroes()))
        {
            StateManager.PushGameState(new GameResultsState(true));
            return true;
        }

        return false;
    }

    private static bool IsTeamDead(List<Hero> team)
    {
        foreach (Hero h in team)
        {
            if (h.IsAlive())
            {
                return false;
            }
        }
        return true;
    }

    public override void OnStateExit()
    {
        GridVisuals.DestroyBattleGridVisuals();
        BattleSystemModelData.Reset();
        BattleGridModelData.Reset();
        VisualTaskQueue.Reset();
    }
}