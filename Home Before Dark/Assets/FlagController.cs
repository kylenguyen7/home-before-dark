using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlagController : MonoBehaviour
{
    public UIDarkHatLabel tester;
    public Text myText;
    public Text myText2;

    private void Update()
    {
        if (GameObject.FindGameObjectsWithTag("Evil").Length - (int)tester.numDefeated == 0)
        {
            myText.enabled = false;
            myText2.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player")) tester.attemptEndLevel();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) tester.attemptEndLevel();
    }
}
