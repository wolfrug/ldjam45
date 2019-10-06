using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float speed = 1f;
    public Vector3 direction = Vector3.left;
    public bool activated = true;
    private float lerp = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (activated){
            transform.Rotate(direction, speed * Time.deltaTime, Space.Self);
        }
    }
}
