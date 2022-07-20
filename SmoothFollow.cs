using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
    public Transform target;
    float distance = 3.5f;
    float height = 1.9f;
    float heightOffset = .0f;
    float heightDamping = 5.0f;
    float rotationDamping = 3.0f;
   
    void Start()
    {
        
    }

    void LateUpdate()
    {
        if (target == null)
            return;

        if (!PlayerController.isDead)
        {
            float wantedRotationAngle = target.eulerAngles.y;
            float wantedHeight = target.position.y + height;

            float currentRotationAngle = transform.eulerAngles.y;
            float currentHeight = transform.position.y;

            currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);
            currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

            Quaternion currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

            transform.position = target.position;
            transform.position -= currentRotation * Vector3.forward * distance;

            transform.position = new Vector3(transform.position.x,
                                    currentHeight + heightOffset,
                                    transform.position.z);
        }

        transform.LookAt(target);
    }
}
