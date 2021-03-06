﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackController : MonoBehaviour
{

    private Rigidbody rb;

    public TrailRenderer A;
    public TrailRenderer B;
    public ParticleSystem C;
    public Collider collisionBox;
    public bool startActive = false;

    private bool active = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if(startActive)
            active = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Enable()
    {
        active = true;
        StartCoroutine(startDebris());
    }

    private void OnCollisionEnter(Collision collision)
    {
        startDebris();
    }

    private IEnumerator startDebris()
    {
        if (active)
        {
            yield return new WaitForSeconds(1);
            //Debug.Log("Contact");
            A.emitting = true;
            B.emitting = true;
            ParticleSystem.EmissionModule smoke = C.emission;
            smoke.enabled = true;
        }
    }

    public void gapDebis()
    {
        A.emitting = false;
        B.emitting = false;
        ParticleSystem.EmissionModule smoke = C.emission;
        smoke.enabled = false;
        StartCoroutine(startDebris());
    }
}
