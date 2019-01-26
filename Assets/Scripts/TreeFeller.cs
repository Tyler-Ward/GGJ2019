﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeFeller : MonoBehaviour
{
    public AudioSource sound;

    private void OnCollisionEnter(Collision collision)
    { 
        bool firstHit = gameObject.GetComponent<Rigidbody>() == null;
        bool hitByPlayer = collision.gameObject.GetComponent<PlayerController>() != null;

        if (firstHit) gameObject.AddComponent<Rigidbody>();

        if (hitByPlayer)
        {
            sound.volume = firstHit ?  1.0f : 0.2f;
        } else
        {
            sound.volume = 0.05f;
        }
        sound.Play();
    }
}
