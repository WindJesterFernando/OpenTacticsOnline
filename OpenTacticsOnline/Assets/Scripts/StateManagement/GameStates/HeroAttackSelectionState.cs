using System.Collections.Generic;
using UnityEngine;

public class HeroAttackSelectionState : AbstractGameState
{
    private Hero attackingHero;
    private int range;
    
    private LinkedList<GridCoord> tilesThatCanBeMovedTo;

    private bool isTargetingFoe;
    
    public HeroAttackSelectionState(Hero attackingHero, int range, bool isTargetingFoe) : base(GameState.AttackSelection)
    {
        this.isTargetingFoe = isTargetingFoe;
        this.attackingHero = attackingHero;
        this.range = range;
    }

    public override void OnStateEnter()
    {
        // todo figure out bool param 
        tilesThatCanBeMovedTo = BattleGridModelData.GetHeroesWithinSteps(attackingHero.coord, range, isTargetingFoe);

        if (tilesThatCanBeMovedTo.Count == 0)
        {
            Debug.Log("No Heroes to attack");
            //TODO
            // BattleSystemModelData.AdvanceCurrentHeroTurnIndex();
            
            StateManager.PopGameState();
        }
        
        foreach (GridCoord t in tilesThatCanBeMovedTo)
        {
            GridVisuals.ChangeColorOfTile(t, Color.magenta);
        }
    }
    
    public override void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GridCoord coord = BattleGridModelData.GetTileUnderMouse();

            if (tilesThatCanBeMovedTo.Contains(coord))
            {
                
                StateManager.PushGameState(new HeroTurnActionState(attackingHero));
            }
            else
            {
                StateManager.PopGameState();
            }
            
            foreach (GridCoord t in tilesThatCanBeMovedTo)
            {
                GridVisuals.ChangeColorOfTile(t, Color.white);
            }
        }
    }
}