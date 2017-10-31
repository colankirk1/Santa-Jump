using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScript : MonoBehaviour
{
    public PlatformerCharacter2D player;
    public Transform cameraLoc;
    public float offset;
    public float speedPercentage = 0.5f;
    public bool VisibleatStart;
    public float startOffset;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlatformerCharacter2D>();
        cameraLoc = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    void Update () {
		if(cameraLoc.position.y < 0)
        {
            transform.position = new Vector3(0, 0 + offset, 0);
        }
        else
        {
            transform.position = new Vector3(0, (cameraLoc.position.y * speedPercentage) + offset, 0);
        }
    }

    public void changeOffset(float off)
    {
        offset = off;
    }

    public void OnBecameVisible()
    {
        if(!VisibleatStart)
            player.spawnBackground();
    }
}
