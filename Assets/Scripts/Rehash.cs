using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rehash : MonoBehaviour
{
    public static Rehash instance;

    public GameObject spawn;
    public GameObject pending;
    public List<Node> queue;

    public bool rehashing;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        queue = new List<Node>();
        rehashing = false;
    }

    private void OnEnable()
    {
        TickManager.OnTick += OnTick;
    }

    private void OnDisable()
    {
        TickManager.OnTick -= OnTick;
    }

    // dispenses each node
    void OnTick()
    {
        if(rehashing)
        {
            // sends off nodes to be rehashed
            if (queue.Count > 0)
            {
                Node node = queue[0];
                queue.RemoveAt(0);
                node.SetDirection(transform.right * -1);
                node.transform.parent = pending.transform;
            }
            // finish rehashing process
            else
            {
                rehashing = false;
            }
        }
    }

    // in case of rehashing during a rehash
    public void RehashInterrupt()
    {
        // sends all nodes in pending to rehash
        while (pending.transform.childCount != 0)
        {
            Transform child = pending.transform.GetChild(0);           
            Node node = child.GetComponent<Node>();
            node.HideHashtag();
            RehashNode(node);
        }
    }

    // prepares a node to be rehashed
    public void RehashNode(Node node)
    {
        node.Teleport(spawn);
        queue.Add(node);
    }

    // cleares line and ends rehashing
    public void Reset()
    {
        queue.Clear();
        rehashing = false;
    }
}
