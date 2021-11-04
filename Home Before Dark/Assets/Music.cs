using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if(GameObject.FindObjectsOfType<Music>().Length > 1)
        {
            Destroy(gameObject);
        }
    }
}
