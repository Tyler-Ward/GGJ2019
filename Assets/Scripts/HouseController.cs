using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseController : MonoBehaviour
{
    public GameObject MeshObj;
    public Collider collider;
    private Dictionary<Vector3Int, int> occupancyGrid = new Dictionary<Vector3Int, int>();
    //0 = empty space
    //1 = unoccupied adjacent cube
    //2 = contains part
    private List<Vector3Int> freeSpaces = new List<Vector3Int>();
    public List<GameObject> componentBlocks = new List<GameObject>();
    public ResourceManager resourceManager;

	public AudioSource pickupSound;

    public int score;
    public int locomotors;

    public void EmptyGrid()
    {
        occupancyGrid = new Dictionary<Vector3Int, int>();
        freeSpaces = new List<Vector3Int>();
        componentBlocks = new List<GameObject>();
        MakeDefaultHouse();
    }

    public int GetScore()
    {
        return score;
    }

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

    public void AddBlockToGrid(int x, int y, int z, bool[] adjacencies)
    {
        pickupSound.Play();

        if (GridContainsPart(x, y, z))
        {
            Debug.Log("overwriting part at " + new Vector3Int(x, y, z));
        }
        if (freeSpaces.Contains(new Vector3Int(x, y, z)))
        {
            freeSpaces.Remove(new Vector3Int(x, y, z));
        }
        occupancyGrid[new Vector3Int(x, y, z)] = 2;

        if(!GridContainsPart(x + 1, y, z) && adjacencies[2])
        {
            occupancyGrid[new Vector3Int(x + 1, y, z)] = 1;
            if(!freeSpaces.Contains(new Vector3Int(x + 1, y, z)))
            {
                freeSpaces.Add(new Vector3Int(x + 1, y, z));
            }
        }
        if (!GridContainsPart(x - 1, y, z) && adjacencies[3])
        {
            occupancyGrid[new Vector3Int(x - 1, y, z)] = 1;
            if (!freeSpaces.Contains(new Vector3Int(x - 1, y, z)))
            {
                freeSpaces.Add(new Vector3Int(x - 1, y, z));
            }
        }
        if (!GridContainsPart(x, y + 1, z) && adjacencies[0])
        {
            occupancyGrid[new Vector3Int(x, y + 1, z)] = 1;
            if (!freeSpaces.Contains(new Vector3Int(x, y + 1, z)))
            {
                freeSpaces.Add(new Vector3Int(x, y + 1, z));
            }
        }
        if (!GridContainsPart(x, y - 1, z) && y >= 1 && adjacencies[1])
        {
            occupancyGrid[new Vector3Int(x, y - 1, z)] = 1;
            if (!freeSpaces.Contains(new Vector3Int(x, y - 1, z)))
            {
                freeSpaces.Add(new Vector3Int(x, y - 1, z));
            }
        }
        if (!GridContainsPart(x, y, z + 1) && adjacencies[4])
        {
            occupancyGrid[new Vector3Int(x, y, z + 1)] = 1;
            if (!freeSpaces.Contains(new Vector3Int(x, y, z + 1)))
            {
                freeSpaces.Add(new Vector3Int(x, y, z + 1));
            }
        }
        if (!GridContainsPart(x, y, z - 1) && adjacencies[5])
        {
            occupancyGrid[new Vector3Int(x, y, z - 1)] = 1;
            if (!freeSpaces.Contains(new Vector3Int(x, y, z - 1)))
            {
                freeSpaces.Add(new Vector3Int(x, y, z - 1));
            }
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

    void MakeDefaultHouse()
    {
        bool[] bools = { false, false, true, true, true, true };
        AddBlockToGrid(0, 0, 0, bools);
        bool[] lbools = { false, false, false, false, false, false };
        AddBlockToGrid(0, -1, 0, lbools);
        score = 2;
        locomotors = 1;
        resourceManager.MaxFuel = 100;
    }

    void Start()
    {
        MakeDefaultHouse();
        //ShowFreeSpaces();
    }

    private void ShowFreeSpaces()
    {
        for (int i = 0; i < freeSpaces.Count; i++)
        {
            GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            obj.GetComponent<Renderer>().material.color = new Color(1, 0, 0, 0.25f);
            obj.GetComponent<Renderer>().material.shader = Shader.Find("Legacy Shaders/Transparent/Bumped Diffuse");
            obj.GetComponent<Collider>().enabled = false;
            obj.transform.parent = MeshObj.transform;
            obj.transform.localPosition = new Vector3(freeSpaces[i].x, freeSpaces[i].y + 0.5f, freeSpaces[i].z);
            obj.transform.localRotation = new Quaternion();
            Destroy(obj, 5);
        }
    }

    bool TestPartFit(Vector3Int position, float xsize, float ysize, float zsize)
    {
        for(int x = 0; x < xsize; x++)
        {
            for (int y = 0; y < ysize; y++)
            {
                for (int z = 0; z < zsize; z++)
                {
                    if(GridContainsPart(position.x + x, position.y + y, position.z + z))
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }

    private Vector3Int RandomlyFitPart(HousePart newBlock)
    {
        float xsize = newBlock.xsize;
        float ysize = newBlock.ysize;
        float zsize = newBlock.zsize;
        bool[] adjacencies = newBlock.faces;
        bool locomotion = newBlock.locomotion;

        List<Vector3Int> validPositions = new List<Vector3Int>();

        if (locomotion)
        {
            for (int x = -1; x <= 1; x++)
            {
                for (int z = -1; z <= 1; z++)
                {
                    if (!GridContainsPart(x, -1, z))
                    {
                        validPositions.Add(new Vector3Int(x, -1, z));
                    }
                }
            }
            TrackController track = newBlock.gameObject.GetComponent<TrackController>();
            track.Enable();
        }
        else
        {
            for (int i = 0; i < 6; i++)
            {
                if (adjacencies[i])
                {
                    for (int j = 0; j < freeSpaces.Count; j++)
                    {
                        switch (i)
                        {
                            case 0:
                                if (GridContainsPart(freeSpaces[j].x, freeSpaces[j].y + 1, freeSpaces[j].z))
                                {
                                    for (int x = 0; x < xsize; x++)
                                    {
                                        for (int z = 0; z < zsize; z++)
                                        {
                                            if (TestPartFit(new Vector3Int(freeSpaces[j].x - x, freeSpaces[j].y,
                                                freeSpaces[j].z - z), xsize, ysize, zsize))
                                            {
                                                validPositions.Add(new Vector3Int(freeSpaces[j].x - x, freeSpaces[j].y,
                                                freeSpaces[j].z - z));
                                            }
                                        }
                                    }
                                }
                                break;
                            case 1:
                                if (GridContainsPart(freeSpaces[j].x, freeSpaces[j].y - 1, freeSpaces[j].z))
                                {
                                    for (int x = 0; x < xsize; x++)
                                    {
                                        for (int z = 0; z < zsize; z++)
                                        {
                                            if (TestPartFit(new Vector3Int(freeSpaces[j].x - x, freeSpaces[j].y,
                                                freeSpaces[j].z - z), xsize, ysize, zsize))
                                            {
                                                validPositions.Add(new Vector3Int(freeSpaces[j].x - x, freeSpaces[j].y,
                                                freeSpaces[j].z - z));
                                            }
                                        }
                                    }
                                }
                                break;
                            case 2:
                                if (GridContainsPart(freeSpaces[j].x + 1, freeSpaces[j].y, freeSpaces[j].z))
                                {
                                    for (int y = 0; y < ysize; y++)
                                    {
                                        for (int z = 0; z < zsize; z++)
                                        {
                                            if (TestPartFit(new Vector3Int(freeSpaces[j].x, freeSpaces[j].y - y,
                                                freeSpaces[j].z - z), xsize, ysize, zsize))
                                            {
                                                validPositions.Add(new Vector3Int(freeSpaces[j].x, freeSpaces[j].y - y,
                                                freeSpaces[j].z - z));
                                            }
                                        }
                                    }
                                }
                                break;
                            case 3:
                                if (GridContainsPart(freeSpaces[j].x - 1, freeSpaces[j].y, freeSpaces[j].z))
                                {
                                    for (int y = 0; y < ysize; y++)
                                    {
                                        for (int z = 0; z < zsize; z++)
                                        {
                                            if (TestPartFit(new Vector3Int(freeSpaces[j].x, freeSpaces[j].y - y,
                                                freeSpaces[j].z - z), xsize, ysize, zsize))
                                            {
                                                validPositions.Add(new Vector3Int(freeSpaces[j].x, freeSpaces[j].y - y,
                                                freeSpaces[j].z - z));
                                            }
                                        }
                                    }
                                }
                                break;
                            case 4:
                                if (GridContainsPart(freeSpaces[j].x, freeSpaces[j].y, freeSpaces[j].z + 1))
                                {
                                    for (int x = 0; x < xsize; x++)
                                    {
                                        for (int y = 0; y < ysize; y++)
                                        {
                                            if (TestPartFit(new Vector3Int(freeSpaces[j].x - x, freeSpaces[j].y - y,
                                                freeSpaces[j].z), xsize, ysize, zsize))
                                            {
                                                validPositions.Add(new Vector3Int(freeSpaces[j].x - x, freeSpaces[j].y - y,
                                                freeSpaces[j].z));
                                            }
                                        }
                                    }
                                }
                                break;
                            case 5:
                                if (GridContainsPart(freeSpaces[j].x, freeSpaces[j].y, freeSpaces[j].z - 1))
                                {
                                    for (int x = 0; x < xsize; x++)
                                    {
                                        for (int y = 0; y < ysize; y++)
                                        {
                                            if (TestPartFit(new Vector3Int(freeSpaces[j].x - x, freeSpaces[j].y - y,
                                                freeSpaces[j].z), xsize, ysize, zsize))
                                            {
                                                validPositions.Add(new Vector3Int(freeSpaces[j].x - x, freeSpaces[j].y - y,
                                                freeSpaces[j].z));
                                            }
                                        }
                                    }
                                }
                                break;
                        }
                    }
                }
            }
        }

        if(validPositions.Count > 0)
        {
            score += newBlock.score;
            if (locomotion)
                locomotors += 1;
            resourceManager.MaxFuel += newBlock.storage;
            return validPositions[Random.Range(0, validPositions.Count)];
        }
        else
        {
            Debug.Log("No Valid Spaces");
            return new Vector3Int(0, 0, 0);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<HousePart>() != null)
        {
            if(collision.relativeVelocity.y >= 1 || collision.relativeVelocity.y <= -1)
            {
                Destroy(collision.gameObject);
                //Debug.Log("destroyed house part: " + collision.gameObject.name);
            }
            else
            {
                Vector3Int position = RandomlyFitPart(collision.gameObject.GetComponent<HousePart>());
                if(!(position == new Vector3Int(0, 0, 0)))
                {
                    collision.gameObject.transform.parent = MeshObj.transform;
                    collision.gameObject.transform.localPosition = new Vector3(position.x, position.y, position.z);
                    collision.gameObject.transform.localRotation = new Quaternion();
                    Destroy(collision.gameObject.GetComponent<Rigidbody>());
                    componentBlocks.Add(collision.gameObject);
                    AddBlockToGrid(position.x, position.y, position.z, collision.gameObject.GetComponent<HousePart>().faces);
                }
                else
                {
                    Destroy(collision.gameObject);
                }
                //Debug.Log("collected house part: " + collision.gameObject.name);
                //ShowFreeSpaces();
            }
        }
    }
}
