using System;
using UnityEngine;

public class AudioManager: MonoBehaviour {
    public AudioSource musicSource, sfxSource;

    public Sound[] musics, sfxs;

    public static AudioManager instance;
    
    void Awake(){
        DontDestroyOnLoad(gameObject);

        if(instance == null) instance = this;
        else{
            Destroy(gameObject);
            return;
        }
    }

    public void PlayMusic(string audioName){
        Sound s = Array.Find(musics, s => s.name == audioName);

        if(s == null){
            Debug.LogWarning("Music " + audioName + " not found");
            return;
        }
        musicSource.clip = s.clip;
        musicSource.Play();
    }

    public void PlaySFX(string audioName){
        Sound s = Array.Find(sfxs, s => s.name == audioName);

        if(s == null){
            Debug.LogWarning("Sound Effect " + audioName + " not found");
            return;
        }
        sfxSource.clip = s.clip;
        sfxSource.Play();
    }
    public void PauseMusic(){
        musicSource.Pause();
    }

    public void ResumeMusic(){
        musicSource.UnPause();
    }
    public void PauseSFX(){
        sfxSource.Pause();
    }

    public void ResumeSFX(){
        sfxSource.UnPause();
    }

    public void StopMusic(){
        musicSource.Stop();
    }
    
    public void StopSFX(){
        sfxSource.Stop();
    }
}
