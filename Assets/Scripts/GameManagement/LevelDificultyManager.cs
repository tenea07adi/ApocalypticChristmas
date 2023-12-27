using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDificultyManager : BasePausableGameObjectController
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
    protected override void UpdateLogic()
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

        DifficultyChanged?.Invoke();
    }

    public void IncreaseDifficulty()
    {

        if (currentDifficulty >= DifficultyLevels.dif5)
        {
            currentDifficulty = DifficultyLevels.dif5;
            return;
        }

        currentDifficulty += 1;

        DifficultyChanged?.Invoke();
    }

    public DifficultyLevels GetCurrentDifficulty()
    {
        return currentDifficulty;
    }

    public int GetCurrentDifficultyAsDisplayNumber()
    {
       switch (currentDifficulty)
        {
            case DifficultyLevels.dif1:
                return 1;
            case DifficultyLevels.dif2:
                return 2;
            case DifficultyLevels.dif3:
                return 3;
            case DifficultyLevels.dif4:
                return 4;
            case DifficultyLevels.dif5:
                return 5;
        }
        return 0;
    }

}
