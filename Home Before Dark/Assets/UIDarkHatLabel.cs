using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDarkHatLabel : MonoBehaviour
{
    Text myText;
    public Image darkHat;
    public LevelFinisher finisher;
    public float numDefeated = 0;

    private void Start()
    {
        myText = gameObject.GetComponent<Text>();
        myText.text = GameObject.FindGameObjectsWithTag("Evil").Length.ToString();
    }

    public void newDefeated()
    {
        numDefeated += 0.5f;
        myText.text = (GameObject.FindGameObjectsWithTag("Evil").Length - (int)numDefeated).ToString();
    }

    public void attemptEndLevel()
    {
        if (GameObject.FindGameObjectsWithTag("Evil").Length - (int)numDefeated == 0)
        {
            EndLevel();
        }
    }

    private void EndLevel()
    {
        finisher.isFinished = true;
        darkHat.enabled = false;
        
        var hats = GameObject.FindGameObjectsWithTag("Good");
        Debug.Log(hats);
        foreach (GameObject hat in hats)
        {
            var hatController = hat.GetComponent<WhiteHatController>();
            if (hatController != null)
            {
                hatController.state = WhiteHatController.states.cheering;
                Debug.Log("got here");
            }

            var hatRb = hat.GetComponent<Rigidbody2D>();
            if(hatRb != null)
            {
                hatRb.velocity = new Vector2(0, 0);
            }
        }

        var containers = GameObject.FindObjectsOfType<IlluminationContainer>();
        foreach (IlluminationContainer container in containers)
        {
            container.levelOver = true;
        }
    }
}
