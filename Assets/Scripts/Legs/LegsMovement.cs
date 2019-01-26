using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegsMovement : MonoBehaviour
{
    Animation anim;
    public LeftLegMovement left_leg;
    public RightLegMovement right_leg;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (anim.isPlaying)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("Spinning");
            left_leg.move();
            right_leg.move();

        }
    }
}
