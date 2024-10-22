using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("FMOD Event")]
    public EventReference reverbTunnel;
    public EventReference reverbHouse;

    //FMOD Snapshot Instance
    public FMOD.Studio.EventInstance reverbTunnelIns;
    public FMOD.Studio.EventInstance reverbHouselIns;

    //FMOD SoundEffect
    public EventReference clickSoundEvent;
    public EventReference hoverSoundEvent;
    public EventReference swipsSoundEvent;

    void Start()
    {
        reverbTunnelIns = FMODUnity.RuntimeManager.CreateInstance(reverbTunnel);
        reverbHouselIns = FMODUnity.RuntimeManager.CreateInstance(reverbHouse);
    }
    public void ClickSound()
    {
        RuntimeManager.PlayOneShot(clickSoundEvent);
    }

    public void HoverSound()
    {
        RuntimeManager.PlayOneShot(hoverSoundEvent);
    }

    public void SwipsSound()
    {
        RuntimeManager.PlayOneShot(swipsSoundEvent);
    }

}
