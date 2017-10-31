using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisingTextScript : MonoBehaviour {

    public TextMesh tm;
    private int temp = 51;  //0.02 lowers alpha by 5, for some reason. 51*5 = 255

    //Fade over time
	void FixedUpdate () {
        tm.color = new Color(tm.color.r, tm.color.g, tm.color.b, tm.color.a - .02f);
        temp--;
        if(temp == 0)
        {
            Destroy(gameObject);
        }
    }
}
