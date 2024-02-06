using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public static class BattleSystemModelData
{ 
    static Hero[] turnOrder;
    static int currentHeroTurnIndex;

    
    
    public static void RandomlyOrderTurns()
    {
        #region Randomly Generate Priority For Turn Order
        
        const int seedForRandom = 9999;
        const int maxRandomValue = 1000;
        Random r = new Random(seedForRandom);
        
        LinkedList<HeroAndTurnPriority> unorderedHeroTurnPriorities = new LinkedList<HeroAndTurnPriority>();

        foreach (Hero h in BattleGridModelData.GetHeroes())
        {
            HeroAndTurnPriority heroAndTurnPriority = new HeroAndTurnPriority(h, r.Next(maxRandomValue));
            unorderedHeroTurnPriorities.AddLast(heroAndTurnPriority);
        }

        #endregion
        
        #region Order Heroes Into Array By Priority 
        
        turnOrder = new Hero[BattleGridModelData.GetHeroes().Count];

        int currentIndex = 0;

        while (unorderedHeroTurnPriorities.Count > 0)
        {
            HeroAndTurnPriority highest = null;

            foreach (HeroAndTurnPriority h in unorderedHeroTurnPriorities)
            {
                if (highest == null)
                    highest = h;
                else if (highest.priority < h.priority)
                    highest = h;
            }

            unorderedHeroTurnPriorities.Remove(highest);
            turnOrder[currentIndex] = highest.hero;
            currentIndex++;
            
            //Debug.Log("adding " + highest.priority);
        }
        
        #endregion
        
    }

    public static Hero GetActiveHero()
    {
        return turnOrder[currentHeroTurnIndex];
    }

    public static void AdvanceCurrentHeroTurnIndex()
    {
        currentHeroTurnIndex++;

        if (currentHeroTurnIndex >= turnOrder.Length - 1)
            currentHeroTurnIndex = 0;
    }
    
}


class HeroAndTurnPriority
{
    public Hero hero;
    public int priority;

    public HeroAndTurnPriority(Hero hero, int priority)
    {
        this.hero = hero;
        this.priority = priority;
    }
    
}