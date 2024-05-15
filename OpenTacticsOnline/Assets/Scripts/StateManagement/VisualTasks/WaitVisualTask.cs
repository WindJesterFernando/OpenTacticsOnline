using UnityEngine;

public class WaitVisualTask : VisualTask
{
    float waitTime;

    public WaitVisualTask(float waitTime)
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