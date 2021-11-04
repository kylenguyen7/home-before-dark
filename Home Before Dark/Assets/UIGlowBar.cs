using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGlowBar : MonoBehaviour
{
    public IlluminationContainer myContainer;
    float startingScale;

    private void Start()
    {
        startingScale = transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        var scale = transform.localScale;
        scale.x = Mathf.Max(0, startingScale * ((myContainer.MyScaleSmooth - 0.05f) / myContainer.maxScale));
        transform.localScale = scale;
    }
}
