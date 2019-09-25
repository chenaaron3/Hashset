using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Modulo : Operator
{
    Animator anim;

    public int forceOutput = -1;

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
            // gets the number of buckets
            int numBuckets = BuildManager.instance.NumBuckets;
            // finds the positive modulus value
            int val = node.GetHash() % numBuckets;
            val = val < 0 ? val + numBuckets : val;
            if(forceOutput >= 0)
            {
                val = forceOutput;
            }
            // compresses the hash value
            node.SetHash(val);
        }
    }
}
