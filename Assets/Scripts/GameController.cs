﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject Menu;
    public ResourceManager resourceManager;
    public GameObject Player;
    public Vector3 startPosition;
    public bool GameRunning;

    public void StartGame ()
    {
        Menu.SetActive(false);
        resourceManager.Fuel = 75;
        GameRunning = true;
        Player.transform.position = startPosition;
        Player.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }

    public void EndGame()
    {
        Debug.Log("END GAME");
        Menu.SetActive(true);
        GameRunning = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameRunning)
        {
            resourceManager.Fuel -= 0.025f;
            if (resourceManager.Fuel <= 0)
            {
                EndGame();
            }

            Debug.Log(Vector3.Dot(Player.transform.up, Vector3.down));
            if(Vector3.Dot(Player.transform.up, Vector3.down) > 0)
            {
                EndGame();
            }
            

            if(Player.transform.position.y<-2)
            {
                EndGame();
            }
        }
    }
}
