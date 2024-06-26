using System.Collections.Generic;

public class AIContoller : AbstractController
{
    public override void DoTurn(Hero activeHero)
    {
        TargetingOptions targets = new TargetingOptions(false, TargetType.EmptyTile, PathBlocker.Ally | PathBlocker.Terrain, false);
         List<GridCoord> nearTiles = BattleGridModelData.FindTargetsWithinSteps(activeHero.coord, activeHero.maxSteps, targets);

         if (nearTiles.Count == 0)
         {
             BattleSystemModelData.AdvanceCurrentHeroTurnIndex();
             return;
         }
         
         int randomIndex = SyncedRandomGenerator.Next(nearTiles.Count);
         GridCoord randomGridCoord = nearTiles[randomIndex];

         StateManager.PushGameState(new HeroTurnActionState(new MoveTurnAction(activeHero), randomGridCoord));
         activeHero.coord = randomGridCoord;   
    }
}