using System.Collections.Generic;

public class AIContoller : AbstractController
{
    public override void DoTurn(Hero activeHero)
    {
         List<GridCoord> nearTiles =
            BattleGridModelData.FindTargetsWithinSteps(activeHero.coord, activeHero.maxSteps,
                 new TargetingOptions(false, TargetType.EmptyTile,
                     PathBlocker.Ally | PathBlocker.Terrain));

         if (nearTiles.Count == 0)
         {
             BattleSystemModelData.AdvanceCurrentHeroTurnIndex();
             return;
         }
         
         int randomIndex = RandomGenerator.random.Next(nearTiles.Count);
         GridCoord randomGridCoord = nearTiles[randomIndex];

         StateManager.PushGameState(new HeroTurnActionState(new MoveTurnAction(activeHero), randomGridCoord));
         // StateManager.PushGameState(new HeroMovementState(activeHero, randomGridCoord));
         activeHero.coord = randomGridCoord;   
    }
}