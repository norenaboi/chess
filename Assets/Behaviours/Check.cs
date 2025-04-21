using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Check : MonoBehaviour
{
    public bool ifnotoccupied;
    private void Start()
    {
        ifnotoccupied = true;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("triggered");
        ifnotoccupied = false;
    }
}
