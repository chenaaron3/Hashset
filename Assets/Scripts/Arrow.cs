using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Operator
{
    public Animator anim;
    public Arrow next;
    public bool first;
    public int pulseRate = 4; // how many flashes before next pulse
    float flashSpeed = .25f; // time beteween each flash

    private void Start()
    {
        if(first)
        {
            StartCoroutine(Pulse());
        }
    }

    public override void OnTick()
    {
        if (node != null)
        {
            // redirects node
            node.SetDirection(transform.right);
        }
    }

    // only first arrows initiate pulse
    IEnumerator Pulse()
    {
        StartCoroutine(Flash());
        yield return new WaitForSeconds(flashSpeed * pulseRate);
        StartCoroutine(Pulse());
    }

    // follows the chain
    public IEnumerator Flash()
    {
        anim.SetTrigger("flash");
        yield return new WaitForSeconds(flashSpeed);
        if(next != null)
        {
            next.StartCoroutine(next.Flash());
        }
    }
}
