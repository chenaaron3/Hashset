using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mode : MonoBehaviour
{
    public static Mode instance;
    public int mode = 1;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        BuildManager.instance.activeBucket = mode;
    }
}
