using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    //the Sound class can be found at the end of the file. it has a reference to the name & the clip file
    public Sound[] music, sfxSounds, UISounds;

    //gameObjects with the audio source component,should be children of the AudioManager object
    public AudioSource musicSource, sfxSource, UISource, globalSource;

    private void Awake(){
        
        if(Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else{
            Destroy(gameObject);
        }
    }

    //plays music globally
    public void PlayMusic(string name){
        Sound s = Array.Find(music, x => x.name == name);

        if(s == null){
            Debug.Log("Sound not found");
        } else{
            musicSource.clip = s.clip;
            musicSource.Play();
        }
    }

    //pauses all, can be resumed
    public void PauseAll(){
        musicSource.Pause();
        sfxSource.Pause();
        globalSource.Pause();
    }

    //to be used after exiting from pause menu
    public void ResumeMusic(){
        musicSource.Play();
    }

    //stops all completely, restarts on next play
    public void StopAll(){
        sfxSource.Stop();
        musicSource.Stop();
        UISource.Stop();
        globalSource.Stop();
    }

    //plays an SFX globally (same volume regardless of position). Does not repeat.
    public void PlaySFX(string name){
        Sound s = Array.Find(sfxSounds, x => x.name == name);

        if(s == null){
            Debug.Log("Sound not found");
        } else{
            globalSource.PlayOneShot(s.clip);
        }
    }

    //plays an SFX at the position of an object. (volume changes based on position) Does not repeat.
    public void PlaySFXAtPoint(string name, Transform spawnPoint){

        Sound s = Array.Find(sfxSounds, x => x.name == name);

        if(s == null){
            Debug.Log("sound not found");
        } else{
            AudioSource audioSource = Instantiate(sfxSource, spawnPoint);
            audioSource.clip = s.clip;
            audioSource.PlayOneShot(s.clip);
            
            float clipLength = audioSource.clip.length;

            Destroy(audioSource.gameObject, clipLength);            
        }
    }

    //plays a repeating SFX at the global scale (volume not related to position)
    public AudioSource PlayRepeatingGlobal(string name){
        Sound s = Array.Find(sfxSounds, x => x.name == name);

        if(s == null){
            Debug.Log("sound not found");
            return null;
        } else{
            AudioSource audioSource = Instantiate(globalSource, this.transform.position, Quaternion.identity);
            audioSource.clip = s.clip;
            audioSource.loop = true;
            audioSource.Play();
            return audioSource;
        }
    }

    //plays repeating SFX at position of an object (volume based on position)
    public AudioSource PlayRepeatingAtPoint(string name, Transform spawnPoint){

        Sound s = Array.Find(sfxSounds, x => x.name == name);

        if(s == null){
            Debug.Log("sound not found");
            return null;
        } else{
            AudioSource audioSource = Instantiate(sfxSource, spawnPoint);
            audioSource.clip = s.clip;

            audioSource.loop = true;
            audioSource.Play();
            return audioSource;
        }
    }

    //for use with buttons & stuff. Does not repeat.
    public void PlayUISound(string name){
        Sound s = Array.Find(UISounds, x => x.name == name);

        if(s == null){
            Debug.Log("Sound not found");
        } else{
            UISource.PlayOneShot(s.clip);
        }
    }
}

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}
