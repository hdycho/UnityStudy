using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ladder : MonoBehaviour,IUseable {

    [SerializeField]
    private Collider2D platformCollider;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Use()
    {
        if(player.Instance.OnLadder)
        {
            UseLadder(false, 3,0,1);
        }
        else
        {
            UseLadder(true, 0,1,0);
            Physics2D.IgnoreCollision(player.Instance.GetComponent<Collider2D>(), platformCollider, true);
        }
    }

    private void UseLadder(bool onLadder,int gravity,int layerWeight,int animSpeed)
    {
        player.Instance.OnLadder = onLadder;
        player.Instance.MyRigidbody.gravityScale = gravity;
        player.Instance.myAnimator.SetLayerWeight(2, layerWeight);
        player.Instance.myAnimator.speed = animSpeed;
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag=="player")
        {
            UseLadder(false, 3,0,1);
            Physics2D.IgnoreCollision(player.Instance.GetComponent<Collider2D>(), platformCollider, false);
        }
        else
        {
            UseLadder(false, 3,0,1);
        }
    }
}
