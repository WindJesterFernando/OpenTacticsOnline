using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueTest : MonoBehaviour
{
    public static QueueTest instance;

    Queue<ActionContainer> queueOfActions;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        queueOfActions = new Queue<ActionContainer>();

        // queueOfActions.Enqueue(new ActionDebugLogContainer("1"));
        // queueOfActions.Enqueue(new ActionWaitContainer(0.5f));
        // queueOfActions.Enqueue(new ActionDebugLogContainer("2"));
        // queueOfActions.Enqueue(new ActionWaitContainer(0.5f));
        // queueOfActions.Enqueue(new ActionDebugLogContainer("3"));
        // queueOfActions.Enqueue(new ActionWaitContainer(0.5f));
        // queueOfActions.Enqueue(new ActionDebugLogContainer("4"));
        // queueOfActions.Enqueue(new ActionWaitContainer(0.5f));
        // queueOfActions.Enqueue(new ActionDebugLogContainer("5"));
        // queueOfActions.Enqueue(new ActionWaitContainer(0.5f));






        // queueOfActions


        //ActionContainer ac = new ActionContainer();



        // Debug.Log("1");
        // //wait 2 seconds
        // Debug.Log("2");
        // //wait 3 seconds
        // Debug.Log("3");
        // //wait 4 seconds
        // Debug.Log("4");
        // //wait 5 seconds
        // Debug.Log("5");
    }

    // Update is called once per frame
    void Update()
    {
        if(queueOfActions.Count > 0)
        {
            queueOfActions.Peek().Update();
            if(queueOfActions.Peek().IsDone)
                queueOfActions.Dequeue();
        }
    }

    public void EnqueueAction(ActionContainer action)
    {
        queueOfActions.Enqueue(action);
    }

}

abstract public class ActionContainer
{
    //abstract public void Execute();
    abstract public void Update();
    public bool IsDone;
}


public class ActionDebugLogContainer : ActionContainer
{
    string logInfo;

    public ActionDebugLogContainer(string logInfo)
    {
        this.logInfo = logInfo;
    }

    public override void Update()
    {
        Debug.Log(logInfo);
        IsDone = true;
    }
}



public class ActionWaitContainer : ActionContainer
{
    float waitTime;

    public ActionWaitContainer(float waitTime)
    {
        this.waitTime = waitTime;
    }

    public override void Update()
    {
        waitTime -= Time.deltaTime;

        if(waitTime <= 0)
        {
            IsDone = true;
        }
    }
}


public class ActionChangeTileContainer : ActionContainer
{
    int id;
    Vector2Int coord;

    public ActionChangeTileContainer(Vector2Int coord, int id)
    {
        this.id = id;
        this.coord = coord;
    }

    public override void Update()
    {
        BattleGridModelData.ChangeTileID(coord, id);
        IsDone = true;
    }
}
