﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject Menu;
    public GameObject Menu2;
    public ResourceManager resourceManager;
    public GameObject Player;
    public Vector3 startPosition;
    public bool GameRunning;
    public HouseController houseController;
    public Text scorebox;

    public void StartGame ()
    {
        Menu.SetActive(false);
        Menu2.SetActive(false);
        resourceManager.Fuel = 75;
        GameRunning = true;
        Player.transform.position = startPosition;
        Player.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        PartSpawner[] partSpawners = gameObject.GetComponents<PartSpawner>();
        for (int i = 0; i < partSpawners.Length; i++)
        {
            partSpawners[i].DestroyObjects();
            partSpawners[i].SpawnObjects();
        }
    }

    public void EndGame()
    {
        Debug.Log("END GAME");
        Menu.SetActive(true);
        GameRunning = false;
        scorebox.text = "SCORE: " + houseController.GetScore();
    }

    public void EndGame2()
    {
        Debug.Log("END GAME 2");
        Menu2.SetActive(true);
        GameRunning = false;
        scorebox.text = "SCORE: " + houseController.GetScore();
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
            resourceManager.Fuel -= 0.05f;
            if (resourceManager.Fuel <= 0)
            {
                EndGame();
            }

            //Debug.Log(Vector3.Dot(Player.transform.up, Vector3.down));
            if(Vector3.Dot(Player.transform.up, Vector3.down) > 0)
            {
                EndGame();
            }
            

            if(Player.transform.position.y<-2)
            {
                EndGame2();
            }
        }
    }
}
