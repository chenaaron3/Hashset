using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bracket : Operator
{
    Bucket myBucket;

    private void Start()
    {
        myBucket = transform.GetComponentInParent<Bucket>();
    }

    // request to add to bucket
    public override void OnTick()
    {
        // if node reached the end of bucket
        if (node != null)
        {
            // if is a deleter node
            if(node.deleter)
            {
                // destroy the deleter node
                Destroy(node.gameObject);
                Generator.instance.processing = false;
                ViewManager.instance.ResetCamera();
            }
            else
            {
                // if bucket is not overflowing
                if (myBucket.CheckOverflow())
                {
                    // stops the node
                    node.SetDirection(Vector2.zero);
                    // pushes the bracket over
                    transform.position += transform.right;
                    // if node was generated and not from hash
                    if (node.fromGenerator)
                    {
                        node.fromGenerator = false;
                        // complete process
                        Generator.instance.processing = false;
                        ViewManager.instance.ResetCamera();
                    }
                }
            }
        }
    }
}
