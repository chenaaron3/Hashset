using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hashtag : Operator
{
    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        anim.speed = TickManager.tickSpeed;
    }

    public override void OnTick()
    {
        anim.SetTrigger("operate");
        if (node != null)
        {
            // sets the hash to the string's hashcode
            node.SetHash(node.GetValue().ToString().GetHashCode());
        }
    }
}
