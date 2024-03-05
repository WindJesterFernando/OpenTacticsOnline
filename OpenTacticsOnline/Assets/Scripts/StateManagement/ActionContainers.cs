using UnityEngine;

public abstract class ActionContainer
{
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
    GridCoord coord;

    public ActionChangeTileContainer(GridCoord coord, int id)
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

public class ExecuteTurnActionContainer : ActionContainer
{
    private readonly TurnAction actionToExecute;
    private GridCoord target;
    public ExecuteTurnActionContainer(TurnAction action, GridCoord target)
    {
        actionToExecute = action;
        this.target = target;
    }
    public override void Update()
    {
        actionToExecute.Execute(target);
        IsDone = true;
    }
}