using UnityEngine;

public class DebugLogVisualTask : VisualTask
{
    string logInfo;

    public DebugLogVisualTask(string logInfo)
    {
        this.logInfo = logInfo;
    }

    public override void Update()
    {
        Debug.Log(logInfo);
        IsDone = true;
    }
}
