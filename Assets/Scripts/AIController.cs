using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    private Rigidbody rb;

    public float MaxSpeed = 2;
    public float MaxTurnSpeed = 45f;
    public float Aceleration = 1f;
    public float TurnAceleration = 20;

    private float CurrentSpeed = 0;
    private float CurrentTurn = 0;

    public AudioSource engineSound;
    public float maxVolume = 1.0f;

    public float intendedSpeed = 1;
    public float intendedTurn = 0;
    public Vector3 targetPosition = new Vector3(0, 0, 0);

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        engineSound.loop = true;
        engineSound.Play();
        targetPosition = GetRandomPosition();
    }

    void FixedUpdate()
    {
        DoAI();
        Move();
        Turn();
        return;
    }

    Vector3 GetRandomPosition()
    {
        System.Random random = new System.Random(System.DateTime.Now.Millisecond);
        Vector3 position = new Vector3(0, -50, 0);
        while (position.y <= 0)
        {
            Vector3 origin = new Vector3(random.Next(-100, 100), 50, random.Next(-100, 100));
            Ray ray = new Ray(origin, new Vector3(0, -1, 0));
            RaycastHit[] casts = Physics.RaycastAll(ray);
            position = casts[0].point;
        }
        return position;
    }

    public void KillAI()
    {
        HouseController houseController = gameObject.GetComponent<HouseController>();
        for(int i = 0; i < houseController.componentBlocks.Count; i++)
        {
            houseController.componentBlocks[i].transform.parent = null;
            houseController.componentBlocks[i].AddComponent<Rigidbody>();
        }
        houseController.EmptyGrid();

        Vector3 position = GetRandomPosition();
        transform.rotation = Quaternion.identity;
        transform.position = new Vector3(position.x, position.y + 1, position.z);
        //Destroy(gameObject);
    }

    private void DoAI()
    {
        if(transform.position.y < -2)
        {
            KillAI();
        }
        if (Vector3.Dot(transform.up, Vector3.down) > 0)
        {
            KillAI();
        }

        if (Vector3.Distance(transform.position, targetPosition) < 5f)
        {
            targetPosition = GetRandomPosition();
        }

        Vector3 relativeVector = transform.InverseTransformPoint(targetPosition);
        intendedTurn = relativeVector.x / relativeVector.magnitude;
    }

    private void Move()
    {


        if (intendedSpeed != 0)
        {
            CurrentSpeed += intendedSpeed * Aceleration * Time.deltaTime;
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
        if (intendedTurn != 0)
        {
            CurrentTurn += intendedTurn * TurnAceleration * Time.deltaTime;
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

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<HouseController>() != null)
        {
            if (collision.gameObject.GetComponent<HouseController>().GetScore() >= gameObject.GetComponent<HouseController>().GetScore())
            {
                KillAI();
            }
            else
            {
                if (collision.gameObject.GetComponent<AIController>() != null)
                {
                    collision.gameObject.GetComponent<AIController>().KillAI();
                }
                if(collision.gameObject.GetComponent<PlayerController>())
                {
                    Component.FindObjectOfType<GameController>().EndGame();
                    HouseController houseController = collision.gameObject.GetComponent<HouseController>();
                    for (int i = 0; i < houseController.componentBlocks.Count; i++)
                    {
                        houseController.componentBlocks[i].transform.parent = null;
                        houseController.componentBlocks[i].AddComponent<Rigidbody>();
                    }
                    houseController.EmptyGrid();
                }
            }
        }
    }
}
