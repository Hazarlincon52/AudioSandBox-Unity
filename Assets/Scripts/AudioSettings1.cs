using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSettings1 : MonoBehaviour
{

    FMOD.Studio.Bus Master;
    FMOD.Studio.Bus SFX;
    FMOD.Studio.Bus Ambient;

    //private PlayerPrefs masterVolumeContainer;
    //private PlayerPrefs SFXVolumeContainer;
    //private PlayerPrefs ambientVolumeContainer;

    public float masterVolume = 0.5f;
    public float SFXVolume = 0.5f;
    public float ambientVolume = 0.5f;

    public Slider masterMenu;
    public Slider SFXMenu;
    public Slider ambientMenu;

    public Slider masterPause;
    public Slider SFXPause;
    public Slider ambientPause;

    private void Awake()
    {
        Master = FMODUnity.RuntimeManager.GetBus("bus:/Master");
        SFX = FMODUnity.RuntimeManager.GetBus("bus:/Master/SFX");
        Ambient = FMODUnity.RuntimeManager.GetBus("bus:/Master/Ambient");

        if (PlayerPrefs.GetFloat("masterVolumeContainer") == 0 && PlayerPrefs.GetFloat("SFXVolumeContainer") == 0 && PlayerPrefs.GetFloat("ambientVolumeContainer") == 0)
        {
            PlayerPrefs.SetFloat("masterVolumeContainer", masterVolume);
            PlayerPrefs.SetFloat("SFXVolumeContainer", SFXVolume);
            PlayerPrefs.SetFloat("ambientVolumeContainer", ambientVolume);
            Master.setVolume(masterVolume);
            SFX.setVolume(SFXVolume);
            Ambient.setVolume(ambientVolume);
        }
        else
        {
            masterVolume = PlayerPrefs.GetFloat("masterVolumeContainer");
            masterMenu.value = masterVolume;

            SFXVolume = PlayerPrefs.GetFloat("SFXVolumeContainer");
            SFXMenu.value = SFXVolume;

            ambientVolume = PlayerPrefs.GetFloat("ambientVolumeContainer");
            ambientMenu.value = ambientVolume;

            Master.setVolume(masterVolume);
            SFX.setVolume(SFXVolume);
            Ambient.setVolume(ambientVolume);
        }
        
    }

    public void MasterVolumeLevel(float newMasterVolume)
    {
        masterVolume = newMasterVolume;
        masterMenu.value = masterVolume;
        masterPause.value = masterVolume;
        PlayerPrefs.SetFloat("masterVolumeContainer", masterVolume);
        Master.setVolume(masterVolume);
    }
    public void SFXVolumeLevel(float newSFXVolume)
    {
        SFXVolume = newSFXVolume;
        SFXMenu.value = SFXVolume;
        SFXPause.value = SFXVolume;
        PlayerPrefs.SetFloat("SFXVolumeContainer", SFXVolume);
        SFX.setVolume(SFXVolume);
    }
    public void AmbientVolumeLevel(float newAmbientVolume)
    {
        ambientVolume = newAmbientVolume;
        ambientMenu.value = ambientVolume;
        ambientPause.value = ambientVolume;
        PlayerPrefs.SetFloat("ambientVolumeContainer", ambientVolume);
        Ambient.setVolume(ambientVolume);
    }

    public void stopAllEvent()
    {
        Master.stopAllEvents(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }
}
