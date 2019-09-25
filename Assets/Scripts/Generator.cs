using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : Operator
{
    public static Generator instance;

    Animator anim;
    public GameObject nodePrefab;
    // line for dispensing nodes
    public List<Node> queue;
    // if a node is currently traversing through
    public bool processing;
    // if the line is moving
    bool consuming;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        queue = new List<Node>();
        anim = GetComponent<Animator>();
        anim.speed = TickManager.tickSpeed;
        processing = false;
    }

    public void GenerateRandom()
    {
        string glyphs = "abcdefghijklmnopqrstuvwxyz0123456789";
        int charAmount = Random.Range(1, 5);
        string myString = "";
        for (int i = 0; i < charAmount; i++)
        {
            myString += glyphs[Random.Range(0, glyphs.Length)];
        }
        GenerateNode(myString);
    }

    public override void OnTick()
    {
        // spins wheel
        anim.SetTrigger("generate");

        // if requested and currently not processing and not rehashing
        if (queue.Count > 0 && !processing && !Rehash.instance.rehashing)
        {
            BuildManager.instance.NumNodes++;
            // if should continue
            if (BuildManager.instance.CheckLoadFactor())
            {
                // starts processing the node
                processing = true;
                // creates the node
                Node node = Consume();
                // pushes the node
                node.SetDirection(transform.right);
            }
            else
            {
                // node did not get generated
                BuildManager.instance.NumNodes--;
            }
        }
    }

    // assume that there is something to consume
    Node Consume()
    {
        consuming = true;
        Invoke("EndConsume", 1 / TickManager.tickSpeed);
        // moves every node up
        foreach(Node n in queue)
        {
            n.MoveDirection(Vector2.up);
        }
        // retreives the first node
        Node node = queue[0];
        queue.RemoveAt(0);
        return node;
    }

    // stops triggers the end of consumption
    void EndConsume()
    {
        consuming = false;
    }

    // generates a node
    public void GenerateNode(string val)
    {
        if(val.Equals(""))
        {
            return;
        }
        if(!consuming)
        {
            // gets the position to spawn the node
            Vector2 pos = (Vector2)(queue.Count == 0 ? transform.position : queue[queue.Count - 1].transform.position) + Vector2.down;
            Node n = Instantiate(nodePrefab, pos, Quaternion.identity, transform).GetComponent<Node>();
            n.fromGenerator = true;
            n.SetValue(val);
            queue.Add(n);
        }
    }

    // generates a node and converts it into a deleter
    public void GenerateDeleterNode(string val)
    {
        if (!consuming)
        {
            // gets the position to spawn the node
            Vector2 pos = (Vector2)(queue.Count == 0 ? transform.position : queue[queue.Count - 1].transform.position) + Vector2.down;
            Node n = Instantiate(nodePrefab, pos, Quaternion.identity, transform).GetComponent<Node>();
            n.fromGenerator = true;
            n.SetValue(val);
            n.TransformDeleter();
            queue.Add(n);
        }
    }

    // clears line and ends processing
    public void Reset()
    {
        queue.Clear();
        processing = false;
    }
}
