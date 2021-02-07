using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    [Header("Music References")]
    [SerializeField] private AudioSource levelMusicDrums;
    [SerializeField] private AudioSource levelMusicBass;
    [SerializeField] private AudioSource levelMusicSynth;

    [Header("Sound Effect References")]
    [SerializeField] private AudioSource soundEffectDeath;
    [SerializeField] private AudioSource soundEffectLightSwitch;

    [Header("Music Sync Settings")]
    [SerializeField] private float bpm = 150f;
    private int beats = 32;
    private float beatsPerSecond = 2.5f;

    private int musicIndex;

    // Start is called before the first frame update
    void Start()
    {
        CharacterController.OnPlayerMove += CycleAudio;
        CharacterController.OnNeutralTriggerActivated += PlayAllAudio;
        CharacterController.OnPlayerDeath += PlayAllAudio;
        musicIndex = -1;
    }

    private void CycleAudio()
    {
        musicIndex++;

        if (musicIndex > 2)
        {
            musicIndex = 0;
        }

        Debug.Log(levelMusicDrums.time);

        /*

        Music notes
        150 / 60 = 2.5 beats per second
        2.5*  12.8 = 32 beats in the whole track

        At a given time, 6 seconds into the track:
        6 / 12.8 = .46875
        .46875 * 32 = currently at beat 15 of 32

        */

        double currentTime = levelMusicDrums.time / 12.8;
        double currentBeat = currentTime * 32;
        double delay = currentBeat - Mathf.Floor((float)currentBeat);
    }

    private IEnumerator WaitUntilNextBeat(float delay, int musicIndex)
    {
        yield return new WaitForSeconds(delay);

        if (musicIndex == 0)
        {
            levelMusicDrums.mute = false;
            levelMusicBass.mute = true;
            levelMusicSynth.mute = true;
        } 
        else if (musicIndex == 1)
        {
            levelMusicDrums.mute = true;
            levelMusicBass.mute = false;
            levelMusicSynth.mute = true;
        } 
        else if (musicIndex == 2)
        {
            levelMusicDrums.mute = true;
            levelMusicBass.mute = true;
            levelMusicSynth.mute = false;
        }
    }

    private void PlayAllAudio()
    {
        musicIndex = -1;
        levelMusicDrums.mute = false;
        levelMusicBass.mute = false;
        levelMusicSynth.mute = false;
        StartCoroutine(WaitUntilNextBeat(0, musicIndex));
    }

    public void ChangeMusicAudioLevel(float newLevel)
    {
        audioMixer.SetFloat("musicLevel", newLevel);
    }

    public void ChangeSoundEffectAudioLevel(float newLevel)
    {
        audioMixer.SetFloat("soundEffectLevel", newLevel);
    }

    void OnDestroy()
    {
        CharacterController.OnPlayerMove -= CycleAudio; 
        CharacterController.OnNeutralTriggerActivated -= PlayAllAudio;
        CharacterController.OnPlayerDeath -= PlayAllAudio;
    }
}
