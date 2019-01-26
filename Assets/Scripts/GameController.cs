using System.Collections;
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameRunning)
        {
            resourceManager.Fuel -= 0.01f;
            if (resourceManager.Fuel <= 0)
            {
                Debug.Log("END GAME");
                Menu.SetActive(true);
                GameRunning = false;
            }
        }
    }
}
