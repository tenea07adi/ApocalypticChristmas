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
    private Scene currentScene;

    [SerializeField]
    private List<SceneAsset> scenes = new List<SceneAsset>();

    private List<SceneDetails> scenesDetails = new List<SceneDetails>();

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

    }

    public void Resume()
    {

    }
    
    public void LoadLevel(SceneAsset lvlScene)
    {
        scenesDetails.Where(c => c.sceneName == lvlScene.name).First()?.Load();
    }

    public void Restart()
    {
        scenesDetails.Where(c => c.sceneName == currentScene.name).First().Restart();
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
            if(UiController.instance != null)
            {
                UiController.instance.TogglePausePanel();
            }
        }
    }

    private void CheckIfGameEnded()
    {
        if (GameEnded())
        {
            if (UiController.instance != null)
            {
                UiController.instance.ToggleEndGamePanel(true);
            }
        }
    }

    private bool GameEnded()
    {
        if (!CharacterController._instance.IsAllive())
        {
            return true;
        }
        return false;
    }

    private void LoadScenesData()
    {
        foreach (var scene in scenes)
        {
            if (scene != null)
            {
                scenesDetails.Add(new SceneDetails(scene.name));
            }
        }

        // get current scene
        currentScene = SceneManager.GetActiveScene();
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
        Debug.Log(sceneName);
        SceneManager.LoadScene(sceneName);
    }
}
