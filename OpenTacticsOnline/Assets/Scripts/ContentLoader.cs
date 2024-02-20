using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class ContentLoader
{
    static Sprite[] mapTileSprites;

    public static void Init()
    {
        mapTileSprites = Resources.LoadAll<Sprite>("tileSet_64x64");
    }

    public static GameObject CreateGridTile(int spriteID)
    {
        GameObject go = new GameObject("Tile");
        SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = mapTileSprites[spriteID];
        return go;
    }

    public static Sprite GetMapTileSprite(int spriteID)
    {
        return mapTileSprites[spriteID];
    }

    public static GameObject CreateAnimatedSprite(HeroRole heroRole)
    {
        #region Setup GameObject

        GameObject go = new GameObject("Hero");
        SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
        sr.sortingOrder = 3;

        FrameAnimator fa = go.AddComponent<FrameAnimator>();
        fa.SetSpriteRenderer(sr);

        #endregion

        #region Load Sprites

        string filePath;

        switch (heroRole)
        {
            case HeroRole.BlackMage:
                filePath = "HeroSprites/BlackMage_Spritesheet";
                break;
            case HeroRole.WhiteMage:
                filePath = "HeroSprites/WhiteMage_Spritesheet";
                break;
            case HeroRole.RedMage:
                filePath = "HeroSprites/RedMage_Spritesheet";
                break;
            case HeroRole.Fighter:
                filePath = "HeroSprites/Fighter_Spritesheet";
                break;
            case HeroRole.Monk:
                filePath = "HeroSprites/Monk_Spritesheet";
                break;
            case HeroRole.Thief:
                filePath = "HeroSprites/Thief_Spritesheet";
                break;
            default:
                filePath = string.Empty;
                Debug.LogWarning("ContentLoader.CreateAnimatedSprite(): heroJobClass spritesheet not found.");
                return null;
        }

        Sprite[] allFrames = Resources.LoadAll<Sprite>(filePath);

        #endregion

        #region Setup Frames

        AnimationFrame[] idleFrames = null;
        AnimationFrame[] walkingFrames = null;
        AnimationFrame[] castingFrames = null;
        AnimationFrame[] attackingFrames = null;
        AnimationFrame[] blockingFrames = null;
        AnimationFrame[] fallingFrames = null;
        AnimationFrame[] knockedOutFrames = null;
        AnimationFrame[] hurtFrames = null;
        AnimationFrame[] danceFrames = null;

        idleFrames = new AnimationFrame[1];
        idleFrames[0] = new AnimationFrame(allFrames[0], 999f);

        walkingFrames = new AnimationFrame[2];
        walkingFrames[0] = new AnimationFrame(allFrames[1], 0.25f);
        walkingFrames[1] = new AnimationFrame(allFrames[2], 0.25f);

        castingFrames = new AnimationFrame[5];
        castingFrames[0] = new AnimationFrame(allFrames[5], 0.15f);
        castingFrames[1] = new AnimationFrame(allFrames[6], 0.15f);
        castingFrames[2] = new AnimationFrame(allFrames[5], 0.15f);
        castingFrames[3] = new AnimationFrame(allFrames[6], 0.15f);
        castingFrames[4] = new AnimationFrame(allFrames[5], 0.15f);

        attackingFrames = new AnimationFrame[2];
        attackingFrames[0] = new AnimationFrame(allFrames[3], 0.15f);
        attackingFrames[1] = new AnimationFrame(allFrames[4], 0.25f);

        blockingFrames = new AnimationFrame[1];
        blockingFrames[0] = new AnimationFrame(allFrames[7], 1.5f);

        fallingFrames = new AnimationFrame[2];
        fallingFrames[0] = new AnimationFrame(allFrames[8], 0.25f);
        fallingFrames[1] = new AnimationFrame(allFrames[9], 0.25f);

        knockedOutFrames = new AnimationFrame[1];
        knockedOutFrames[0] = new AnimationFrame(allFrames[9], 999f);

        hurtFrames = new AnimationFrame[1];
        hurtFrames[0] = new AnimationFrame(allFrames[8], 999f);

        danceFrames = new AnimationFrame[5];
        danceFrames[0] = new AnimationFrame(allFrames[0], 0.5f);
        danceFrames[1] = new AnimationFrame(allFrames[6], 1.5f);
        danceFrames[2] = new AnimationFrame(allFrames[5], 0.15f);
        danceFrames[3] = new AnimationFrame(allFrames[6], 0.15f);
        danceFrames[4] = new AnimationFrame(allFrames[0], 0.5f);

        fa.AddFrameAnimationToLibrary(AnimationKey.Idle, idleFrames);
        fa.AddFrameAnimationToLibrary(AnimationKey.Walking, walkingFrames);
        fa.AddFrameAnimationToLibrary(AnimationKey.Casting, castingFrames);
        fa.AddFrameAnimationToLibrary(AnimationKey.Attacking, attackingFrames);
        fa.AddFrameAnimationToLibrary(AnimationKey.Blocking, blockingFrames);
        fa.AddFrameAnimationToLibrary(AnimationKey.Falling, fallingFrames);
        fa.AddFrameAnimationToLibrary(AnimationKey.KnockedOut, knockedOutFrames);
        fa.AddFrameAnimationToLibrary(AnimationKey.Hurt, hurtFrames);
        fa.AddFrameAnimationToLibrary(AnimationKey.DanceChoreography, danceFrames);

        #endregion

        fa.StartAnimation(AnimationKey.Idle);

        return go;

    }

}
