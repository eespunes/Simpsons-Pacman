using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacMan : MonoBehaviour
{
    private float v;

    private int horizontal, vertical;

    private bool change;

    private Node next;
    private Grid grid;
    private Node[,] n;
    private GameController gc;
    private Node initial;

    void Start()
    {
        gc = GameObject.Find("Game Controller").GetComponent<GameController>();
        grid = GameObject.Find("Grid").GetComponent<Grid>();
        v = gc.V;
        next = grid.Find((int)grid.pac_pos.x, (int)grid.pac_pos.y);
        transform.position = next.Coord;
        initial = next;
        change = false;
        horizontal = -1;
    }

    void Update()
    {
        if (gc.Start1)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (horizontal != 0)
                    change = true;
                transform.rotation = Quaternion.Euler(90, 0, 0);
                horizontal = -1;
                vertical = 0;
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (horizontal != 0)
                    change = true;
                transform.rotation = Quaternion.Euler(-90, 180, 0);
                vertical = 0;
                horizontal = 1;
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (vertical != 0)
                    change = true;
                transform.rotation = Quaternion.Euler(90, 0, -90);
                horizontal = 0;
                vertical = 1;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (vertical != 0)
                    change = true;
                transform.rotation = Quaternion.Euler(90, 0, 90);
                vertical = -1;
                horizontal = 0;
            }


            if (change || transform.position == next.Coord)
            {
                change = false;
                n = grid.TheNodes;
                if ((next.Pos_x + horizontal) >= grid.sizeX)
                {
                    transform.position = n[0, next.Pos_y].Coord;
                    next = n[0 + horizontal, next.Pos_y + vertical];
                }
                else if ((next.Pos_x + horizontal) < 0)
                {
                    transform.position = n[(grid.sizeX - 1), next.Pos_y].Coord;
                    next = n[grid.sizeX - 1 + horizontal, next.Pos_y + vertical];
                }
                else
                    next = n[next.Pos_x + horizontal, next.Pos_y + vertical];
            }
            if (next.Walkable)
                transform.position = Vector3.MoveTowards(transform.position, next.Coord, v);
            else
            {
                next = grid.TheNodes[next.Pos_x - horizontal, next.Pos_y - vertical];
                horizontal = 0;
                vertical = 0;
            }
        }
    }

    public void ResetPosition()
    {
        transform.rotation = Quaternion.Euler(90, 0, 0);
        horizontal = -1;
        next = initial;
        transform.position = initial.Coord;
    }

    //Gets and setters
    public Node Next
    {
        get
        {
            return next;
        }
    }
}
