using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresentScript : MonoBehaviour {

    public GameObject spawn;
    public int direction = 0;
    public float distance = 0;
    public float xLoc;
    private bool hasBecomeVisible = false;
    public float fallSpeed;
    public GameObject risingText;
    private GameObject temp;
    private MeshRenderer mr;
    private TextMesh tm;
    public RisingTextScript rts;
    public SpriteRenderer presentSprite;
    private bool isFading = false;
    private int fadeCount;

    private void FixedUpdate()
    {
        transform.position -= transform.up * fallSpeed;
        if (isFading)
        {
            presentSprite.color = new Color(presentSprite.color.r, presentSprite.color.g, presentSprite.color.b, presentSprite.color.a - .02f);
            fadeCount--;
            if (fadeCount == 0)
            {
                Destroy(gameObject);
            }
        }
    }

    public void OnBecameVisible()
    {
        if (!hasBecomeVisible)
        {
            hasBecomeVisible = true;
            xLoc = transform.position.x;
            if (xLoc < -6)
            {
                direction = 1;
            }
            else if (xLoc > 6)
            {
                direction = -1;
            }
            else
            {
                direction = 1 - (Random.Range(0, 2) * 2);
            }

            distance = Random.Range(20, 60) / 10f;
            if (Mathf.Abs(xLoc + (distance * direction)) > 8)
            {
                Instantiate(spawn, new Vector3(xLoc + ((distance * direction) * -1), transform.position.y + 2), Quaternion.identity);
            }
            else
            {
                Instantiate(spawn, new Vector3(xLoc + (distance * direction), transform.position.y + 2), Quaternion.identity);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //fade over time
        switch (collision.gameObject.tag)
        {
            case "PresentDestroyer":
                fadeCount = 51; //0.02 lowers alpha by 5, for some reason. 51*5 = 255
                isFading = true;
                break;
        }
    }

    public void destroy(int score)
    {
        temp = Instantiate(risingText, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
        mr = temp.GetComponent<MeshRenderer>();
        tm = temp.GetComponent<TextMesh>();
        tm.text = score.ToString();
        mr.enabled = true;
        rts.enabled = true;
        Destroy(gameObject);
    }
}
