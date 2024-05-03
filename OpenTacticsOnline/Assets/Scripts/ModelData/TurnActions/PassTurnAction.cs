using System.Collections.Generic;
using UnityEngine;

public class PassTurnAction : TurnAction
{
    public PassTurnAction(Hero owner) : base(owner, "Pass")
    {
    }

    public override void ApplyEffectToModelData(GridCoord target)
    {
        UnityEngine.Debug.Log("Pass");
    }

    public override void EnqueueVisualSequence(GridCoord target)
    {
        VisualTaskQueue.EnqueueAction(new ApplyEffectToModelDataVisualTask(this, target));
    }
}