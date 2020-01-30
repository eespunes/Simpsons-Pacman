using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private GridTest g;
    private void Start()
    {
        g = GameObject.Find("Grid").GetComponent<GridTest>();
    }
    private void OnMouseDown()
    {
        GetComponent<MeshRenderer>().material.color = Color.red;
        g.SetStart(transform.position);
    }
}
