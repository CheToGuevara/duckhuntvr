using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class LevelTime
{
    public float newTime=0;
}

[Serializable]
public class PlayerProgress
{
    public float secondsHard;
    public float secondsMed;


    PlayerProgress()
    {
        secondsHard = 120;
        secondsMed = 180;
    }

    public static PlayerProgress CreateNewPlayer()
    {
        PlayerProgress newPlayer = new PlayerProgress();


        return newPlayer;

    }

    public bool UpdateLevel(bool hardLevel, float newtime)
    {
        if (hardLevel)
        {
            if (newtime < secondsHard)
            {
                secondsHard = newtime;
                return true;
            }

        }
        else
        {
            if (newtime < secondsMed)
            {
                secondsMed = newtime;
                return true;
            }
        }

        return false;
    }

}
