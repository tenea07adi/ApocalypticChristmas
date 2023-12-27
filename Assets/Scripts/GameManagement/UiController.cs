using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UiController : MonoBehaviour
{
    public static UiController instance;

    [SerializeField]
    private GameObject pausePanel;
    [SerializeField]
    private GameObject endGamePanel;

    [SerializeField]
    private Text timerText;

    [SerializeField]
    private Text endGameTimerText;

    [SerializeField]
    List<UnityEngine.UI.Image> hpImages;
    [SerializeField]
    List<UnityEngine.UI.Image> difficultyImages;
    [SerializeField]
    List<UnityEngine.UI.Image> energyImages;

    private DisplayStats hpStats = null;
    private DisplayStats difficultyStats = null;
    private DisplayStats energyStats = null;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        hpStats = new DisplayStats(hpImages);
        difficultyStats = new DisplayStats(difficultyImages);
        energyStats = new DisplayStats(energyImages);
    }

    // Update is called once per frame
    void Update()
    {
        ManageTimer();
        ManageHp();
        ManageDifficulty();
        ManageEnergy();
    }

    public void TogglePausePanel()
    {
        TogglePausePanel(!pausePanel.active);
    }

    public void TogglePausePanel(bool state)
    {
        if(endGamePanel.active == false)
        {
            pausePanel.SetActive(state);
        }
    }

    public void ToggleEndGamePanel(bool state)
    {
        TogglePausePanel(false);

        endGamePanel.SetActive(state);
    }

    private void ManageHp()
    {
        hpStats.Set(CharacterController._instance.GetHP());
    }

    private void ManageDifficulty()
    {
        difficultyStats.Set(LevelDificultyManager.instance.GetCurrentDifficultyAsDisplayNumber());
    }
   
    private void ManageEnergy()
    {
        energyStats.Set(CharacterController._instance.GetEnergy());
    }

    private void ManageTimer()
    {
        string currentTime = "";
        int currentTimeInSeconds = TimerController.instance.GetCurrentTimeInSeconds();

        string minutes = currentTimeInSeconds / 60 < 10 ? "0" + currentTimeInSeconds / 60 : (currentTimeInSeconds / 60).ToString();
        string seconds = currentTimeInSeconds % 60 < 10 ? "0" + currentTimeInSeconds % 60 : (currentTimeInSeconds % 60).ToString();

        currentTime = $"{minutes} : {seconds}";

        timerText.text = currentTime;
        endGameTimerText.text = currentTime;
    }

}

public class DisplayStats
{
    public List<UnityEngine.UI.Image> images;

    public int currentValue;

    public DisplayStats(List<UnityEngine.UI.Image> images)
    {
        this.images = images;
    }

    public void Set(int value)
    {
        currentValue = value;
        ChangeDisplay();
    }

    private void ChangeDisplay()
    {
        int count = 0;

        foreach (UnityEngine.UI.Image image in images)
        {
            count++;

            if (count > currentValue)
            {
                image.color = new Vector4(image.color.r, image.color.g, image.color.b, 0.3f);
            }
            else
            {
                image.color = new Vector4(image.color.r, image.color.g, image.color.b, 1);
            }
        }
    }
}
