using DarknessWarGodLearning;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSvc : MonoBehaviour
{
    public static AudioSvc Instance { get; private set; }

    public AudioSource bgAudio;
    public AudioSource uiAudio;
    public void InitSvc()
    {
        Instance = this;
    }
    public void PlayBGMusic(string name,bool isLoop = true)
    {
        AudioClip audio = ResSvc.Instance.LoadAudio("ResAudio/"+name,true);
        if(bgAudio.clip == null || bgAudio.clip.name != audio.name)
        {
            bgAudio.clip = audio;
            bgAudio.loop = isLoop;
            bgAudio.Play();
        }
    }
    public void PlayUIAudio(string name)
    {
        AudioClip audio = ResSvc.Instance.LoadAudio("ResAudio/" + name, true);
        uiAudio.clip = audio;
        uiAudio.Play();
    }
}
