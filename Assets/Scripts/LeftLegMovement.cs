using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftLegMovement : MonoBehaviour
{
    private Animation anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void move()
    {
        //Debug.Log("Spinning");
        //anim.Play("ArmatureAction_001");
        GetComponent<Animation>().Play("ArmatureAction_001");   
    }
}
