using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioSettingsManager : MonoBehaviour
{

    public Scrollbar masterVolumeScrollbar;

    public Scrollbar musicVolumeScrollbar;
    public Scrollbar sfxVolumeScrollbar;
    public Scrollbar voiceVolumeScrollbar;
    public Scrollbar footstepVolumeScrollbar;

    private bool duringAwake = false;

    private void OnEnable()
    {
        duringAwake = true;

        masterVolumeScrollbar.value = AudioManager.masterVolumePlayerSetting;        

        musicVolumeScrollbar.value = AudioManager._MusicVolumePlayerSetting;        
        sfxVolumeScrollbar.value = AudioManager._SFXVolumePlayerSetting;        
        voiceVolumeScrollbar.value = AudioManager._VoiceVolumePlayerSetting;        
        footstepVolumeScrollbar.value = AudioManager._FootstepVolumePlayerSetting; 

        duringAwake = false;       
    }

    public void updateVolumes()
    {
        if(duringAwake)
        {
            return;
        }

        AudioManager.masterVolumePlayerSetting = masterVolumeScrollbar.value;

        AudioManager.musicVolumePlayerSetting = musicVolumeScrollbar.value;        
        AudioManager.sfxVolumePlayerSetting = sfxVolumeScrollbar.value;        
        AudioManager.voiceVolumePlayerSetting = voiceVolumeScrollbar.value;        
        AudioManager.footstepVolumePlayerSetting = footstepVolumeScrollbar.value;        
    }

}