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
    SpriteRenderer spriteRenderer;
    Dictionary<AnimationKey, AnimationFrame[]> frameAnimationLibrary = new Dictionary<AnimationKey, AnimationFrame[]>();
    AnimationFrame[] animationFrames;
    float timeUntilNextFrame;
    int currentFrame;
    bool returnToIdleAfterCompletion;

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

                if(returnToIdleAfterCompletion)
                {
                    StartAnimation(AnimationKey.Idle);
                }
            }

            timeUntilNextFrame = animationFrames[currentFrame].time;
            spriteRenderer.sprite = animationFrames[currentFrame].sprite;
        }
    }

    public void StartAnimation(AnimationKey key, bool returnToIdleAfterCompletion = false)
    {
        currentFrame = 0;
        animationFrames = frameAnimationLibrary[key];
        spriteRenderer.sprite = animationFrames[currentFrame].sprite;
        timeUntilNextFrame = animationFrames[currentFrame].time;
        this.returnToIdleAfterCompletion = returnToIdleAfterCompletion;
    }

    public void AddFrameAnimationToLibrary(AnimationKey key, AnimationFrame[] frames)
    {
        frameAnimationLibrary.Add(key, frames);
    }

}

public class AnimationFrame
{
    public Sprite sprite;
    public float time;

    public AnimationFrame(Sprite sprite, float time)
    {
        this.sprite = sprite;
        this.time = time;
    }
}

