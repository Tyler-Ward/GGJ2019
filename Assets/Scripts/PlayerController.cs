using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    public ResourceManager resources;

    public float MaxSpeed = 2;
    public float MaxTurnSpeed = 45f;
    public float Aceleration = 1f;
    public float TurnAceleration = 20;

    private float CurrentSpeed = 0;
    private float CurrentTurn = 0;

    public AudioSource engineSound;
    public float maxVolume = 1.0f;
    public Camera liveCam;
    public Camera deathCam;
    public GameObject deathCamMount;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        engineSound.loop = true;
        engineSound.Play();
    }

    void FixedUpdate()
    {
        Move();
        Turn();
        deathCamMount.transform.eulerAngles = new Vector3(0, 0, 0);
        return;

    }

    public void onDeath()
    {
        deathCam.enabled = true;
        liveCam.enabled = false;
    }

    public void onLive()
    {
        deathCam.enabled = false;
        liveCam.enabled = true;
    } 

    private void Move()
    {
        MaxSpeed = (5f * gameObject.GetComponent<HouseController>().locomotors) / gameObject.GetComponent<HouseController>().score;

        if (resources.Fuel > 0 && Input.GetAxis("Vertical")!=0)
        {
            CurrentSpeed += Input.GetAxis("Vertical")* Aceleration* Time.deltaTime;
        }
        else
        {
            if (CurrentSpeed > 0)
            {
                CurrentSpeed -= (Aceleration * Time.deltaTime / 2);
                if (CurrentSpeed < 0)
                    CurrentSpeed = 0;
            }
            else if (CurrentSpeed < 0)
            {
                CurrentSpeed += (Aceleration * Time.deltaTime / 2);
                if (CurrentSpeed > 0)
                    CurrentSpeed = 0;
            }
        }

        if (CurrentSpeed > MaxSpeed)
            CurrentSpeed = MaxSpeed;
        if (CurrentSpeed < -MaxSpeed)
            CurrentSpeed = -MaxSpeed;

        engineSound.volume = Mathf.Abs(CurrentSpeed / MaxSpeed) * maxVolume;

        Vector3 movement = transform.forward * CurrentSpeed * Time.deltaTime;
        rb.MovePosition(rb.position + movement);
    }

    private void Turn()
    {
        if (resources.Fuel > 0 && Input.GetAxis("Horizontal") != 0)
        {
            CurrentTurn += Input.GetAxis("Horizontal") * TurnAceleration * Time.deltaTime;
        }
        else
        {
            if (CurrentTurn > 0)
            {
                CurrentTurn -= (TurnAceleration * Time.deltaTime / 1.5f);
                if (CurrentTurn < 0)
                    CurrentTurn = 0;
            }
            else if (CurrentTurn < 0)
            {
                CurrentTurn += (TurnAceleration * Time.deltaTime / 1.5f);
                if (CurrentTurn > 0)
                    CurrentTurn = 0;
            }
        }

        if (CurrentTurn > MaxTurnSpeed)
            CurrentTurn = MaxTurnSpeed;
        if (CurrentTurn < -MaxTurnSpeed)
            CurrentTurn = -MaxTurnSpeed;



        // Determine the number of degrees to be turned based on the input, speed and time between frames.
        float turn = CurrentTurn * Time.deltaTime;

        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        rb.MoveRotation(rb.rotation * turnRotation);
    }
}
