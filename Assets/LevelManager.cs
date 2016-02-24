using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public List<Level> levelList = new List<Level>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public bool generate = false;
    public bool randomizeSteps = false;
    
    public List<Color> colors = new List<Color>();
    
    Transform controller;

    // Use this for initialization
    void Start()
    {
        GameObject temp = (GameObject)GameObject.Instantiate(new GameObject(), Vector3.zero, Quaternion.identity);
        controller = temp.transform;
        controller.position = Vector3.zero;

        nodeSets.Add(voidNodes);
        colors.Add(Color.white);
    }

    // Update is called once per frame
    void Update()
    {
        if (generate)
        {
            GenerateRoomNodes();
            generate = false;
        }

        if (randomizeSteps)
        {
            RandomizeStepCount();
            randomizeSteps = false;
        }
    }

    public Level GenerateLevel()
    {
        // generate the level, iterating through a sequence of rooms


        return null;
    }

    public List<List<Vector3>> nodeSets = new List<List<Vector3>>();
    List<Vector3> voidNodes = new List<Vector3>();

    public int stepMin = 0;
    public int stepMax = 10;

    public void RandomizeStepCount()
    {
        stepMin = Random.Range(100, 500);
        stepMax = Random.Range(stepMin + 200, stepMin * 2);
    }

    int gridMaxX;
    int gridMaxY;
    int gridMinX;
    int gridMinY;

    public void LayGridObject_old()
    {
        // make a grid before making room so rooms don't overlap
        // 100 by 100 tolerance

        bool collidedXmax = false;
        int xMaxTolerance = 50;
        bool collidedYmax = false;
        int yMaxTolerance = 50;
        bool collidedXmin = false;
        int xMinTolerance = 50;
        bool collidedYmin = false;
        int yMinTolerance = 50;

        Vector3 temp;
        
        temp = controller.position;


        colors.Add(Color.yellow);

        List<Vector3> tesGrid = new List<Vector3>();
        nodeSets.Add(tesGrid);
        for (int loop = 0; loop < 10; loop++)
        {
            Vector3 tempN = new Vector3(5, 0, loop);
            tesGrid.Add(tempN);
        }


        while (!collidedXmax && xMaxTolerance >= 0)
        {
            // keep moving right (x max) until collided with another tile
            foreach (List<Vector3> nodeSet in nodeSets)
            {
                foreach (Vector3 coord in nodeSet)
                {
                    if (Vector3.Distance(temp, coord) <= 1)
                    {
                        // collided, end here
                        collidedXmax = true;
                        break;
                    }
                }
            }

            temp.x += 1;
            xMaxTolerance--;
        }

        while (!collidedYmax && yMaxTolerance >= 0)
        {
            // keep moving right (x max) until collided with another tile
            foreach (List<Vector3> nodeSet in nodeSets)
            {
                foreach (Vector3 coord in nodeSet)
                {
                    if (Vector3.Distance(temp, coord) <= 1)
                    {
                        // collided, end here
                        collidedXmax = true;
                        break;
                    }
                }
            }

            temp.y += 1;
            yMaxTolerance--;
        }

        while (!collidedXmin && xMinTolerance >= 0)
        {
            // keep moving right (x max) until collided with another tile
            foreach (List<Vector3> nodeSet in nodeSets)
            {
                foreach (Vector3 coord in nodeSet)
                {
                    if (Vector3.Distance(temp, coord) <= 1)
                    {
                        // collided, end here
                        collidedXmax = true;
                        break;
                    }
                }
            }

            temp.x -= 1;
            xMinTolerance--;
        }

        while (!collidedYmin && yMinTolerance >= 0)
        {
            // keep moving right (x max) until collided with another tile
            foreach (List<Vector3> nodeSet in nodeSets)
            {
                foreach (Vector3 coord in nodeSet)
                {
                    if (Vector3.Distance(temp, coord) <= 1)
                    {
                        // collided, end here
                        collidedXmax = true;
                        break;
                    }
                }
            }

            temp.y -= 1;
            yMinTolerance--;
        }
    }

    public List<Vector3> measuringBuffer = new List<Vector3>();

    public void ClampRoomBorders(List<Vector3> room)
    {
        // just clamp each room after generated so other rooms don't overlap with it again

        measuringBuffer.Clear();

        foreach (Vector3 node in room)
        {
            measuringBuffer.Add(node);
        }

        float maxX = measuringBuffer.Max(node => node.x);
        float maxY = measuringBuffer.Max(node => node.z);
        float minX = measuringBuffer.Min(node => node.x);
        float minY = measuringBuffer.Min(node => node.z);

        // try to just fill in the clamped area with empty nodes
        for (int x = (int)minX; x <= (int)maxX; x++)
        {
            for (int y = (int)minY; y <= (int)maxY; y++)
            {
                voidNodes.Add(new Vector3(x, 0, y));
            }
        }
    }

    public void GenerateRoomNodes()
    {
        // generate a sequnce of nodes up to n-steps, hops over if overlap old node

        List<Vector3> nodeSet = new List<Vector3>();
        nodeSets.Add(nodeSet);

        Color col = new Color(Random.Range(0, 255) / (float)255, Random.Range(0, 255) / (float)255, Random.Range(0, 255) / (float)255);
        colors.Add(col);

        // make the room, randomizing tiles per 1000 or so steps

        for (int i = 0; i <= Random.Range(100, 500); i++)
        {
            bool validMove = false;
            bool isHopping = false;
            int moveTries = 0;

            ////////// try to account for other room

            while(!validMove)
            {
                moveTries++;
                if (moveTries >= 10)
                {
                    // all passages blocked, try to hop over
                    
                    controller.Translate(Vector3.forward);
                    isHopping = true;

                    while (isHopping)
                    {
                        controller.Translate(Vector3.forward);
                        foreach (List<Vector3> set in nodeSets)
                        {
                            foreach (Vector3 coords in set)
                            {
                                if (Vector3.Distance(controller.position, coords) <= 0.1f)
                                {
                                    isHopping = true;
                                    break;
                                }
                                else
                                {
                                    isHopping = false;
                                    break;
                                    // successfully hopped over
                                }
                            }
                        }
                    }
                    break;
                }

                // if trying to hop, don't turn. EDIT actually need to turn in case hop over to other room
                if (!isHopping)
                {

                    // calculate chance of turning
                    int turn = Random.Range(0, 3);

                    switch (turn)
                    {
                        case 1:
                            controller.Rotate(0, 90, 0);
                            break;
                        case 2:
                            controller.Rotate(0, -90, 0);
                            break;
                        default:
                            break;
                    }
                }

                // move the controller forward in new facing
                controller.Translate(Vector3.forward);

                // if first move, always valid
                if (i == 0)
                {
                    validMove = true;
                    continue;
                }

                // if already got node, move back and repeat
                foreach (List<Vector3> set in nodeSets)
                {
                    foreach (Vector3 coords in set)
                    {
                        if (Vector3.Distance(controller.position, coords) <= 0.1f)
                        {
                            controller.Translate(Vector3.back);
                            validMove = false;
                            break;
                        }
                        else
                        {
                            validMove = true;
                        }
                    }
                }
            }

            nodeSet.Add(controller.position);
        }

        ClampRoomBorders(nodeSet);
    }

    void OnDrawGizmos()
    {
        int count = 0;
        foreach (List<Vector3> nodeSet in nodeSets)
        {
            Gizmos.color = colors[count];

            foreach (Vector3 coord in nodeSet)
            {
                if (count == 0) Gizmos.DrawSphere(coord, .5f);
                else Gizmos.DrawCube(coord, Vector3.one);
            }

            count++;
        }
    }
}

