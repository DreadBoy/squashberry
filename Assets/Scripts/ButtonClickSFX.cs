﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(AudioSource))]
public class ButtonClickSFX : MonoBehaviour
{
    private AudioSource SFX;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => { OnClick(); });
    }

    void Awake()
    {
        SFX = GetComponent<AudioSource>();
        if (SFX != null)
            SFX.playOnAwake = false;
    }


    void OnClick()
    {
        if (SFX == null)
            return;

        if (Settings.saveData.SFX)
            SFX.Play();
    }
}
