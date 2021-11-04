using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelFinisher : MonoBehaviour
{
    Text myText;
    public Image blackScreen;
    public float textDelay = 1f;
    public float transitionDelay = 5f;
    public bool isFinished = false;
    public bool isGameOver = false;

    private float stepTimer = 0.19f;
    
    // Start is called before the first frame update
    void Start()
    {
        myText = gameObject.GetComponent<Text>();
        var col = myText.color;
        col.a = 0;
        myText.color = col;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (isGameOver)
        {
            myText.text = "You ran out of light.\nPress R to restart.";
            if (textDelay <= 0)
            {
                var col = myText.color;
                myText.fontSize = 200;
                col.r = 256;
                col.g = 256;
                col.b = 256;
                col.a = 1;
                myText.color = col;
            }
            textDelay -= Time.deltaTime;
            return;
        }

        if (!isFinished)
        {
            if (stepTimer <= 0)
            {
                var col = blackScreen.color;
                col.a -= 0.1f;
                col.a = Mathf.Max(0, col.a);
                blackScreen.color = col;
                stepTimer = 0.19f;
            }
            stepTimer -= Time.deltaTime;
            return;
        }

        if(textDelay <= 0)
        {
            var col = myText.color;
            col.a = 1;
            myText.color = col;
        }
        textDelay -= Time.deltaTime;

        if(transitionDelay <= 0)
        {
            if(stepTimer <= 0)
            {
                var col = blackScreen.color;
                col.a += 0.1f;
                blackScreen.color = col;
                stepTimer = 0.19f;

                if(col.a >= 1)
                {
                    int index = SceneManager.GetActiveScene().buildIndex;
                    SceneManager.LoadScene(index + 1);
                }
            }
            stepTimer -= Time.deltaTime;
        }
        transitionDelay -= Time.deltaTime;
    }
}
