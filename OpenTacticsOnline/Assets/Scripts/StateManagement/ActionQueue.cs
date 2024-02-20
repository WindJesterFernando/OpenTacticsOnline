using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ActionQueue
{
    static Queue<ActionContainer> queueOfActions;
    
    public static void Init()
    {
        queueOfActions = new Queue<ActionContainer>();
    }
    
    public static void Update()
    {
        if(queueOfActions.Count > 0)
        {
            queueOfActions.Peek().Update();
            if(queueOfActions.Peek().IsDone)
                queueOfActions.Dequeue();
        }
    }

    public static void EnqueueAction(ActionContainer action)
    {
        queueOfActions.Enqueue(action);
    }

    public static int GetActionCount()
    {
        return queueOfActions.Count;
    }
}
