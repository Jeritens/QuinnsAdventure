using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    Transform playerTransform;
    [SerializeField]
    Vector3 offset;

    private void LateUpdate() {
        transform.position = playerTransform.position + offset;
    }
}
