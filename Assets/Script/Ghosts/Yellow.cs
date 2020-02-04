using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yellow : Ghost
{
    private void Start()
    {
        Initial();
        next = grid.Find((int)grid.yellow_pos.x, (int)grid.yellow_pos.y);
        initial = next;
        transform.position = next.Coord;
        InvokeRepeating("Change", 0, 5);
    }
    void Update()
    {
        if (gc.Start1)
        {
            if (inJail)
                Move();
            else
            {
                if (!gc.Eat || meatTime)
                {
                    NormalState();
                    if (path != null && path.Count > 0)
                        Move();
                    else
                        Change();
                }
                else
                {
                    Escape();
                    Move();
                }
            }
        }
    }

    public void Change()
    {
        if (!inJail)
        {
            Node posible = grid.Find(Random.Range(0, sizeX - 1), Random.Range(0, sizeY - 1));
            while (!posible.Walkable)
                posible = grid.Find(Random.Range(0, sizeX - 1), Random.Range(0, sizeY - 1));
            path = FindPath(next.Coord, posible.Coord);
            next = path[0];
            path.RemoveAt(0);
        }
    }
}
