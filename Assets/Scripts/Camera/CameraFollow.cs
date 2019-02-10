using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothing = 5f;

    Vector3 offset; //offset between camera and player

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - target.position; 
    }

    // Update is called once per frame
    /*void Update()
    {
        
    }*/

    // Since we are following a Physics Object
    private void FixedUpdate()
    {
        Vector3 targetCamPos = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
    }
}
