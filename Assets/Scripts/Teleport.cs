using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : Operator
{
    public override void OnTick()
    {
        if (node != null)
        {
            // if probing, enable the flags
            if(BuildManager.instance.activeBucket == 1)
            {
                node.SetProbeAcceptModes(2);
            }
            // consumes the hashtag
            node.HideHashtag();
            // teleports the node to the right hash
            int bucketIndex = node.GetHash();
            Bucket targetBucket = BuildManager.instance.buckets[bucketIndex];
            node.Teleport(targetBucket.nodesBucket);
            // follow node if it is from generator
            if (node.fromGenerator)
            {
                ViewManager.instance.LerpCam(targetBucket.transform.position + new Vector3(0, 0, -10));
            }
        }
    }
}
