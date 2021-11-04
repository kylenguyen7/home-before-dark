using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IlluminationController : MonoBehaviour
{
    public float glowTime = 3f;
    Animator animator;

    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(glowTime < 0)
        {
            animator.SetTrigger("end");
        }
        glowTime -= Time.deltaTime;
    }

    public void myDestroy()
    {
        Destroy(gameObject);
    }
}
