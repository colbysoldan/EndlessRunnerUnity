using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateMusic : MonoBehaviour
{
    List<AudioSource> music = new List<AudioSource>();

    // Start is called before the first frame update
    public void Start()
    {
        //creates an array of all audio sources
        AudioSource[] allAS = GameObject.FindWithTag("gamedata").GetComponentsInChildren<AudioSource>();

        //adds background music
        music.Add(allAS[0]);
        Slider musicSlider = this.GetComponent<Slider>();

        //saves playersetting for music volume
        if(PlayerPrefs.HasKey("musicvolume"))
        {
            musicSlider.value = PlayerPrefs.GetFloat("musicvolume");
            UpdateMusicVolume(musicSlider.value);
        }
        else
        {
            //default music volume
            musicSlider.value = 1;
            UpdateMusicVolume(1);
        }
    }

    public void UpdateMusicVolume(float value)
    {
        PlayerPrefs.SetFloat("musicvolume", value);
        foreach(AudioSource m in music)
        {
            m.volume = value;
        }
    }
}
