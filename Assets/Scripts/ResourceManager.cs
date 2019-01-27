using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour
{
	public float Fuel;
	public Slider FuelSlider;

    public float MaxFuel;


    // Start is called before the first frame update
    void Start()
    {
        MaxFuel = 100;
    }
  

    // Update is called once per frame
    void Update()
    {
        if (Fuel > MaxFuel)
            Fuel = MaxFuel;
        FuelSlider.value = Fuel;
        FuelSlider.maxValue = MaxFuel;
    }
}
