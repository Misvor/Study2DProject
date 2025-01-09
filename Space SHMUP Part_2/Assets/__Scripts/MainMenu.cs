using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    private AssetBundle bundle;

    public Button exitButton;
    public Button startGame;
    public Button highscore;
    // Start is called before the first frame update
    void Start()
    {
        bundle = AssetBundle.LoadFromFile("Assets/_Prefabs");
        
        exitButton.onClick.AddListener(ExitTask);
        startGame.onClick.AddListener(StartGameTask);
        highscore.onClick.AddListener(HighscoreTask);        
    }

	
	void ExitTask()
	{
        Application.Quit();
    }
    void StartGameTask()
	{
        SceneManager.LoadScene("_Scene_0");
    }
    void HighscoreTask()
	{
        SceneManager.LoadScene("_Scene_0");
    }

   
}
