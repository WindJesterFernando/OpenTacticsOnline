using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueTest : MonoBehaviour
{

    Queue<ActionContainer> queueOfActions;

    // Start is called before the first frame update
    void Start()
    {
        queueOfActions = new Queue<ActionContainer>();

        queueOfActions.Enqueue(new ActionDebugLogContainer("1"));
        queueOfActions.Enqueue(new ActionWaitContainer(1));
        queueOfActions.Enqueue(new ActionDebugLogContainer("2"));
        queueOfActions.Enqueue(new ActionWaitContainer(2));
        queueOfActions.Enqueue(new ActionDebugLogContainer("3"));
        queueOfActions.Enqueue(new ActionWaitContainer(3));
        queueOfActions.Enqueue(new ActionDebugLogContainer("4"));
        queueOfActions.Enqueue(new ActionWaitContainer(4));
        queueOfActions.Enqueue(new ActionDebugLogContainer("5"));
        queueOfActions.Enqueue(new ActionWaitContainer(5));






        


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
