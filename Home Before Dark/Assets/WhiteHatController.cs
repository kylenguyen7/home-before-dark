using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteHatController : MonoBehaviour
{
    public int hp;
    public bool isEvil;

    // movement and animation
    Animator animator;
    Rigidbody2D rb;
    bool isDead = false;
    bool isRunning = true;
    bool facingRight = true;

    SpriteOutline myOutliner;

    // waypoints
    public WaypointInfo[] waypoints;
    public float waitTimeVariance = 0.2f;
    public float walkSpeed = 1f;

    private float waitTimer;
    private int waypointIndex = 0;
    public states state;

    private bool isCheering = false;

    public AudioClip deadSound;
    private AudioSource mySource;

    public enum states
    {
        walking,
        waiting,
        cheering,
        dead
    }
    
    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        myOutliner = gameObject.GetComponent<SpriteOutline>();

        waypointIndex = 0;
        state = states.walking;
        animator.SetBool("isRunning", isRunning);
        mySource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case states.dead: deadState(); break;
            case states.cheering: cheerState(); break;
            case states.walking: walkState(); break;
            case states.waiting: waitState(); break;
        }
    }

    private void cheerState()
    {
        if(!isDead && !isCheering)
        {
            animator.SetTrigger("cheer");
        }
        isCheering = true;
    }

    private void deadState()
    {
        isDead = true;
    }

    private void walkState()
    {
        if(waypointIndex > waypoints.Length - 1 || rb == null)
        {
            return;
        }
        
        Transform destination = waypoints[waypointIndex].gameObject.transform;
        Vector2 dir = (Vector2)(destination.position - transform.position);
        dir.Normalize();

        rb.velocity = dir * walkSpeed;

        if (Vector2.Distance(transform.position, destination.position) < 0.2f )
        {
            waitTimer = waypoints[waypointIndex].waitTime;
            if(waitTimer != 0) waitTimer += Random.Range(-waitTimeVariance, waitTimeVariance);

            waypointIndex = (waypointIndex + 1) % waypoints.Length;
            state = states.waiting;
            rb.velocity = Vector3.zero;
            isRunning = false;
            animator.SetBool("isRunning", isRunning);
            // Debug.Log("Waiting now.");
        }

        if ((facingRight && rb.velocity.x < 0) || (!facingRight && rb.velocity.x > 0))
        {
            Flip();
        }

    }

    private void waitState()
    {
        if(waitTimer <= 0)
        {
            state = states.walking;
            isRunning = true;
            animator.SetBool("isRunning", isRunning);
            // Debug.Log("Done waiting!");
        }
        waitTimer -= Time.deltaTime;
    }

    public void takeDamage(int amount)
    {
        if (isDead) return;
        
        hp -= amount;
        animator.SetTrigger("hit");

        if(hp <= 0)
        {
            Die();
        }
    }

    public void toggleOutline(bool turnOn)
    {
        myOutliner.outlineSize = turnOn ? 1 : 0;
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector2 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void Die()
    {
        animator.SetBool("isDead", true);
        gameObject.layer = LayerMask.NameToLayer("Corpses");
        isDead = true;
        state = states.dead;

        if (rb != null)
        {
            rb.velocity = Vector3.zero;
        }

        PlayerController player = GameObject.FindObjectOfType<PlayerController>();
        if(player != null)
        {
            if (isEvil)
            {
                var text = GameObject.FindObjectOfType<UIDarkHatLabel>();
                if (text != null) text.newDefeated();
                player.glowModify(0.1f);
            }
            else
            {
                player.glowModify(-0.6f);
            }
        }
    }

    public void playDeadSound()
    {
        if (deadSound != null) mySource.PlayOneShot(deadSound);

        var scs = GameObject.FindObjectOfType<SimpleCameraShake>();
        if (scs != null)
        {
            scs.shakeItUp();
        }
    }
}
