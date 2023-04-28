using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float x, y, z;
    private void FixedUpdate()
    {
        transform.Rotate(x, y, z);
    }
}
