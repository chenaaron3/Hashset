using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewManager : MonoBehaviour
{
    public static ViewManager instance;

    public GameObject cam;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        // be able to move camera when not processing
    }

    public void LerpCam(Vector3 dest)
    {
        StartCoroutine(LerpCamRoutine(dest, 1 / TickManager.tickSpeed, false));
    }

    public IEnumerator LerpCamRoutine(Vector3 dest, float time, bool delay)
    {
        if(delay)
        {
            // waits 1 tick before moving
            yield return new WaitForSeconds(1 / TickManager.tickSpeed);
        }
        Vector3 currentPos = cam.transform.position;
        float t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / time;
            cam.transform.position = Vector3.Lerp(currentPos, dest, t);
            yield return null;
        }
        cam.transform.position = dest;
    }

    public void ResetCamera()
    {
        StartCoroutine(LerpCamRoutine(new Vector3(0, 0, -10), 1 / TickManager.tickSpeed, true));
    }
}
