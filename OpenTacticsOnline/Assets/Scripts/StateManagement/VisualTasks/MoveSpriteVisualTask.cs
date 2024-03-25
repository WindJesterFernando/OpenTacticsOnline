using UnityEngine;

public class MoveSpriteVisualTask : VisualTask
{
    GameObject heroVisualRepresentation;
    float duration;
    Vector3 startPos;
    Vector3 endPos;

    float curTime;
    
    public MoveSpriteVisualTask(GameObject heroVisualRepresentation, Vector3 startPos, Vector3 endPos, float duration)
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
