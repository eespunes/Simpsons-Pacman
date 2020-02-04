using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Node : IComparable
{
    private bool corridor;
    private Vector3 coord;
    private int pos_x, pos_y;
    private bool walkable;
    private int h, g, f;
    private Node mother;
    private bool occupied;


    public Node(Vector3 c, int px, int py, bool walk, bool occupied)
    {
        coord = c;
        pos_x = px;
        pos_y = py;
        walkable = walk;
        h = 0;
        g = 0;
        f = 0;
        this.occupied = occupied;
        mother = null;
        corridor = false;
    }

    public int CompareTo(object obj)
    {
        Node n = (Node)obj;
        if (n == null)
            return 1;

        if (this.F < n.F)
            return -1;
        else
            if (this.H < n.H)
            return -1;
        else if (this.H == n.H)
            if (this.G < n.G)
                return -1;
            else if (this.G == n.G && this.F == n.F)
                return 0;
        return 1;
    }

    //Gets and setters
    public bool Walkable
    {
        get
        {
            return walkable;
        }

        set
        {
            walkable = value;
        }
    }

    public int Pos_x
    {
        get
        {
            return pos_x;
        }

        set
        {
            pos_x = value;
        }
    }

    public int Pos_y
    {
        get
        {
            return pos_y;
        }

        set
        {
            pos_y = value;
        }
    }

    public Vector3 Coord
    {
        get
        {
            return coord;
        }

        set
        {
            coord = value;
        }
    }



    public Node Mother
    {
        get
        {
            return mother;
        }

        set
        {
            mother = value;
        }
    }

    public int H
    {
        get
        {
            return h;
        }

        set
        {
            h = value;
        }
    }

    public int G
    {
        get
        {
            return g;
        }

        set
        {
            g = value;
        }
    }

    public int F
    {
        get
        {
            return f;
        }

        set
        {
            f = value;
        }
    }

    public bool Occupied
    {
        get
        {
            return occupied;
        }

        set
        {
            occupied = value;
        }
    }

    public bool Corridor
    {
        get
        {
            return corridor;
        }

        set
        {
            corridor = value;
        }
    }

}