using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct HousePartType
{
    public float xsize;
    public float ysize;
    public float zsize;
    public GameObject model;
    public bool[] faces;
}

public class HousePart : MonoBehaviour
{
    public float xpos;
    public float ypos;
    public float zpos;
    public HousePartType part;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
