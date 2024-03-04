using System.Collections.Generic;
using UnityEngine;

public class HeroAttackSelectionState : AbstractGameState
{
    private Hero attackingHero;
    private int range;
    
    private LinkedList<GridCoord> tilesThatCanBeMovedTo;
    
    public HeroAttackSelectionState(Hero attackingHero, int range) : base(GameState.AttackSelection)
    {
        this.attackingHero = attackingHero;
        this.range = range;
    }

    public override void OnStateEnter()
    {
        // todo figure out bool param 
        tilesThatCanBeMovedTo = BattleGridModelData.GetHeroesWithinSteps(attackingHero.coord, range, !attackingHero.isAlly);

        if (tilesThatCanBeMovedTo.Count == 0)
        {
            Debug.Log("No tiles to move to");
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
                attackingHero.visualRepresentation.GetComponent<FrameAnimator>()
                    .StartAnimation(AnimationKey.Attacking, true);
                
                ActionQueue.EnqueueAction(new ActionWaitContainer(0.5f));
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