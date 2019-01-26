using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HousePart : MonoBehaviour
{
    public float xsize = 1;
    public float ysize = 1;
    public float zsize = 1;
    public float xpos = 0;
    public float ypos = 0;
    public float zpos = 0;
    public bool[] faces = {false, true, false, false, false, false}; //up down left right front back
}
