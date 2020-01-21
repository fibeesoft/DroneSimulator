using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform playerTransform;
    Vector3 playerPosition;
    Vector3 cameraPosition;
 
     void Update()
    {
        playerPosition = playerTransform.position;
        cameraPosition = new Vector3(playerPosition.x, playerPosition.y + 1f, playerPosition.z - 5f);
    }
     void FixedUpdate()
    {
        FollowWithLerp();
    }
 
    void Follow(){
        transform.position = cameraPosition;
    }
 
    void FollowWithLerp(){
 
        transform.position = Vector3.Lerp(transform.position, cameraPosition, 0.3f);
    }
}
