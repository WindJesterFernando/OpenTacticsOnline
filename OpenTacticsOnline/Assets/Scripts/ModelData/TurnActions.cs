using UnityEngine;

public abstract class TurnAction
{
    public string name;
    public abstract void Execute();
}


public class MoveTurnAction : TurnAction
{
    public MoveTurnAction()
    {
        name = "Move";
    }
    
    public override void Execute()
    {
        Debug.Log("Moving");
        StateManager.PopGameState();
    }
}

public class AttackTurnAction : TurnAction
{
    public AttackTurnAction()
    {
        name = "Attack";
    }
    
    public override void Execute()
    {
        Debug.Log("Attacking");
        StateManager.PopGameState();
    }
}