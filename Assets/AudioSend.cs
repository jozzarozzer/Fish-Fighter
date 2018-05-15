﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSend : MonoBehaviour
{
    public AudioClip clip;
    public GameObjectVariable AudioSpawner;

    public void SendAudio()
    {
        AudioSpawner spawnerScript = AudioSpawner.value.GetComponent<AudioSpawner>();
        spawnerScript.PlayClip(clip);
    }

}