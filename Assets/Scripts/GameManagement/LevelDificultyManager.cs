using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDificultyManager : MonoBehaviour
{
    public static LevelDificultyManager instance = null;
    public enum DifficultyLevels { dif1 = 0, dif2 = 1, dif3 = 2, dif4 = 3, dif5 = 4 };

    private DifficultyLevels currentDifficulty;

    public delegate void ChangeDifficulty();

    public event ChangeDifficulty DifficultyChanged;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        currentDifficulty = 0;

        ManageDifficulty();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ManageDifficulty()
    {
        TimerController.instance.AddAction(10, true, IncreaseDifficulty);
    }

    public void DecreaseDifficulty()
    {
        if (currentDifficulty <= DifficultyLevels.dif1)
        {
            currentDifficulty = DifficultyLevels.dif1;
            return;
        }

        currentDifficulty -= 1;

        DifficultyChanged.Invoke();
    }

    public void IncreaseDifficulty()
    {

        if (currentDifficulty >= DifficultyLevels.dif5)
        {
            currentDifficulty = DifficultyLevels.dif5;
            return;
        }

        currentDifficulty += 1;

        DifficultyChanged.Invoke();
    }
}
