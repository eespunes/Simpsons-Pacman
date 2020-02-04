using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Donuts : MonoBehaviour
{
    private GameController gc;
    private int x, y;

    private void Start()
    {
        gc = GameObject.Find("Game Controller").GetComponent<GameController>();
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Player")
        {
            if (gameObject.name.Contains("Venus"))
                gc.AddSpecialPoint(x, y);

            else
                if (gameObject.name.Contains("Beer"))
                gc.EatingTime();
            gc.AddNormalPoint(x, y, 1);
            Destroy(gameObject);
        }
    }
    public void AddPosition(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}
