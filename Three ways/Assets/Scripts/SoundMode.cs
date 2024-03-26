using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.UI;

public class SoundMode : MonoBehaviour
{
    private bool _soundActive;
    public Image soundOn;
    public Image soundOff;
    public Image sound;

    private void Start()
    {
        _soundActive = false;
        ReadSound();
    }

    private void TurnOn()
    {
        if(sound != null ) sound.sprite = soundOn.sprite;
        _soundActive = true;
        AudioListener.pause = false;
    }

    private void TurnOff()
    {
        if(sound != null ) sound.sprite = soundOff.sprite;
        _soundActive = false;
        AudioListener.pause = true;
    }

    private void ReadSound()
    {
        bool play = Convert.ToBoolean(PlayerPrefs.GetString("sound", "true"));

        if (play) TurnOn();
        else TurnOff();
    }

    private void WriteSound()
    {
        PlayerPrefs.SetString("sound", _soundActive.ToString());
    }
    public void EditSound()
    {
        if(_soundActive)
        {
            TurnOff();
        }
        else
        {
            TurnOn();
        }
        WriteSound();
    }
}
