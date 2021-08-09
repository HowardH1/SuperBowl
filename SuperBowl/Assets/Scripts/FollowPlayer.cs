using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform target;
    [Range(0f, 1.0f)]
    public float smooth = 0.5f;
    public float rotationSpeed = 5f;
    public bool canRotate = false;
    private Vector3 cameraOffset;
    private Vector3 velocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        cameraOffset = transform.position - target.position;
    }

    void LateUpdate()
    {
        enterRotationMode();
        if (canRotate==true)
        {
            Quaternion camTurnAngle = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * rotationSpeed, Vector3.up);
            cameraOffset = camTurnAngle * cameraOffset;
        }

        Vector3 newPos = target.position + cameraOffset;

        transform.position = Vector3.SmoothDamp(transform.position, newPos, ref velocity, smooth);
        transform.LookAt(target);
    }

    void enterRotationMode()
    {
        if(Input.GetKeyDown(KeyCode.R) && canRotate == false)
        {
            canRotate = true;
        }
        else if(Input.GetKeyUp(KeyCode.R) && canRotate == true)
        {
            canRotate = false;
        }
    }
}
