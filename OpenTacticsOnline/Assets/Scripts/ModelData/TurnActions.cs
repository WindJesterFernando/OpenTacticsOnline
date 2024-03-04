using System.Net.Mail;
using Unity.Mathematics;
using UnityEngine;

public abstract class TurnAction
{
    public string name;
    public abstract void Execute();
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
}

public class AttackTurnAction : TurnAction
{
    private Hero l;
    private int q;
    public bool resetS;
    
    public AttackTurnAction(Hero e, int i = 1, string m = "Attack")
    {
        l = e;
        q = i;
        name = m;
        if (i == 3 && m == "MM")
            resetS = true;
    }
    
    public override void Execute()
    {
        Debug.Log("Attacking");
        StateManager.PushGameState(new HeroAttackSelectionState(l, q));
    }
}