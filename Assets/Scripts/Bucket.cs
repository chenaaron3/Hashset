using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bucket : Operator
{
    public GameObject nodesBucket;
    public Animator pushAnim;
    public Operator bracket;
    protected int index;
    public Text bucketIndex;

    public virtual void RehashNodes()
    {
    }

    public virtual bool CheckOverflow()
    {
        return false;
    }

    // deletes a ndoe and moves everything correctly
    public virtual void RemoveNode(GameObject n)
    {
        // moves children
        foreach(Transform child in nodesBucket.transform)
        {
            if(child.transform.position.x == n.transform.position.x)
            {
                Destroy(child.gameObject);
            }
            if (child.transform.position.x > n.transform.position.x)
            {
                child.GetComponent<Node>().MoveDirection(Vector2.left);
            }
        }
        // moves bracket
        bracket.transform.position += Vector3.left;
    }

    // sets the bucket's ui index
    public void SetBucketIndex(int index)
    {
        this.index = index;
        bucketIndex.text = index + "";
    }
}
