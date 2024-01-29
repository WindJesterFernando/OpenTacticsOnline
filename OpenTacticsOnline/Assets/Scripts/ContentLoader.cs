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


        AnimationFrame[] animationFrames = null;

        if(spriteID == 1)
        {
            Sprite[] allFrames = Resources.LoadAll<Sprite>("HeroSprites/BlackMage_Spritesheet");
            animationFrames = new AnimationFrame[5];
            animationFrames[0].sprite = allFrames[0];
            animationFrames[0].time = 0.5f;
            animationFrames[1].sprite = allFrames[6];
            animationFrames[1].time = 1.5f;
            animationFrames[2].sprite = allFrames[5];
            animationFrames[2].time = 0.15f;
            animationFrames[3].sprite = allFrames[6];
            animationFrames[3].time = 0.15f;
            animationFrames[4].sprite = allFrames[0];
            animationFrames[4].time = 0.5f;
        }
        
        fa.Setup(sr, animationFrames);

        return go;
    }

}

