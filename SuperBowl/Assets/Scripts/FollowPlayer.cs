using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform target;
    public float distance;
    public float height;
    private Vector3 cameraBoom;
    private bool cameraState;
    public float smooth = 5.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        cameraState = gameObject.activeSelf;
        cameraBoom = new Vector3(target.position.x + distance, target.position.y + height, target.position.z);
        transform.position = Vector3.Lerp(transform.position, cameraBoom, Time.deltaTime * smooth);
    }

}
