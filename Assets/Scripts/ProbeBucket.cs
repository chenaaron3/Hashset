using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProbeBucket : Bucket
{
    public static int maxCapacity;

    public SpriteRenderer[] pusherParts;

    public enum ProbeMode { NEW, USED, OCCUPIED};
    Color[] modeColors = { Color.green, Color.yellow, Color.red };
    ProbeMode mode;
    public ProbeMode Mode
    {
        get
        {
            return mode;
        }
        set
        {
            mode = value;
            foreach(SpriteRenderer sr in pusherParts)
            {
                sr.color = modeColors[(int)mode];
            }
        }
    }
    // value of node if occupied
    public string nodeValue;

    private void Start()
    {
        pushAnim.speed = TickManager.tickSpeed;
        maxCapacity = 1;
        Mode = ProbeMode.NEW;
    }

    public override void OnTick()
    {
        // triggers pin to push
        pushAnim.SetTrigger("push");
        if (node != null)
        {
            if(node.deleter)
            {
                // if flag matches bucket (green or yellow)
                if (node.probeFlags.Contains(mode))
                {
                    // kills itslef on bracket
                    node.SetDirection(transform.right);
                    node.SetProbeAcceptModes(0);
                    nodeValue = "";
                }
                // if node matches bucket's node
                else if (Mode == ProbeMode.OCCUPIED && node.GetValue().Equals(nodeValue))
                {
                    // destroys the other node
                    node.SetDirection(transform.right);
                    node.SetProbeAcceptModes(0);
                    nodeValue = "";
                    // marks the bucket as used
                    Mode = ProbeMode.USED;
                }
                // probe
                else
                {
                    // probe
                    GameObject nextBucket = BuildManager.instance.GetNextBucket(index).nodesBucket;
                    ViewManager.instance.LerpCam(nextBucket.transform.position + new Vector3(0, 0, -10));
                    node.Teleport(nextBucket);
                    node.SetProbeAcceptModes(1);
                }
            }
            else
            {
                // if flag matches bucket
                if(node.probeFlags.Contains(mode))
                {
                    // insert
                    node.SetDirection(transform.right);
                    Mode = ProbeMode.OCCUPIED;
                    node.SetProbeAcceptModes(0);
                    nodeValue = node.GetValue();
                }
                // if node matches bucket's node
                else if(Mode == ProbeMode.OCCUPIED && node.GetValue().Equals(nodeValue))
                {
                    // insert
                    node.SetDirection(transform.right);
                    node.SetProbeAcceptModes(0);
                }
                // probe
                else
                {
                    // probe
                    GameObject nextBucket = BuildManager.instance.GetNextBucket(index).nodesBucket;
                    ViewManager.instance.LerpCam(nextBucket.transform.position + new Vector3(0, 0, -10));
                    node.Teleport(nextBucket);
                    node.SetProbeAcceptModes(1);
                }
            }
        }
    }

    // returns true if should continue
    public override bool CheckOverflow()
    {
        return true;
    }

    public override void RehashNodes()
    {
        // sends all nodes to rehash
        while (nodesBucket.transform.childCount != 0)
        {
            Transform child = nodesBucket.transform.GetChild(0);
            Rehash.instance.RehashNode(child.GetComponent<Node>());
        }
        // resets bracket
        bracket.transform.position = transform.position + transform.right;
        Mode = ProbeMode.NEW;
    }
}
