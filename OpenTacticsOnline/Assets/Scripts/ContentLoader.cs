using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterJobClasses
{
    BlackMage,
    RedMage,
    WhiteMage,
    Fighter,
    Monk,
    Thief
}

public static class ContentLoader
{
    static Sprite[] mapTileSprites;


    public static void Init()
    {
        mapTileSprites = Resources.LoadAll<Sprite>("tileSet_64x64");
    }

    public static GameObject CreateGridTile(int spriteID)
    {
        GameObject go = new GameObject();
        SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = mapTileSprites[spriteID];
        return go;
    }

    public static Sprite GetMapTileSprite(int spriteID)
    {
        return mapTileSprites[spriteID];
    }

    public static GameObject CreateAnimatedSprite(CharacterJobClasses spriteID)
    {
        GameObject go = new GameObject();
        SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
        sr.sortingOrder = 3;

        FrameAnimator fa = go.AddComponent<FrameAnimator>();
        fa.SetSpriteRenderer(sr);

        AnimationFrame[] idleFrames = null;
        AnimationFrame[] walkingFrames = null;
        AnimationFrame[] castingFrames = null;

        if (spriteID == CharacterJobClasses.BlackMage)
        {
            Sprite[] allFrames = Resources.LoadAll<Sprite>("HeroSprites/BlackMage_Spritesheet");
            castingFrames = new AnimationFrame[5];
            castingFrames[0] = new AnimationFrame(allFrames[0], 0.5f);
            castingFrames[1] = new AnimationFrame(allFrames[6], 1.5f);
            castingFrames[2] = new AnimationFrame(allFrames[5], 0.15f);
            castingFrames[3] = new AnimationFrame(allFrames[6], 0.15f);
            castingFrames[4] = new AnimationFrame(allFrames[0], 0.5f);

            idleFrames = new AnimationFrame[1];
            idleFrames[0] = new AnimationFrame(allFrames[0], 999f);

            walkingFrames = new AnimationFrame[2];
            walkingFrames[0] = new AnimationFrame(allFrames[1], 0.5f);
            walkingFrames[1] = new AnimationFrame(allFrames[2], 0.5f);
        }

        fa.AddFrameAnimationToLibrary(AnimationKey.Idle, idleFrames);
        fa.AddFrameAnimationToLibrary(AnimationKey.Walking, walkingFrames);
        fa.AddFrameAnimationToLibrary(AnimationKey.Casting, castingFrames);

        fa.StartAnimation(AnimationKey.Idle);

        return go;
    }

}

