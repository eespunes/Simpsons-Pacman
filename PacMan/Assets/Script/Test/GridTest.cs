using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTest : MonoBehaviour
{

    private Node[,] theNodes;
    private GameObject[,] cubes;
    public int sizeX, sizeY;
    public LayerMask notWalkable;
    public GameObject beer;
    private Vector3 start, end;
    private List<Node> l;
    private Node corridorE, corridorD;

    void Awake()
    {
        Vector3 position;
        bool walk, occupied;
        start = Vector3.zero;
        end = Vector3.zero;
        int middleX = sizeX / 2, middleY = sizeY / 2;
        theNodes = new Node[sizeX, sizeY];
        cubes = new GameObject[sizeX, sizeY];

        for (int x = 0; x < sizeX; x++)
            for (int y = 0; y < sizeY; y++)
            {
                position = new Vector3((transform.position.x + (x - middleX)), 0, (transform.position.z + (y - middleY)));
                if (Physics.CheckSphere(position, 0.1f, notWalkable))
                {
                    walk = false;
                    occupied = false;
                }
                else
                {
                    GameObject go = Instantiate(beer, position, Quaternion.identity);
                    cubes[x, y] = go;
                    go.name = x + "," + y;
                    occupied = true;
                    walk = true;
                }
                theNodes[x, y] = new Node(position, x, y, walk, occupied);
            }
        corridorE = theNodes[0, 11];
        corridorD = theNodes[18, 11];
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {

            l = FindPath(start, end);
            foreach (Node n in l)
            {
                cubes[n.Pos_x, n.Pos_y].GetComponent<MeshRenderer>().material.color = Color.blue;
            }
            print(l.Count);
            start = Vector3.zero;
            end = Vector3.zero;
        }
    }
    public void SetStart(Vector3 t)
    {
        if (l != null && start == Vector3.zero && end == Vector3.zero)
            foreach (Node n in l)
            {
                cubes[n.Pos_x, n.Pos_y].GetComponent<MeshRenderer>().material.color = Color.white;
            }
        if (start == Vector3.zero)
            start = t;
        else if (end == Vector3.zero)
            end = t;
    }
    private void OnDrawGizmos()
    {
        int middleX = sizeX / 2, middleY = sizeY / 2;
        for (int x = 0; x < sizeX; x++)
            for (int y = 0; y < sizeY; y++)
            {
                if (Physics.CheckSphere(new Vector3((transform.position.x + (x - middleX)), 0, (transform.position.z + (y - middleY))), 0.1f, notWalkable))
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
    }
    public Node Find(int x, int y)
    {
        return TheNodes[x, y];
    }
    public System.Collections.Generic.List<Node> FindPath(Vector3 startPos, Vector3 targetPos)
    {
        // Get the start and ending nodes
        Node s = null, e = null, neighbour;
        int g, h, corridorG, normalG, corridorH, normalH;

        for (int x = 0; x < sizeX; x++)
            for (int y = 0; y < sizeY; y++)
            {
                if (s == null)
                    if (targetPos == theNodes[x, y].Coord)
                        s = theNodes[x, y];
                if (e == null)
                    if (startPos == theNodes[x, y].Coord)
                        e = theNodes[x, y];
                if (s != null && e != null)
                    break;
            }
        // Create OPEN and CLOSE lists
        ArrayList open, close;
        open = new ArrayList();
        close = new ArrayList();
        // Add the start node to OPEN
        open.Add(s);
        // While we have nodes in the OPEN list
        while (open.Count != 0)
        {
            // Get the node in the OPEN list with the lowest F cost or 
            // with the lowest F but Lower H (distance to end node)
            Node actual = (Node)open[0];
            //print("ACTUAL: " + actual.Pos_x + "," + actual.Pos_y + "= G:" + actual.G + ", H:" + actual.H + ", F:" + actual.F);
            // Change that node from OPEN to CLOSE
            open.Remove(actual);
            close.Add(actual);
            // If it's the ending node, we're done: Get the total path found and finish
            if (actual == e)
            {
                return CalcularLlista(actual, close.Count);
            }
            else
            {
                for (int x = actual.Pos_x - 1; x <= actual.Pos_x + 1; x++)
                {
                    for (int y = actual.Pos_y - 1; y <= actual.Pos_y + 1; y++)
                    {
                        if (actual.Pos_x == x || actual.Pos_y == y)
                        {
                            if (actual.Pos_x == x && actual.Pos_y == y)
                                continue;

                            if (x < 0)
                                neighbour = theNodes[sizeX - 1, y];
                            else if (y < 0)
                                neighbour = theNodes[x, sizeY - 1];
                            else if (x >= sizeX)
                                neighbour = theNodes[0, y];
                            else if (y >= sizeY)
                                neighbour = theNodes[x, 0];
                            else
                                neighbour = theNodes[x, y];

                            // If that neighbour is not walkable or it's closed, 
                            // skip that neighbour and get next
                            if (!neighbour.Walkable || close.Contains(neighbour))
                                continue;
                            // If we haven't visited that node yet, or this node new G cost is shorter than before
                            if (!open.Contains(neighbour))
                            {

                                // Set its G,H & F costs and set the actual node as the parent of this neighbour

                                normalG = Mathf.Abs(s.Pos_x - neighbour.Pos_x) + Mathf.Abs(s.Pos_y - neighbour.Pos_y);
                                normalH = Mathf.Abs(e.Pos_x - neighbour.Pos_x) + Mathf.Abs(e.Pos_y - neighbour.Pos_y);

                                if (neighbour.Pos_x > sizeX / 2)
                                {
                                    corridorG = Mathf.Abs((int)corridorD.Pos_x - neighbour.Pos_x) + Mathf.Abs(corridorD.Pos_y - neighbour.Pos_y) + Mathf.Abs(s.Pos_x - corridorE.Pos_x) + Mathf.Abs(s.Pos_y - (int)corridorE.Pos_y);
                                    corridorH = Mathf.Abs((int)corridorD.Pos_x - neighbour.Pos_x) + Mathf.Abs((int)corridorD.Pos_y - neighbour.Pos_y) + Mathf.Abs(e.Pos_x - (int)corridorE.Pos_x) + Mathf.Abs(e.Pos_y - (int)corridorE.Pos_y);
                                }
                                else
                                {
                                    corridorG = Mathf.Abs((int)corridorE.Pos_x - neighbour.Pos_x) + Mathf.Abs(corridorE.Pos_y - neighbour.Pos_y) + Mathf.Abs(s.Pos_x - corridorD.Pos_x) + Mathf.Abs(s.Pos_y - (int)corridorD.Pos_y);
                                    corridorH = Mathf.Abs((int)corridorE.Pos_x - neighbour.Pos_x) + Mathf.Abs((int)corridorE.Pos_y - neighbour.Pos_y) + Mathf.Abs(e.Pos_x - (int)corridorD.Pos_x) + Mathf.Abs(e.Pos_y - (int)corridorD.Pos_y);
                                }
                                if (normalG < corridorG)
                                    g = normalG;
                                else
                                    g = corridorG;
                                if (normalH < corridorH)
                                    h = normalH;
                                else
                                    h = corridorH;

                                neighbour.G = g;
                                neighbour.H = h;
                                neighbour.F = g + h;

                                neighbour.Mother = actual;
                                // if the neighbour it's not on the OPEN list, add it.
                                if (!open.Contains(neighbour))
                                {
                                    open.Add(neighbour);
                                }
                            }
                        }
                    }
                }
            }
        }
        return null;
    }
    public List<Node> CalcularLlista(Node actual, int length)
    {
        List<Node> path = new List<Node>
        {
            actual
        };
        Node n = actual;
        int i = 0;
        while (n.Mother != null && i <= length)
        {
            n = n.Mother;
            path.Add(n);
            i++;
        }
        return path;
    }

}

