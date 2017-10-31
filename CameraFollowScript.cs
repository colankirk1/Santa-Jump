using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Track the player position above a certain point
public class CameraFollowScript : MonoBehaviour
{
    public Transform player;
    private float highest = 0;
    private float playerY;

    void LateUpdate()
    {
        playerY = player.position.y;
        if(highest < 4 && playerY < 0)
        {
            transform.position = new Vector3(0, 0, -10);
        }
        else
        {
            if(playerY > highest-4)
            {
                transform.position = new Vector3(0, playerY, -10);
                if(playerY > highest)
                {
                    highest = playerY;
                }
            }
        }
    }
}
