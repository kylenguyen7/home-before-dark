using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : MonoBehaviour
{
    public GameObject selectedWhiteHatHandler;
    
    // Update is called once per frame
    void Update()
    {
        var mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        transform.position = mousePos;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        var whh = collision.gameObject.GetComponent<WhiteHatHandler>();
        if(whh != null)
        {
            selectedWhiteHatHandler = whh.gameObject;
            whh.masterToggleOutline(true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        var whh = collision.gameObject.GetComponent<WhiteHatHandler>();
        if (whh != null)
        {
            selectedWhiteHatHandler = whh.gameObject;
            whh.masterToggleOutline(false);
        }

        selectedWhiteHatHandler = null;
    }
}
