using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Operator : MonoBehaviour
{
    public Node node;

    private void OnEnable()
    {
        TickManager.OnTick += OnTick;
    }

    private void OnDisable()
    {
        TickManager.OnTick -= OnTick;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Node"))
        {
            node = collision.GetComponent<Node>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Node"))
        {
            node = null;
        }
    }

    public virtual void OnTick()
    {
    }
}
