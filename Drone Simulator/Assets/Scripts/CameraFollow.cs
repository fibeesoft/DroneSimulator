using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform playerTransform;
    Vector3 playerPosition;
    Vector3 cameraPosition;
     void LateUpdate()
    {
        playerPosition = playerTransform.position;
        cameraPosition = new Vector3(playerPosition.x, playerPosition.y + 1f, playerPosition.z - 5f);
        transform.position = cameraPosition;
    }
}
