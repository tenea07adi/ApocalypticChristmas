using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TimerController;

public class TimerController : MonoBehaviour
{
    public static TimerController instance = null;

    public delegate void ActionToRun();

    private const float oneSecondValue = 1;

    // when a second pass this value is increased with one, is usefull to simplify the second calculation
    [SerializeField]
    private int currentTimeInSecond = 0;

    // this value is decreaseing and is reseting at one second
    [SerializeField]
    private float currentCalcTimmerValue = 0;

    private List<TimerActionSettings> actionsToRun = new List<TimerActionSettings>();

    // when is scheduled the next action to run
    [SerializeField]
    private int nextActionRunTime = 0;


    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (actionsToRun != null || actionsToRun.Count > 0)
        {
            if (currentCalcTimmerValue > 0)
            {
                currentCalcTimmerValue -= Time.deltaTime;
            }
            else
            {
                currentTimeInSecond++;
                currentCalcTimmerValue = oneSecondValue;

                if (nextActionRunTime != 0 && nextActionRunTime <= currentTimeInSecond)
                {
                    ExecuteActions();
                }
            }
        }
    }

    public void AddAction(int secondsToWait, bool loop, ActionToRun action)
    {
        int nextRun = currentTimeInSecond + secondsToWait;

        TimerActionSettings actionSettings = new TimerActionSettings(secondsToWait, action, currentTimeInSecond + secondsToWait, loop, 0);

        actionsToRun.Add(actionSettings);

        if (nextActionRunTime == 0 || nextActionRunTime > nextRun)
        {
            nextActionRunTime = nextRun;
        }
    }

    public void AddRandomAction(int minIntervalInSeconds, int maxIntervalInSeconds, bool loop, ActionToRun action)
    {
        int intervalFluctoatiaon = (maxIntervalInSeconds - minIntervalInSeconds) / 2;
        int secondsToWait = maxIntervalInSeconds - intervalFluctoatiaon;

        int nextRun = currentTimeInSecond + secondsToWait;

        TimerActionSettings actionSettings = new TimerActionSettings(secondsToWait, action, nextRun, loop, intervalFluctoatiaon);

        actionsToRun.Add(actionSettings);

        if (nextActionRunTime == 0 || nextActionRunTime > nextRun)
        {
            nextActionRunTime = nextRun;
        }
    }

    private void ExecuteActions()
    {
        int minNextActionToRunTime = 0;

        foreach (var ac in actionsToRun)
        {
            if (ac.NextActionRunTime <= currentTimeInSecond)
            {
                ac.ActionToRun.Invoke();

                if (ac.LoopActionRun)
                {
                    if (ac.RandomIntervalFluctoation <= 0)
                    {
                        ac.NextActionRunTime = currentTimeInSecond + ac.SecondsToWait;
                    }
                    else
                    {
                        ac.NextActionRunTime = currentTimeInSecond + UnityEngine.Random.Range(ac.SecondsToWait - ac.RandomIntervalFluctoation, ac.SecondsToWait + ac.RandomIntervalFluctoation);
                    }

                    if (minNextActionToRunTime == 0 || minNextActionToRunTime > ac.NextActionRunTime)
                    {
                        minNextActionToRunTime = ac.NextActionRunTime;
                    }
                }
                else
                {
                    actionsToRun.Remove(ac);
                }
            }
            else
            {
                if (nextActionRunTime == 0 || (minNextActionToRunTime > ac.NextActionRunTime && ac.NextActionRunTime > 0))
                {
                    minNextActionToRunTime = ac.NextActionRunTime;
                }
            }

        }

        nextActionRunTime = minNextActionToRunTime;

    }

}

public class TimerActionSettings
{
    public TimerActionSettings()
    {
        this.SecondsToWait = 0;
        this.NextActionRunTime = 0;
        this.LoopActionRun = false;
        this.RandomIntervalFluctoation = 0;
    }

    public TimerActionSettings(int secondsToWait, ActionToRun actionToRun, int nextActionRunTime, bool loopActionRun, int randomIntervalFluctoation)
    {
        this.SecondsToWait = secondsToWait;
        this.ActionToRun = actionToRun;
        this.NextActionRunTime = nextActionRunTime;
        this.LoopActionRun = loopActionRun;
        this.RandomIntervalFluctoation = randomIntervalFluctoation;
    }

    public int SecondsToWait;
    public ActionToRun ActionToRun;
    public int NextActionRunTime;
    public bool LoopActionRun;
    public int RandomIntervalFluctoation;
}