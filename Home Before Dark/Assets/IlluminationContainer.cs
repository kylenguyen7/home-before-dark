using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IlluminationContainer : MonoBehaviour
{
    public float startScale = 1f;
    public float decayPerSecond = 0.01f;
    public float maxScale = 2f;
    public bool isDead = false;
    public bool levelOver = false;

    private float myScale;

    private float myScaleSmooth;

    public float MyScaleSmooth
    {
        get { return myScaleSmooth; }
    }


    // Start is called before the first frame update
    void Start()
    {
        myScale = startScale;
        myScaleSmooth = startScale;
    }

    // Update is called once per frame
    void Update()
    {
        if(levelOver)
        {
            myScale += 0.05f;
        } else
        {
            myScale -= decayPerSecond * Time.deltaTime;
            myScale = Mathf.Max(myScale, 0);
        }

        myScaleSmooth += (myScale - myScaleSmooth) / 8f;
        transform.localScale = new Vector3(myScaleSmooth, myScaleSmooth, myScaleSmooth);

        if(myScaleSmooth < 0.05f)
        {
            isDead = true;
        }
    }

    public void grow(float amount)
    {
        if (levelOver) return;

        myScale += amount;
        myScale = Mathf.Clamp(myScale, 0, maxScale);
    }
}
