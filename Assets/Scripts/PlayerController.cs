using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float moveVertical = Input.GetAxis("Vertical");
        float moveHorizontal = Input.GetAxis("Horizontal");

        Vector3 acceleration = new Vector3(0.0f, 0.0f, moveVertical);
        Vector3 rotation = new Vector3(0.0f, moveHorizontal, 0.0f);

        rb.AddRelativeForce(acceleration * 10);
        rb.AddTorque(rotation * 9);
    }
}
