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
    [SerializeField] private AudioSource EndingTheme;
    [SerializeField] private AudioSource levelComplete;

    [Header("Sound Effect References")]
    [SerializeField] private AudioSource soundEffectDeath;
    [SerializeField] private AudioSource soundEffectLightSwitch;

    [Header("Music Sync Settings")]
    [SerializeField] private float bpm = 150f;
    private int beats = 128;
    private float beatInterval = .625f;

    private int musicIndex;
    private bool shouldPlaySwitchSound;

    void Start()
    {
        CharacterController.OnPlayerMove += CycleAudio;
        CharacterController.OnNeutralTriggerActivated += PlayAllAudio;
        CharacterController.OnPlayerDeathAnimationTriggered += PlayerDeathAudio;
        CharacterController.OnPlayerDeath += PlayerDeathResetAudio;
        CharacterController.OnPlayerCompleteLevel += PlayLevelCompleteMusic;
        musicIndex = -1;
        shouldPlaySwitchSound = true;
    }

    private void CycleAudio()
    {
        // Increment index of instrument to play
        musicIndex++;
        if (musicIndex > 2)
        {
            musicIndex = 0;
        }

        // Calculate delay for switching instrument on beat
        double currentTime = levelMusicDrums.time / 12.8;
        double currentBeat = currentTime * beats;
        float delay = (float)(Mathf.CeilToInt((float)currentBeat) - currentBeat) * beatInterval;
        StartCoroutine(WaitUntilNextBeat(delay, musicIndex));

        // Determine if light switch should play
        if (shouldPlaySwitchSound) {
            shouldPlaySwitchSound = false;
            soundEffectLightSwitch.Play();
        }        
    }

    private IEnumerator WaitUntilNextBeat(float delay, int musicIndex)
    {
        /*
            Music notes
            150 / 60 = 2.5 beats per second
            1.25 * 12.8 = 64 beats in the whole track

            At a given time, 6 seconds into the track:
            6 / 12.8 = .46875
            .46875 * 64 = currently at beat 15 of 32
        */
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
        // Reset flags
        musicIndex = -1;
        soundEffectLightSwitch.Play();
        shouldPlaySwitchSound = true;

        // Unmute music
        levelMusicDrums.mute = false;
        levelMusicBass.mute = false;
        levelMusicSynth.mute = false;
    }

    private void StopAllAudio()
    {
        levelMusicDrums.mute = true;
        levelMusicBass.mute = true;
        levelMusicSynth.mute = true;
    }

    private void PlayLevelCompleteMusic()
    {
        levelComplete.Play();
    }

    private void PlayerDeathAudio()
    {
        soundEffectDeath.Play();
    }

    private void PlayerDeathResetAudio()
    {
        PlayAllAudio();
    }

    public void ChangeMusicAudioLevel(float newLevel)
    {
        audioMixer.SetFloat("musicLevel", newLevel);
    }

    public void ChangeSoundEffectAudioLevel(float newLevel)
    {
        audioMixer.SetFloat("soundEffectLevel", newLevel);
    }

    public void PlayEndingTheme()
    {
        StopAllAudio();
        EndingTheme.Play();
    }

    void OnDestroy()
    {
        // Clean up event connections
        CharacterController.OnPlayerMove -= CycleAudio; 
        CharacterController.OnNeutralTriggerActivated -= PlayAllAudio;
        CharacterController.OnPlayerDeathAnimationTriggered -= PlayerDeathAudio;
        CharacterController.OnPlayerDeath -= PlayerDeathAudio;
        CharacterController.OnPlayerCompleteLevel -= PlayerDeathResetAudio;
    }
}
