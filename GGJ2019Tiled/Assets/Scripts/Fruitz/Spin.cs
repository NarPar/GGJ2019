using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    public float speed = 700.0f;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(transform.position, transform.forward, speed * Time.deltaTime);
        //transform.Rotate(Vector3.forward, speed * Time.deltaTime);

        //GetComponent<Rigidbody2D>().AddTorque(speed * Time.deltaTime);

        //rb.MoveRotation(rb.rotation + speed * Time.deltaTime);
    }
}
