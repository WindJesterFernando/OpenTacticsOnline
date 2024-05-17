using System.Collections.Generic;

public class AIContoller : AbstractController
{
    public override void DoTurn(Hero activeHero)
    {
        TargetingOptions targets = new TargetingOptions(false, TargetType.EmptyTile, PathBlocker.Ally | PathBlocker.Terrain);
         List<GridCoord> nearTiles = BattleGridModelData.FindTargetsWithinSteps(activeHero.coord, activeHero.maxSteps, targets);

         if (nearTiles.Count == 0)
         {
             BattleSystemModelData.AdvanceCurrentHeroTurnIndex();
             return;
         }
         
         // todo: change this from synced to some other random  
         // what if we can use it in the battle with 3 teams, one network one local, one ai
         int randomIndex = SyncedRandomGenerator.Next(nearTiles.Count);
         GridCoord randomGridCoord = nearTiles[randomIndex];

         StateManager.PushGameState(new HeroTurnActionState(new MoveTurnAction(activeHero), randomGridCoord));
         activeHero.coord = randomGridCoord;   
    }
}