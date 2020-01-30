using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pink : Ghost
{
    int distance;
    Vector3 pacPos;

    private void Start()
    {
        distance = 4;
        Initial();
        next = grid.Find((int)grid.pink_pos.x, (int)grid.pink_pos.y);
        initial = next;
        transform.position = next.Coord;
        pacPos = Vector3.zero;
        end = next.Coord;
    }

    void Update()
    {
        if (gc.Start1)
        {
            if ((!gc.Eat || meatTime))
            {
                NormalState();
                if (pacPos != pac.Next.Coord && !inJail)
                {
                    end = Change();
                    pacPos = pac.Next.Coord;
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

    Vector3 Change()
    {
        Node posible = theNodes[0, 0];
        int i = 1, x, y;
        while (!posible.Walkable)
        {
            x = pac.Next.Pos_x + distance - i;
            y = pac.Next.Pos_y + distance - i;
            if (x >= 0 && x < sizeX && y >= 0 && y < sizeY)
                posible = grid.Find(x, y);
            i++;
        }
        return posible.Coord;
    }
}
