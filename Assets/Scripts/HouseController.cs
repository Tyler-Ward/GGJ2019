using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseController : MonoBehaviour
{
    public GameObject MeshObj;
    private Dictionary<Vector3Int, int> occupancyGrid = new Dictionary<Vector3Int, int>();
    //0 = empty space
    //1 = unoccupied adjacent cube
    //2 = contains part

    public bool GridContainsPart(int x, int y, int z)
    {
        Vector3Int key = new Vector3Int(x, y, z);
        if(occupancyGrid.ContainsKey(key))
        {
            if (occupancyGrid[key] == 1)
            {
                //Debug.Log("grid " + key + " contains air adjacent to part");
                return false;
            }
            else if (occupancyGrid[key] == 1)
            {
                //Debug.Log("grid " + key + " contains air");
                return false;
            }
            else
            {
                //Debug.Log("grid " + key + " contains part");
                return true;
            }
        }
        else
        {
            //Debug.Log("grid " + key + " contains null");
            return false;
        }
    }

    public void AddBlockToGrid(int x, int y, int z)
    {
        if (GridContainsPart(x, y, z))
        {
            Debug.Log("overwriting part at " + new Vector3Int(x, y, z));
        }
        occupancyGrid[new Vector3Int(x, y, z)] = 2;

        if(!GridContainsPart(x + 1, y, z))
        {
            occupancyGrid[new Vector3Int(x + 1, y, z)] = 1;
        }
        if (!GridContainsPart(x - 1, y, z))
        {
            occupancyGrid[new Vector3Int(x - 1, y, z)] = 1;
        }
        if (!GridContainsPart(x, y + 1, z))
        {
            occupancyGrid[new Vector3Int(x, y + 1, z)] = 1;
        }
        if (!GridContainsPart(x, y - 1, z))
        {
            occupancyGrid[new Vector3Int(x, y - 1, z)] = 1;
        }
        if (!GridContainsPart(x, y, z + 1))
        {
            occupancyGrid[new Vector3Int(x, y, z + 1)] = 1;
        }
        if (!GridContainsPart(x, y, z - 1))
        {
            occupancyGrid[new Vector3Int(x, y, z - 1)] = 1;
        }
    }

    public int GetOccupancyAt(int x, int y, int z)
    {
        Vector3Int key = new Vector3Int(x, y, z);
        if (occupancyGrid.ContainsKey(key))
        {
            return occupancyGrid[key];
        }
        else
        {
            return 0;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        for(int x = 0; x < 1; x++)
        {
            for (int y = 0; y < 1; y++)
            {
                for (int z = 0; z < 1; z++)
                {
                    AddBlockToGrid(x, y, z);
                }
            }
        }

        for (int x = -9; x < 10; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                for (int z = -9; z < 10; z++)
                {
                    int occupancy = GetOccupancyAt(x, y, z);
                    Debug.Log(new Vector3Int(x, y, z) + " " + occupancy);
                    if(occupancy == 1)
                    {
                        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        obj.GetComponent<Renderer>().material.color = new Color(1, 0, 0, 0.25f);
                        obj.GetComponent<Renderer>().material.shader = Shader.Find("Legacy Shaders/Transparent/Bumped Diffuse");
                        obj.GetComponent<Collider>().enabled = false;
                        obj.transform.parent = MeshObj.transform;
                        obj.transform.position = new Vector3(x, y+0.5f, z);
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
