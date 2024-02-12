using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnimationKey
{
    Idle,
    Walking,
    Casting,
    Blocking,
    Attacking,
    Falling,
    KnockedOut,
    Hurt,
    DanceChoreography
}

public class FrameAnimator : MonoBehaviour
{
    Dictionary<AnimationKey, AnimationFrame[]> frameAnimationLibrary = new Dictionary<AnimationKey, AnimationFrame[]>();

    //AnimationFrame[] idleAnimationFrames;
    //AnimationFrame[] walkingAnimationFrames;
    //AnimationFrame[] castingAnimationFrames;

    AnimationFrame[] animationFrames;
    float timeUntilNextFrame;

    SpriteRenderer spriteRenderer;

    int currentFrame;

    bool returnToIdleAfterAnimationFrameCompletion;

    public void SetSpriteRenderer(SpriteRenderer spriteRenderer)
    {
        this.spriteRenderer = spriteRenderer;
    }

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
                    animationFrames = frameAnimationLibrary[AnimationKey.Idle];
                }
            }

            timeUntilNextFrame = animationFrames[currentFrame].time;

            spriteRenderer.sprite = animationFrames[currentFrame].sprite;
        }

        #region Animation Debug

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            StartAnimation(AnimationKey.Idle);
        }
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            StartAnimation(AnimationKey.Walking);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            StartAnimation(AnimationKey.Casting, true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            StartAnimation(AnimationKey.Blocking, true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            StartAnimation(AnimationKey.Attacking, true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            StartAnimation(AnimationKey.Falling, true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            StartAnimation(AnimationKey.KnockedOut);
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            StartAnimation(AnimationKey.Hurt);
        }
        if(Input.GetKeyDown(KeyCode.Alpha9))
        {
            StartAnimation(AnimationKey.DanceChoreography);
        }

        #endregion
    }

    public void StartAnimation(AnimationKey key, bool returnToIdleAfterAnimationFrameCompletion = false)
    {
        currentFrame = 0;
        animationFrames = frameAnimationLibrary[key];
        spriteRenderer.sprite = animationFrames[currentFrame].sprite;
        timeUntilNextFrame = animationFrames[currentFrame].time;
        this.returnToIdleAfterAnimationFrameCompletion = returnToIdleAfterAnimationFrameCompletion;
    }

    public void AddFrameAnimationToLibrary(AnimationKey key, AnimationFrame[] frames)
    {
        frameAnimationLibrary.Add(key, frames);
    }
    
}

public class AnimationFrame
{
    public AnimationFrame(Sprite sprite, float time)
    {
        this.sprite = sprite;
        this.time = time;
    }

    public Sprite sprite;
    public float time;
}

