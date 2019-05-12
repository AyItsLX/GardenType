using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Menu : MonoBehaviour {

    [SerializeField] private GameObject mainMenuObj;
    [SerializeField] private GameObject creditObj;

    public static int dontShow = 0;

    public void OnStartPressed()
    {
        SceneManager.LoadScene("LevelMenu");
        if (dontShow == 0)
        {
            dontShow++;
            Tutorial.showTutorial = true;
        }
    }

    public void OnLevelPressed()
    {
        SceneManager.LoadScene("LevelMenu");
    }

    public void OnMenuPressed()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OnReturnPressed()
    {
        mainMenuObj.SetActive(true);
        creditObj.SetActive(false);
    }

    public void OnCreditPressed()
    {
        mainMenuObj.SetActive(false);
        creditObj.SetActive(true);
    }

    public void OnQuitPressed()
    {
        Application.Quit();
    }

    public void OnLevelNoPressed()
    {
        string level = EventSystem.current.currentSelectedGameObject.name;
        GameManager.currentLevel = level;
        SceneManager.LoadScene("Battlefield");
    }
}
