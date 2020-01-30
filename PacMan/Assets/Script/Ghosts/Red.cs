using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Red : Ghost
{
    protected void Start()
    {
        Initial();
        next = grid.Find((int)grid.red_pos.x, (int)grid.red_pos.y);
        initial = next;
        transform.position = next.Coord;
    }

    void Update()
    {
        if (gc.Start1)
        {
            if (!gc.Eat || meatTime)
            {
                NormalState();
                if (end != pac.Next.Coord && !inJail)
                {
                    end = pac.Next.Coord;
                    path = FindPath(next.Coord, end);
                    next = path[0];
                    path.RemoveAt(0);
                }
            }
            else
                Escape();
            Move();
        }
    }
}
