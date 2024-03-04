using UnityEngine;

public abstract class TurnAction
{
    public string name;
    public abstract void Execute();
}


public class MoveTurnAction : TurnAction
{
    private Hero o;
    public MoveTurnAction(Hero o)
    {
        this.o = o;
        name = "Move";
    }
    
    public override void Execute()
    {
        Debug.Log("Moving");
        StateManager.PushGameState(new HeroMoveSelectionState(o));
    }
}

public class AttackTurnAction : TurnAction
{
    private Hero o;
    public AttackTurnAction(Hero e)
    {
        o = e;
        name = "Attack";
    }
    
    public override void Execute()
    {
        Debug.Log("Attacking");
        StateManager.PopGameState();
    }
}