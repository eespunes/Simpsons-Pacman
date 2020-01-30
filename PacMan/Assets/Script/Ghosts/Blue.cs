using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blue : Ghost
{
    int nH, cH, h;
    int distance;

    private void Start()
    {

        distance = 5;
        Initial();
        next = grid.Find((int)grid.blue_pos.x, (int)grid.blue_pos.y);
        transform.position = next.Coord;
        initial = next;
        end = next.Coord;
        InvokeRepeating("Change", time + 2, 5);
        InvokeRepeating("IsNear", time + 2, 1);
    }

    void Update()
    {
        if (gc.Start1)
        {
            if (!gc.Eat || meatTime)
            {
                NormalState();
                if (near && end != pac.Next.Coord && !inJail)
                {
                    end = pac.Next.Coord;
                    path = FindPath(next.Coord, end);
                    next = path[0];
                    path.RemoveAt(0);
                }

                if (path != null && path.Count > 0)
                    Move();
                else
                {
                    Move();
                    Change();
                }
            }
            else
            {
                Escape();
                Move();
            }
        }
    }

    public void Change()
    {
        if (!near && !inJail)
        {
            Node posible = grid.Find(Random.Range(0, sizeX - 1), Random.Range(0, sizeY - 1));
            while (!posible.Walkable)
                posible = grid.Find(Random.Range(0, sizeX - 1), Random.Range(0, sizeY - 1));
            end = posible.Coord;
            path = FindPath(next.Coord, end);
            next = path[0];
            path.RemoveAt(0);
        }
    }

    public void IsNear()
    {
        nH = Mathf.Abs(pac.Next.Pos_x - next.Pos_x) + Mathf.Abs(pac.Next.Pos_y - next.Pos_y);
        if (next.Pos_x > sizeX / 2)
            cH = Mathf.Abs((int)corridorD.x - next.Pos_x) + Mathf.Abs((int)corridorD.y - next.Pos_y) + Mathf.Abs(pac.Next.Pos_x - (int)corridorE.x) + Mathf.Abs(pac.Next.Pos_y - (int)corridorE.y);
        else
            cH = Mathf.Abs((int)corridorE.x - next.Pos_x) + Mathf.Abs((int)corridorE.y - next.Pos_y) + Mathf.Abs(pac.Next.Pos_x - (int)corridorD.x) + Mathf.Abs(pac.Next.Pos_y - (int)corridorD.y);
        if (nH < cH)
            h = nH;
        else
            h = cH;
        if (h <= distance)
            near = true;
        else
            near = false;
    }

}
