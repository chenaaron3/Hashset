using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;

    public Text bucketText;
    public Text nodeText;
    public Slider loadSlider;

    int numBuckets;
    public int NumBuckets
    {
        get
        {
            return numBuckets;
        }
        set
        {
            numBuckets = value;
            bucketText.text = "Buckets: " + numBuckets;
            loadSlider.value = (numNodes * 1.0f / numBuckets) / loadFactor;
        }
    }
    int numNodes;
    public int NumNodes
    {
        get
        {
            return numNodes;
        }
        set
        {
            numNodes = value;
            nodeText.text = "Nodes: " + numNodes;
            loadSlider.value = (numNodes * 1.0f / numBuckets) / loadFactor;
        }
    }
    public float loadFactor;

    public GameObject bucketAnchor;
    public GameObject[] bucketPrefabs;
    public List<Bucket> buckets;
    public int activeBucket;
    Vector2 bucketCursor;

    // how many buckets in each column
    int maxHeight = 10;
    // the width of each bucket
    int bucketWidth;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        buckets = new List<Bucket>();
        bucketCursor = bucketAnchor.transform.position;    
        NumNodes = 0;
        AddBuckets(1);
    }

    IEnumerator CreateBucket()
    {
        yield return new WaitForEndOfFrame();
        AddBuckets(1);
    }

    public float GetLoadAmount()
    {
        return numNodes * 1.0f / numBuckets;
    }

    // return true if should continue
    public bool CheckLoadFactor()
    {
        if(GetLoadAmount() >= loadFactor)
        {
            RehashBuckets();
            return false;
        }
        return true;
    }

    public void RehashBuckets()
    {
        Rehash.instance.RehashInterrupt();
        // sends all nodes to be rehashed
        foreach (Bucket bucket in buckets)
        {
            bucket.RehashNodes();
        }
        // trigger rehasher to start dispensing
        Invoke("InvokeRehash", 1 / TickManager.tickSpeed * .5f);
        // adds more buckets
        AddBuckets(numBuckets / 2 + 1);
        // orders the buckets nicely
        ReorderBuckets();
    }

    void InvokeRehash()
    {
        Rehash.instance.rehashing = true;
    }

    // creates and tracks the bucket
    public void AddBuckets(int num)
    {
        NumBuckets += num;
        for(int j = 0; j < num; j ++)
        {
            Bucket bucket = Instantiate(bucketPrefabs[activeBucket], bucketCursor, Quaternion.identity).GetComponent<Bucket>();
            bucketCursor += Vector2.down;
            bucket.SetBucketIndex(buckets.Count);
            buckets.Add(bucket);
        }
    }

    // reorders the buckets based on given width and height
    void ReorderBuckets()
    {
        bucketCursor = bucketAnchor.transform.position;
        int sqrt = Mathf.CeilToInt(Mathf.Sqrt(buckets.Count));
        int height = Mathf.Min(sqrt, maxHeight);
        for(int j = 0; j < buckets.Count; j++)
        {
            buckets[j].transform.position = bucketCursor;
            // moves cursor down
            bucketCursor += new Vector2(0, -1);
            // if hit max height, move to the right
            int width = 2;
            width += activeBucket == 0 ? ChainBucket.maxCapacity : ProbeBucket.maxCapacity;
            if(bucketAnchor.transform.position.y - bucketCursor.y >= height)
            {
                bucketCursor = new Vector2(bucketCursor.x + width, bucketAnchor.transform.position.y);
            }
        }
    }

    public void ChangeCollisionMethod(int index)
    {
        activeBucket = index;
        Restart();
        //Mode.instance.mode = index;
        //SceneManager.LoadScene(0);
    }

    public Bucket GetNextBucket(int index)
    {
        // if last bucket, return the first bucket
        if(index == buckets.Count - 1)
        {
            return buckets[0];
        }
        return buckets[index + 1];
    }

    public void Restart()
    {
        Generator.instance.Reset();
        Rehash.instance.Reset();
        NumNodes += Generator.instance.queue.Count;
        // destroys all the nodes
        foreach (Node n in FindObjectsOfType<Node>())
        {
            Destroy(n.gameObject);
        }
        // destroys all the buckets
        while (buckets.Count > 0)
        {
            Destroy(buckets[0].gameObject);
            buckets.RemoveAt(0);
        }
        // resets setup
        bucketCursor = bucketAnchor.transform.position;
        NumBuckets = 0;
        AddBuckets(1);
    }
}
