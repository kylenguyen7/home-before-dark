using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIIlluminatorsMask : MonoBehaviour
{
    public PlayerController player;
    RectTransform rectTransform;
    float startWidth;

    void Start()
    {
        rectTransform = gameObject.GetComponent<RectTransform>();
        startWidth = rectTransform.rect.width;
    }

    // Update is called once per frame
    void Update()
    {
        float ratio = (float)player.NumIlluminators / player.maxIlluminators;
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, startWidth * ratio);

        Debug.Log(startWidth * ratio);
    }
}
