using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeFeller : MonoBehaviour
{
    public int fuelAmount;
    public AudioSource sound;
    public ParticleSystem hitdebris;

    private void OnCollisionEnter(Collision collision)
    { 
        bool firstHit = gameObject.GetComponent<Rigidbody>() == null;
        bool hitByPlayer = collision.gameObject.GetComponent<PlayerController>() != null;

        if (firstHit)
        {
            gameObject.AddComponent<Rigidbody>();
            Destroy(gameObject, 0.15f);
            hitdebris.Play(true);
        }

        if (hitByPlayer)
        {
            if (firstHit)
            {
                GameObject.Find("ResourceController").GetComponent<ResourceManager>().Fuel += fuelAmount;
            }
            sound.volume = firstHit ?  1.0f : 0.2f;
        } else
        {
            sound.volume = 0.05f;
        }
        sound.Play();
    }
}
