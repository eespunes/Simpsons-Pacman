using Boo.Lang;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    private Node[,] theNodes;
    public int sizeX, sizeY;
    public LayerMask notWalkable;
    public GameObject donut, beer, extras;
    public GameObject red, blue, yellow, pink, ghosts, pac;
    private System.Collections.Generic.List<Node> notOccupied;
    public Vector2 corridorEIn, corridorEFin, corridorDIn, corridorDFin;
    public Vector2 red_pos, yellow_pos, blue_pos, pink_pos, pac_pos;

    void Awake()
    {
        Vector3 position;
        bool walk, occupied;
        int i = 1;
        int max = Random.Range(30, 50);
        int middleX = sizeX / 2, middleY = sizeY / 2;
        GameObject go;
        theNodes = new Node[sizeX, sizeY];
        notOccupied = new System.Collections.Generic.List<Node>();
        for (int x = 0; x < sizeX; x++)
            for (int y = 0; y < sizeY; y++)
            {
                position = new Vector3((transform.position.x + (x - middleX)), 0, (transform.position.z + (y - middleY)));
                if (Physics.CheckSphere(position, 0.5f, notWalkable))
                {
                    walk = false;
                    occupied = false;
                }
                else
                {
                    if (i < max)
                    {
                        go = Instantiate(donut, position, Quaternion.Euler(90, 0, 0));
                        go.transform.parent = extras.transform;
                        go.GetComponent<Donuts>().AddPosition(x, y);
                    }
                    else
                    {
                        i = 0;
                        go = Instantiate(beer, position, Quaternion.Euler(90, 0, 0));
                        go.transform.parent = extras.transform;
                        go.GetComponent<Donuts>().AddPosition(x, y);
                    }
                    occupied = true;
                    walk = true;
                    i++;
                }
                theNodes[x, y] = new Node(position, x, y, walk, occupied);
                if ((x <= corridorEIn.x && x >= corridorEFin.x && y == corridorEFin.y) || (x >= corridorDIn.x && x <= corridorDFin.x && y == corridorDFin.y))
                    theNodes[x, y].Corridor = true;
            }
        Instantiate(pac, theNodes[(int)pac_pos.x, (int)pac_pos.y].Coord, Quaternion.Euler(90, 0, 0)).name = "Homer";
        CreateGhosts();
    }
    public void CreateGhosts()
    {
        Instantiate(red, theNodes[(int)red_pos.x, (int)red_pos.y].Coord, Quaternion.Euler(90, 0, 0)).transform.parent = ghosts.transform;
        Instantiate(blue, theNodes[(int)blue_pos.x, (int)blue_pos.y].Coord, Quaternion.Euler(90, 0, 0)).transform.parent = ghosts.transform;
        Instantiate(yellow, theNodes[(int)yellow_pos.x, (int)yellow_pos.y].Coord, Quaternion.Euler(90, 0, 0)).transform.parent = ghosts.transform;
        Instantiate(pink, theNodes[(int)pink_pos.x, (int)pink_pos.y].Coord, Quaternion.Euler(90, 0, 0)).transform.parent = ghosts.transform;
    }

    private void OnDrawGizmos()
    {
        int middleX = sizeX / 2, middleY = sizeY / 2;
        for (int x = 0; x < sizeX; x++)
            for (int y = 0; y < sizeY; y++)
            {
                if (Physics.CheckSphere(new Vector3((transform.position.x + (x - middleX)), 0, (transform.position.z + (y - middleY))), 0.5f, notWalkable))
                    Gizmos.color = Color.red;
                else
                    Gizmos.color = Color.white;
                Gizmos.DrawWireCube(new Vector3((transform.position.x + (x - middleX)), 0, (transform.position.z + (y - middleY))), Vector3.one);
            }
    }
    public Node[,] TheNodes
    {
        get
        {
            return theNodes;
        }

        set
        {
            theNodes = value;
        }
    }
    public System.Collections.Generic.List<Node> NotOccupied
    {
        get
        {
            return notOccupied;
        }
        set
        {
            notOccupied = value;
        }
    }

    public void AddNotOccupied(int x, int y)
    {
        theNodes[x, y].Occupied = false;
        notOccupied.Add(theNodes[x, y]);
    }
    public Node Find(int x, int y)
    {
        return TheNodes[x, y];
    }
}