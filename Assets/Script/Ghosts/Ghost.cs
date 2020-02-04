using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour {
    protected int sizeX, sizeY;

    private float v, initialV;
    protected float time;

    protected bool firstScape,inJail,onlyOne;
    protected bool near, meatTime;

    protected Vector3 end;
    protected Vector2 corridorE, corridorD;

    protected Node[,] theNodes;
    protected Node next, initial;
    protected PacMan pac;
    protected GameController gc;
    protected Grid grid;

    protected List<Node> path;

    protected SpriteRenderer sr;
    protected Sprite s;
    public Sprite dead, eatable;

    protected void Initial()
    {
        onlyOne = false;
        meatTime = false;
        inJail = true;
        sr = GetComponent<SpriteRenderer>();
        s = sr.sprite;
        firstScape = true;
        gc = GameObject.Find("Game Controller").GetComponent<GameController>();
        grid = GameObject.Find("Grid").GetComponent<Grid>();
        pac = GameObject.Find("Homer").GetComponent<PacMan>();
        time = gc.initialSong.length;
        if (gameObject.name.Contains("Red"))
            time += 1;
        else if (gameObject.name.Contains("Blue"))
            time += 2;
        else if (gameObject.name.Contains("Yellow"))
            time += 3;
        else if (gameObject.name.Contains("Pink"))
            time += 4;
        theNodes = grid.TheNodes;
        sizeX = grid.sizeX;
        sizeY = grid.sizeY;
        corridorE = grid.corridorEFin;
        corridorD = grid.corridorDFin;
        v = gc.V;
        initialV = v;
        end = Vector3.zero;
        near = false;
        Invoke("ExitJail", time);
    }

    //To pass from his Node from the next node in the path
    protected void Move()
    {
        if (transform.position == next.Coord&&path!=null&&path.Count > 0&&!inJail)
        {
                next = path[0];
                path.RemoveAt(0);
            if (next.Corridor)
                v = initialV / 2;
            else if(!gc.Eat)
                v = initialV;
            if (next.Pos_x == 0)
                {
                    next = path[0];
                    path.RemoveAt(0);
                    transform.position = next.Coord;
                }
                else if (next.Pos_x == sizeX - 1)
                {
                    next = path[0];
                    path.RemoveAt(0);
                    transform.position = next.Coord;
                }
        }
        else
           transform.position = Vector3.MoveTowards(transform.position, next.Coord, v);
    }

    //When pacman can't eat this
    protected void NormalState()
    {
        if (onlyOne)
        {
            sr.sprite = s;
            v = initialV;
            onlyOne = false;
            firstScape = true;
        }
        if (gc.OtherTime || !gc.Eat)
        {
            meatTime = false;
        }
    }

    //When pacman can eat this
    protected void Escape()
    {
        if ((firstScape || end == next.Coord)&&!inJail)
        {
            onlyOne = true;
            near = false;
            if (firstScape)
            {
                v = initialV / 2;
                sr.sprite = eatable;
            }
            Node posible = null;
            if (pac.Next.Pos_x > sizeX / 2 && pac.Next.Pos_y > sizeY / 2)
            {
                posible = grid.Find(UnityEngine.Random.Range(0, (sizeX - 1) / 2), UnityEngine.Random.Range(0, (sizeY - 1) / 2));
                while (!posible.Walkable)
                    posible = grid.Find(UnityEngine.Random.Range(0, (sizeX - 1) / 2), UnityEngine.Random.Range(0, (sizeY - 1) / 2));
            }
            else if (pac.Next.Pos_x <= sizeX / 2 && pac.Next.Pos_y <= sizeY / 2)
            {
                posible = grid.Find(UnityEngine.Random.Range((sizeX - 1) / 2, sizeX - 1), UnityEngine.Random.Range((sizeY - 1) / 2, sizeY - 1));
                while (!posible.Walkable)
                    posible = grid.Find(UnityEngine.Random.Range((sizeX - 1) / 2, sizeX - 1), UnityEngine.Random.Range((sizeY - 1) / 2, sizeY - 1));
            }
            else if (pac.Next.Pos_x > sizeX / 2 && pac.Next.Pos_y <= sizeY / 2)
            {
                posible = grid.Find(UnityEngine.Random.Range(0, (sizeX - 1) / 2), UnityEngine.Random.Range((sizeY - 1) / 2, sizeY - 1));
                while (!posible.Walkable)
                    posible = grid.Find(UnityEngine.Random.Range(0, (sizeX - 1) / 2), UnityEngine.Random.Range((sizeY - 1) / 2, sizeY - 1));
            }
            else
            {
                posible = grid.Find(UnityEngine.Random.Range((sizeX - 1) / 2, sizeX - 1), UnityEngine.Random.Range(0, (sizeY - 1) / 2));
                while (!posible.Walkable)
                    posible = grid.Find(UnityEngine.Random.Range((sizeX - 1) / 2, sizeX - 1), UnityEngine.Random.Range(0, (sizeY - 1) / 2));
            }
            end = posible.Coord;
            path = FindPath(next.Coord, end);
            next = path[0];
            path.RemoveAt(0);
            firstScape = false;
        }

    }

    //When this is in jail
    protected void ExitJail()
    {
            sr.sprite = s;
            if (next.Pos_y == grid.red_pos.y)
            {
                next = grid.Find(initial.Pos_x, initial.Pos_y + 1);
                Invoke("RestartMovement", 1);
            }
            else
            {
                next = grid.Find(initial.Pos_x, initial.Pos_y + 2); ;
                Invoke("RestartMovement", 1);

            }
    }

    //When this has exit from jail
    public void RestartMovement()
    {
        path = null;
        inJail = false;
        meatTime = true;
    }

    //When Pacman Dies
    public void ResetPosition()
    {
        next = initial;
        inJail = true;
        transform.position = initial.Coord;
        Invoke("ExitJail", time);
    }

    private void OnTriggerEnter(Collider other)

    {
        if (other.tag == "Player")
        {
            if (gc.Eat && !meatTime)
            {
                sr.sprite = dead;
                inJail = true;
                next = initial;
                gc.AddNormalPoint(0, 0, 0);
                Invoke("ExitJail", 4f);
            }
            else 
               gc.LoseLife();
        }
    }

    protected List<Node> FindPath(Vector3 startPos, Vector3 targetPos)
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
                return GetPath(actual, close.Count);
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
                                    corridorG = Mathf.Abs((int)corridorD.x - neighbour.Pos_x) + Mathf.Abs((int)corridorD.y - neighbour.Pos_y) + Mathf.Abs(s.Pos_x - (int)corridorE.x) + Mathf.Abs(s.Pos_y - (int)corridorE.y);
                                    corridorH = Mathf.Abs((int)corridorD.x - neighbour.Pos_x) + Mathf.Abs((int)corridorD.y - neighbour.Pos_y) + Mathf.Abs(e.Pos_x - (int)corridorE.x) + Mathf.Abs(e.Pos_y - (int)corridorE.y);
                                }
                                else
                                {
                                    corridorG = Mathf.Abs((int)corridorE.x - neighbour.Pos_x) + Mathf.Abs((int)corridorE.y - neighbour.Pos_y) + Mathf.Abs(s.Pos_x - (int)corridorD.x) + Mathf.Abs(s.Pos_y - (int)corridorD.y);
                                    corridorH = Mathf.Abs((int)corridorE.x - neighbour.Pos_x) + Mathf.Abs((int)corridorE.y - neighbour.Pos_y) + Mathf.Abs(e.Pos_x - (int)corridorD.x) + Mathf.Abs(e.Pos_y - (int)corridorD.y);
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
    protected List<Node> GetPath(Node actual, int length)
    {
        List<Node> path = new List<Node>();
        path.Add(actual);
        Node n = actual;
        int i = 0;
        while (n.Mother != null && i <= length)
        {
            n = n.Mother;
            if(!path.Contains(n))
            path.Add(n);
            i++;
        }
        return path;
    }
}
