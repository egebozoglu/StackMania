using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Movement();
    }

    void Movement()
    {
        if (PlayerController.instance.alive)
        {
            transform.position = PlayerController.instance.cameraPosition.transform.position;
            transform.rotation = PlayerController.instance.cameraPosition.transform.rotation;
        }
    }
}
