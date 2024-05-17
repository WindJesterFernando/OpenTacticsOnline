using UnityEngine;

public class MoveSpriteVisualTask : VisualTask
{
    GameObject heroVisualRepresentation;
    float duration;
    Vector3 startPos;
    Vector3 endPos;

    float currentTime;
    
    public MoveSpriteVisualTask(GameObject heroVisualRepresentation, Vector3 startPos, Vector3 endPos, float duration)
    {
        this.heroVisualRepresentation = heroVisualRepresentation;
        this.duration = duration;
        this.startPos = startPos;
        this.endPos = endPos;
    }

    public override void Update()
    {
        currentTime += Time.deltaTime;

        Vector3 lerpVal = Vector3.Lerp(startPos, endPos, currentTime / duration);
        lerpVal.z = heroVisualRepresentation.transform.position.z;
        heroVisualRepresentation.transform.position = lerpVal;
        
        if(currentTime >= duration)
            IsDone = true;
    }
}