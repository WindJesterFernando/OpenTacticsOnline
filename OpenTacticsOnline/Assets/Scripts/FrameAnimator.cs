using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameAnimator : MonoBehaviour
{
    AnimationFrame[] idleAnimationFrames;
    AnimationFrame[] walkingAnimationFrames;
    AnimationFrame[] castingAnimationFrames;
    
    AnimationFrame[] animationFrames;
    float timeUntilNextFrame;

    SpriteRenderer spriteRenderer;

    int currentFrame;

    bool returnToIdleAfterAnimationFrameCompletion;

    void Update()
    {
        timeUntilNextFrame -= Time.deltaTime;

        if (timeUntilNextFrame < 0)
        {
            currentFrame++;

            if (currentFrame >= animationFrames.Length)
            {
                currentFrame = 0;

                if(returnToIdleAfterAnimationFrameCompletion)
                {
                    animationFrames = idleAnimationFrames;
                }
            }

            timeUntilNextFrame = animationFrames[currentFrame].time;

            spriteRenderer.sprite = animationFrames[currentFrame].sprite;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            StartAnimation(ref castingAnimationFrames, true);

        }
    }

    public void Setup(SpriteRenderer spriteRenderer, AnimationFrame[] idleAnimationFrames, AnimationFrame[] walkingAnimationFrames, AnimationFrame[] castingAnimationFrames)
    {
        spriteRenderer.sortingOrder = 3;

        this.spriteRenderer = spriteRenderer;
        this.idleAnimationFrames = idleAnimationFrames;
        this.walkingAnimationFrames = walkingAnimationFrames;
        this.castingAnimationFrames = castingAnimationFrames;

        StartAnimation(ref idleAnimationFrames);
    }

    private void StartAnimation(ref AnimationFrame[] animationFrames, bool returnToIdleAfterAnimationFrameCompletion = false)
    {
        currentFrame = 0;
        this.animationFrames = animationFrames;
        spriteRenderer.sprite = animationFrames[currentFrame].sprite;
        timeUntilNextFrame = animationFrames[currentFrame].time;
        this.returnToIdleAfterAnimationFrameCompletion = returnToIdleAfterAnimationFrameCompletion;
    }
    
}

public struct AnimationFrame
{
    public Sprite sprite;
    public float time;
}

