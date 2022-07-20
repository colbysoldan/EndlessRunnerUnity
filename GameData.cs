using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameData : MonoBehaviour
{
    public static GameData singleton;
    public Text scoreText = null;
    public GameObject musicSlider;
    public GameObject soundSlider;
    public int score = 0;

    //this will store settings, scores, etc.
    private void Awake()
    {
        GameObject[] gd = GameObject.FindGameObjectsWithTag("gamedata");

        //if a game data exists, we will destroy any new ones that try to generate
        if (gd.Length > 1)
        {
            Destroy(this.gameObject);
        }
        //if one doesn't already exist, we will create it and store all new data there
        DontDestroyOnLoad(this.gameObject);

        //we will call the singleton each time we need to access the data
        singleton = this;

        musicSlider.GetComponent<UpdateMusic>().Start();
        soundSlider.GetComponent<UpdateSound>().Start();

        //PlayerPrefs.SetInt("score", 0);
    }

    public void UpdateScore(int s)
    {
        score += s;
        //saves score when you lose a life
        PlayerPrefs.SetInt("score", score);
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }
        
}
