using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{
    public static UiController instance;

    [SerializeField]
    private Text timerText;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        ManageTimer();
    }

    private void ManageTimer()
    {
        string currentTime = "";
        int currentTimeInSeconds = TimerController.instance.GetCurrentTimeInSeconds();

        string minutes = currentTimeInSeconds / 60 < 10 ? "0" + currentTimeInSeconds / 60 : (currentTimeInSeconds / 60).ToString();
        string seconds = currentTimeInSeconds % 60 < 10 ? "0" + currentTimeInSeconds % 60 : (currentTimeInSeconds % 60).ToString();


        currentTime = $"{minutes} : {seconds}";

        timerText.text = currentTime;
    }
}
