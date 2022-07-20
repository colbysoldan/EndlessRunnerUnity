using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    GameObject[] panels;
    GameObject[] mmButtons;
    int maxLives = 3;

    //ensures help panel isn't open at start
    void Start()
    {
        panels = GameObject.FindGameObjectsWithTag("subpanel");
        mmButtons = GameObject.FindGameObjectsWithTag("mmbutton");

        foreach (GameObject p in panels)
            p.SetActive(false);
    }

    //goes on the 'X' button on help panel
    public void ClosePanel(Button button)
    {
        button.gameObject.transform.parent.gameObject.SetActive(false);
        foreach (GameObject b in mmButtons)
            b.SetActive(true);
    }

    //goes on the 'Help' button on menu panel
    public void OpenPanel(Button button)
    {
        button.gameObject.transform.GetChild(1).gameObject.SetActive(true);
        foreach (GameObject b in mmButtons)
            if(b != button.gameObject)
                b.SetActive(false);
    }

    //loads the game itself
    public void LoadGameScene()
    {
        //sets lives at start
        PlayerPrefs.SetInt("lives", maxLives);
        SceneManager.LoadScene("ScrollingWorld", LoadSceneMode.Single);
    }

    //method for what to do when quitting the game
    public void QuitGame()
    {
        Application.Quit();
    }

    //links hitting the escape key with the QuitGame method
    void Update()
    {
        if(Input.GetKey("escape"))
        {
            QuitGame();
        }    
    }
}
