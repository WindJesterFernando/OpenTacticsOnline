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
public abstract class ActionContainer
{
    //abstract public void Execute();
    public abstract void Update();
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

public class ActionMoveSpriteContainer : ActionContainer
{
    GameObject heroVisualRepresentation;
    float duration;
    Vector3 startPos;
    Vector3 endPos;

    float curTime;
    
    public ActionMoveSpriteContainer(GameObject heroVisualRepresentation, Vector3 startPos, Vector3 endPos, float duration)
    {
        this.heroVisualRepresentation = heroVisualRepresentation;
        this.duration = duration;
        this.startPos = startPos;
        this.endPos = endPos;
    }

    public override void Update()
    {
        curTime += Time.deltaTime;

        Vector3 lerpVal = Vector3.Lerp(startPos, endPos, curTime / duration);
        lerpVal.z = heroVisualRepresentation.transform.position.z;
        heroVisualRepresentation.transform.position = lerpVal;
        
        if(curTime >= duration)
            IsDone = true;
    }
}