using FMODUnity;
using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RadioController : MonoBehaviour, Interactable
{
    public EventReference inputsound;
    FMOD.Studio.EventInstance radioEvent;
    public ParticleSystem radio1;
    public bool playRadio;

    void Start()
    {
       
        radioEvent = FMODUnity.RuntimeManager.CreateInstance(inputsound);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(radioEvent, GetComponent<Transform>());
       

        if (playRadio)
        {
            radioEvent.start();
            radio1.Play();
            
        }
    }
    //Radio
    public void Radio1_Demo_Play()
    {
        
        if (!radio1.isPlaying)
        {
            radioEvent.start();
            radio1.Play();
            
        }
        else
        {
            radioEvent.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            radio1.Stop();
        }
        
    }

    
}
