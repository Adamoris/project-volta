using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform player;
    public Transform target;

    // Update is called once per frame
    void Update()
    {
        target.transform.position = player.transform.position;
    }
}