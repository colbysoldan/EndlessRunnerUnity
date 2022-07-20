using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateSound : MonoBehaviour
{
    List<AudioSource> sfx = new List<AudioSource>();

    // Start is called before the first frame update
    public void Start()
    {
        //creates an array of all audio sources
        AudioSource[] allAS = GameObject.FindWithTag("gamedata").GetComponentsInChildren<AudioSource>();

        //adds background music

        for (int i = 1; i < allAS.Length; i++)
            sfx.Add(allAS[i]);
        
        Slider sfxSlider = this.GetComponent<Slider>();

        //saves playersetting for music volume
        if (PlayerPrefs.HasKey("sfxvolume"))
        {
            sfxSlider.value = PlayerPrefs.GetFloat("sfxvolume");
            UpdateSoundVolume(sfxSlider.value);
        }
        else
        {
            //default music volume
            sfxSlider.value = 1;
            UpdateSoundVolume(1);
        }
    }

    public void UpdateSoundVolume(float value)
    {
        PlayerPrefs.SetFloat("sfxvolume", value);
        foreach (AudioSource s in sfx)
        {
            s.volume = value;
        }
    }
}
