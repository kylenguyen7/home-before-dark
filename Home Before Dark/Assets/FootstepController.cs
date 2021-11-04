using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepController : MonoBehaviour
{
    public PlayerController player;
    public float stepTime = 0.1f;
    public bool jump = false;
    public bool dead = false;
    float stepTimer;

    public AudioSource stepSource;
    public AudioSource jumpSource;
    public AudioSource deadSource;
    public AudioClip stepSound;
    public AudioClip jumpSound;
    public AudioClip deadSound;

    private void Start()
    {
        player = GameObject.FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        jumpSource.volume = 0.6f;
        
        if (stepTimer <= 0 && player.isRunning && player.isGrounded)
        {
            stepSource.PlayOneShot(stepSound);
            stepTimer = stepTime;
        }
        stepTimer -= Time.deltaTime;

        if (!player.isRunning || !player.isGrounded) stepSource.volume = 0;
        else stepSource.volume = 0.69f;

        if(jump)
        {
            jumpSource.PlayOneShot(jumpSound);
            jump = false;
        }

        if(dead)
        {
            deadSource.PlayOneShot(deadSound);
            dead = false;
        }
    }
}
