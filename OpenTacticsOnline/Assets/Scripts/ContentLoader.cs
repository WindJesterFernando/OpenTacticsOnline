using System.Collections;
using System.Collections.Generic;
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
        GameObject go = new GameObject();
        SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = mapTileSprites[spriteID];
        return go;
    }

    public static Sprite GetMapTileSprite(int spriteID)
    {
        return mapTileSprites[spriteID];
    }

    public static GameObject CreateAnimatedSprite(int spriteID)
    {
        GameObject go = new GameObject();
        SpriteRenderer sr = go.AddComponent<SpriteRenderer>();

        FrameAnimator fa = go.AddComponent<FrameAnimator>();


        AnimationFrame[] idleFrames = null;
        AnimationFrame[] walkingFrames = null;
        AnimationFrame[] castingFrames = null;

        if(spriteID == 1)
        {
            Sprite[] allFrames = Resources.LoadAll<Sprite>("HeroSprites/BlackMage_Spritesheet");
            castingFrames = new AnimationFrame[5];
            castingFrames[0].sprite = allFrames[0];
            castingFrames[0].time = 0.5f;
            castingFrames[1].sprite = allFrames[6];
            castingFrames[1].time = 1.5f;
            castingFrames[2].sprite = allFrames[5];
            castingFrames[2].time = 0.15f;
            castingFrames[3].sprite = allFrames[6];
            castingFrames[3].time = 0.15f;
            castingFrames[4].sprite = allFrames[0];
            castingFrames[4].time = 0.5f;

            idleFrames = new AnimationFrame[1];
            idleFrames[0].sprite = allFrames[0];
            idleFrames[0].time = 999f;

            walkingFrames = new AnimationFrame[2];
            walkingFrames[0].sprite = allFrames[1];
            walkingFrames[0].time = 0.5f;
            walkingFrames[1].sprite = allFrames[2];
            walkingFrames[1].time = 0.5f;

        }
        
        fa.Setup(sr, idleFrames, walkingFrames, castingFrames);

        return go;
    }

}

