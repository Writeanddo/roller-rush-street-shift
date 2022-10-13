using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("soundVolume"))
        {
            LoadPrefs(); 
        }
        else
        {
            PlayerPrefs.SetFloat("soundVolume", 0.75f);
            LoadPrefs();
        }
    }

    // Update is called once per frame
    void Update()
    {
        AudioListener.volume = volumeSlider.value;
        PlayerPrefs.SetFloat("soundVolume", volumeSlider.value);
        PlayerPrefs.Save(); //I dont like to save at each frame
    }


    private void LoadPrefs()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("soundVolume");
        PlayerPrefs.SetFloat("soundVolume", volumeSlider.value); //Set at each frame ? It should be ok
    }
}
