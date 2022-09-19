using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    public float speed = 1.0f;
    void Update()
    {
        if (Input.GetKey("up"))
        {
            transform.Translate(Vector3.forward * Time.deltaTime * speed, Space.World);
        }

        if (Input.GetKey("down"))
        {
            transform.Translate(Vector3.back * Time.deltaTime * speed, Space.World);
        }

        if (Input.GetKey("left"))
        {
            transform.Translate(Vector3.left * Time.deltaTime * speed, Space.World);
        }

        if (Input.GetKey("right"))
        {
            transform.Translate(Vector3.right * Time.deltaTime * speed, Space.World);
        }

        if (Input.GetKey(KeyCode.I))
        {
            transform.Translate(Vector3.up * Time.deltaTime * speed, Space.World);
        }

        if (Input.GetKey(KeyCode.K))
        {
            transform.Translate(Vector3.down * Time.deltaTime * speed, Space.World);
        }
    }
}