public class Level : ScriptableObject
{
    // each level contains many rooms
    public List<Room> roomList = new List<Room>();
    
    public Room GenerateRoom()
    {
        // new room object
        Room rm = CreateInstance<Room>();

        GameObject temp = (GameObject)GameObject.Instantiate(new GameObject(), Vector3.zero, Quaternion.identity);
        Transform controller = temp.transform;

        // make the room, randomizing tiles per 1000 or so steps

        for (int i = 0; i<= Random.Range(1000,1500); i++)
        {
            // calculate chance of turning
            int turn = Random.Range(0, 3);

            switch (turn)
            {
                case 1:
                    controller.Rotate(0, 90, 0);
                    break;
                case 2:
                    controller.Rotate(0, -90, 0);
                    break;
                default:
                    break;
            }

            // move the controller forward in new facing
            controller.Translate(Vector3.forward);
            Debug.Log(controller.position);

            rm.grid.Add(controller.position);
        }

        return rm;
    }
}

public class Room : ScriptableObject
{
    // a room with a set number of organized tiles
    public RoomManager RoomManager;
    public List<Vector3> grid = new List<Vector3>();
}

public class RoomManager : ScriptableObject
{
    // contains the state of a room
}

public class Tile : ScriptableObject
{
    // each tile in the room contains various information
    private int[] tileIndex = new int[2];
}