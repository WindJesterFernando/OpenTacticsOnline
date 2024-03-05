using UnityEngine;

public abstract class TurnAction
{
    public string name;
    public abstract void Execute();
    public abstract void AddVisuals();
}


public class MoveTurnAction : TurnAction
{
    private Hero onwer;
    public MoveTurnAction(Hero onwer)
    {
        this.onwer = onwer;
        name = "Move";
    }
    
    public override void Execute()
    {
        Debug.Log("Moving");
        StateManager.PushGameState(new HeroMoveSelectionState(onwer));
    }

    public override void AddVisuals()
    {
        
    }
}

public class AttackTurnAction : TurnAction
{
    private Hero l;
    private int q;
    public bool resetS;
    private bool isTargetingFoe;

    public AttackTurnAction(Hero e, int i = 1, string m = "Attack", bool isTargetingFoe = true)
    {
        l = e;
        q = i;
        name = m;
        this.isTargetingFoe = isTargetingFoe;
        if (i == 3 && m == "MM")
            resetS = true;
    }
    
    public override void Execute()
    {
        Debug.Log("Attacking");
        StateManager.PushGameState(new HeroAttackSelectionState(l, q, isTargetingFoe));
    }

    public override void AddVisuals()
    {
        
    }
}