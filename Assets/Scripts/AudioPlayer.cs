using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] private string[] _soundNames;
    [SerializeField] private AudioClip[] _sounds;
    [SerializeField] private AudioSource _soundPlayer, _musicPlayer;
    [SerializeField] private float _musicVolume;

    private Dictionary<string, AudioClip> _allSounds = new Dictionary<string, AudioClip>();

    public static AudioPlayer Player;

    private void Awake()
    {
        Player = this;
    }

    private void Start()
    {
        ChangeState();

        for(int i = 0; i < _soundNames.Length; i++)
        {
            _allSounds[_soundNames[i]] = _sounds[i];
        }

        PlayMusic();
    }

    public void ChangeState()
    {
        _soundPlayer.enabled = PlayerPrefs.GetInt("SoundPlays") == 1;
        _musicPlayer.volume = PlayerPrefs.GetInt("MusicPlays") * _musicVolume;
    }

    public void PlayMusic()
    {
        _musicPlayer.clip = _allSounds["music"];
        _musicPlayer.Play();
    }

    public void PlaySounds(string sound)
    {
        _soundPlayer.clip = _allSounds[sound];
        _soundPlayer.Play();
    }
}
