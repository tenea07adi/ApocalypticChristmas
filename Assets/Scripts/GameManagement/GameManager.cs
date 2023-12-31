using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField]
    private string menuSceneName = "MenuScene";

    [SerializeField]
    private string currentSceneName;

    [SerializeField]
    private List<string> scenesNames = new List<string>();

    private List<SceneDetails> scenesDetails = new List<SceneDetails>();

    private bool isPaused = false;
    private bool isEnded = false;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        LoadScenesData();
    }

    // Update is called once per frame
    void Update()
    {
        CheckForManagementCommands();
        CheckIfGameEnded();
    }

    public void Pause()
    {
        if (isEnded)
        {
            return;
        }

        isPaused = true;

        if (UiController.instance != null)
        {
            UiController.instance.TogglePausePanel(isPaused);
        }

        ToggleAllPaussableObjects(isPaused);
    }

    public void Resume()
    {
        if (isEnded)
        {
            return;
        }

        isPaused = false;

        if (UiController.instance != null)
        {
            UiController.instance.TogglePausePanel(isPaused);
        }

        ToggleAllPaussableObjects(isPaused);
    }
    
    public void LoadLevel(string lvlSceneString)
    {
        scenesDetails.Where(c => c.sceneName == lvlSceneString).First()?.Load();
    }

    public void Restart()
    {
        scenesDetails.Where(c => c.sceneName == currentSceneName).First().Restart();
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ExitToMenu()
    {
        scenesDetails.Where(c => c.sceneName == menuSceneName).First()?.Load();
    }

    private void CheckForManagementCommands()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    private void CheckIfGameEnded()
    {
        if (GameEnded() && !isEnded)
        {
            if (UiController.instance != null)
            {
                Pause();
                UiController.instance.ToggleEndGamePanel(true);
            }

            isEnded = true;
        }
    }

    private bool GameEnded()
    {
        if (CharacterController._instance != null && !CharacterController._instance.IsAllive())
        {
            return true;
        }

        return false;
    }

    private void LoadScenesData()
    {
        foreach (var scene in scenesNames)
        {
            if (scene != null)
            {
                scenesDetails.Add(new SceneDetails(scene));
            }
        }

        // get current scene
        currentSceneName = SceneManager.GetActiveScene().name;
    }

    private void ToggleAllPaussableObjects(bool state)
    {
        var objs = GetPaussableObjects();

        foreach (var obj in objs)
        {
            obj.TogglePause(state);
        }
    }

    private List<BasePausableGameObjectController> GetPaussableObjects()
    {
        return FindObjectsOfType<BasePausableGameObjectController>().ToList();
    }
}

public class SceneDetails
{
    public string sceneName = null;

    public SceneDetails(string sceneName)
    {
        this.sceneName = sceneName;
    }

    public void Restart()
    {
        SceneManager.LoadScene(sceneName);
    }

    public void Load()
    {
        SceneManager.LoadScene(sceneName);
    }
}
