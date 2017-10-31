using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyerScript : MonoBehaviour {

    public Rigidbody2D playerBody;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerBody.gravityScale = 0;
            playerBody.velocity = new Vector2(0, 0);
        }
        else
        {
            Destroy(collision.gameObject);
        }
    }
}
