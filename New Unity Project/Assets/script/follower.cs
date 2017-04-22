using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class follower : MonoBehaviour {
    private Renderer myRenderer;
    private bool cIsWhite;
    private bool cIsOther;

    // Use this for initialization
    void Start () {
        myRenderer = GetComponent<Renderer>();
        cIsWhite = true;
        cIsOther = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (cIsWhite&&!cIsOther)
            {
                myRenderer.material.color = Color.green;
                cCheck();
            }
            else if (cIsOther&&!cIsWhite)
            {
                myRenderer.material.color = Color.blue;
                cCheck();
            }
            else if(cIsWhite&&cIsOther)
            {
                myRenderer.material.color = Color.white;
                cCheck();
            }
        }
    }
   
    private void cCheck()
    {
        if (myRenderer.material.color == Color.green)
        {
            cIsWhite = false;
            cIsOther = true;
        }
        else if (myRenderer.material.color == Color.blue)
        {
            cIsWhite = true;
            cIsOther = true;
        }
        else
        {
            cIsWhite = true;
            cIsOther = false;
        }
    }


}
