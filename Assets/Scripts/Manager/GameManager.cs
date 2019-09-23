using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private CreationManager creationManagerScript;
    private GameObject background;
    private GameObject canvas;
    private GameObject menu;
    private GameObject mainMenu;
    private GameObject levelMenu;
    private GameObject title;
    private GameObject gameOverTitle;
    private Button buttonLevelMode;
    private Button buttonSimulation;
    private int highestLevel;

    // Start is called before the first frame update
    void Start()
    {
        creationManagerScript = gameObject.GetComponent<CreationManager>();
        highestLevel = int.Parse(File.ReadAllLines(Constants.InfoFilePath)[0]);
        FindUIObjects();
    }

    void FindUIObjects()
    {
        canvas = GameObject.Find(Constants.Canvas);
        background = GameObject.Find(Constants.Background);
        title = GameObject.Find(Constants.TextTitle);
        menu = GameObject.Find(Constants.Menu);
        mainMenu = GameObject.Find(Constants.MainMenu);
        gameOverTitle = canvas.transform.GetChild(1).gameObject;
        levelMenu = menu.transform.GetChild(1).gameObject;
        buttonLevelMode = GameObject.Find(Constants.ButtonLevel).GetComponent<Button>();
        buttonLevelMode.onClick.AddListener(OnButtonLevelModeClicked);
        buttonSimulation = GameObject.Find(Constants.ButtonSimulation).GetComponent<Button>();
    }

    void OnButtonLevelModeClicked()
    {
        mainMenu.SetActive(false);
        levelMenu.SetActive(true);
        for (int levelNum = 1; levelNum <= 5; levelNum++)
        {
            if (levelNum <= highestLevel)
            {
                AddPlayLevelListener(levelNum);
            }
            else
            {
                GameObject.Find(Constants.ButtonLevel + levelNum).SetActive(false);
            }
        }
    }

    void AddPlayLevelListener(int levelNum)
    {
        Button button = GameObject.Find(Constants.ButtonLevel + levelNum).GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            PrepareForLevel(levelNum);
        });
    }

    void PrepareForLevel(int levelNum)
    {
        menu.SetActive(false);
        background.SetActive(false);
        creationManagerScript.CreateWorldForMap(levelNum);
    }

    public void GameOver()
    {
        gameOverTitle.SetActive(true);
        Invoke("LoadHomeScene", 5);
    }

    public void LoadHomeScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
