using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameAnimator : MonoBehaviour
{
    Sprite[] spriteFrames;
    float timeUntilNextFrame;

    float timeOnEachFrame = 1.5f;

    SpriteRenderer spriteRenderer;

    void Update()
    {
        
    }

    public void Setup(SpriteRenderer spriteRenderer, Sprite[] spriteFrames, float timeOnEachFrame)
    {
        this.timeOnEachFrame = timeOnEachFrame;
        this.spriteRenderer = spriteRenderer;
        this.spriteFrames = spriteFrames;
        spriteRenderer.sprite = spriteFrames[0];
    }
}
