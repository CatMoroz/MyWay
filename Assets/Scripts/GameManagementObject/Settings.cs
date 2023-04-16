using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] private Toggle _sound, _music;

    private void Awake()
    {
        if(!PlayerPrefs.HasKey("SoundPlays"))
            PlayerPrefs.SetInt("SoundPlays", 1);

        if (!PlayerPrefs.HasKey("MusicPlays"))
            PlayerPrefs.SetInt("MusicPlays", 1);

        _sound.isOn = PlayerPrefs.GetInt("SoundPlays") != 1;
        _music.isOn = PlayerPrefs.GetInt("MusicPlays") != 1;

        if (AudioPlayer.Player != null)
            AudioPlayer.Player.ChangeState();
    }

    public void SoundsOffOn(bool state)
    {
        PlayerPrefs.SetInt("SoundPlays", (!state).GetHashCode());

        if (AudioPlayer.Player != null)
            AudioPlayer.Player.ChangeState();
    }

    public void MusicOffOn(bool state)
    {
        PlayerPrefs.SetInt("MusicPlays", (!state).GetHashCode());
        
        if (AudioPlayer.Player != null)
            AudioPlayer.Player.ChangeState();
    }
}
