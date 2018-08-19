using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ply : MonoBehaviour {

    protected Rigidbody2D R2D;

    // Use this for initialization
    void Start () {
        R2D = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKey(KeyCode.A))
        {
            R2D.AddForce(new Vector2(-5, 0));
        }
    }
}
