using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameAnimator : MonoBehaviour
{
    AnimationFrame[] animationFrames;
    float timeUntilNextFrame;

    //float timeOnEachFrame = 1.5f;

    SpriteRenderer spriteRenderer;

    int currentFrame;

    void Update()
    {
        timeUntilNextFrame -= Time.deltaTime;

        if(timeUntilNextFrame < 0)
        {
            currentFrame++;

            if(currentFrame >= animationFrames.Length)
            {
                currentFrame = 0;
            }

            timeUntilNextFrame = animationFrames[currentFrame].time;

            spriteRenderer.sprite = animationFrames[currentFrame].sprite;
        }
    }

    public void Setup(SpriteRenderer spriteRenderer, AnimationFrame[] animationFrames)
    {
        this.spriteRenderer = spriteRenderer;
        this.animationFrames = animationFrames;
        spriteRenderer.sprite = animationFrames[0].sprite;
        spriteRenderer.sortingOrder = 3;
        currentFrame = 0;
        timeUntilNextFrame = animationFrames[currentFrame].time;
    }
}


public struct AnimationFrame
{
    public Sprite sprite;
    public float time;
}

