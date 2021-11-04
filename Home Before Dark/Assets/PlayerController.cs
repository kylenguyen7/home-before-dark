using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float runSpeed = 5f;
    public float jumpForce = 5f;

    public float upGravityScale = 1;
    public float downGravityScale = 2;

    public int numJumpsMax = 2;
    int numJumps;

    float distanceToSide;
    bool facingRight = true;
    public bool isGrounded = true;
    public bool isRunning = false;

    bool isInvincible = false;
    public float invincibleTime = 1f;
    float invincibleTimer;

    public bool canMove = true;
    public float stunTime = 0.2f;
    float stunTimer;

    Rigidbody2D rb;
    Animator animator;

    public GameObject dustPrefab;
    public Transform dustEmitterTransform;

    public float dustTime = 5f;
    float dustTimer;

    public GameObject illuminatorPrefab;
    public GameObject magicMissilePrefab;

    public float attackTime = 0.1f;
    float attackTimer;

    public Transform projEmitterTransform;

    public Selector selector;

    public IlluminationContainer illuminationContainer;
    public float knockBackAmount = 5f;

    public int maxIlluminators;
    private int numIlluminators;
    public float illuminatorRechargeTime;
    private float illuminatorRechargeTimer;

    private bool isDead = false;
    private bool levelEnded = false;
    public LevelFinisher finisher;

    public bool canIlluminate = true;

    public int NumIlluminators
    {
        get { return numIlluminators; }
    }


    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        distanceToSide = gameObject.GetComponent<BoxCollider2D>().bounds.extents.x;

        numJumps = numJumpsMax;

        dustTimer = dustTime;
        attackTimer = attackTime;
        illuminatorRechargeTimer = illuminatorRechargeTime;
        numIlluminators = maxIlluminators;
    }

    void FixedUpdate()
    {
        updateGravityScale();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
        {
            rb.velocity = new Vector2(0, 0);
            return;
        }

        if(illuminationContainer.isDead)
        {
            Die();
        }
        
        // Handle jumping
        jumpingHandler();

        // Handle running
        runningHandler();

        // Handle invincibility
        invincibilityHandler();

        // Handle attacking
        attackHandler();

        // Dev tools
        if (Input.GetKeyDown(KeyCode.Q)) Restart();
    }

    private void myShake()
    {
        var scs = GameObject.FindObjectOfType<SimpleCameraShake>();
        if(scs != null)
        {
            scs.shakeItUp();
        }
    }

    private void attackHandler()
    {
        // Fire magic missile
        if (Input.GetMouseButton(0) && attackTimer <= 0)
        {
            Attack();
            attackTimer = attackTime;
        }
        attackTimer -= Time.deltaTime;

        // Fire illuminators
        if (Input.GetMouseButtonDown(1) && numIlluminators > 0 && canIlluminate)
        {
            Illuminate();
            numIlluminators--;
        }

        // Recharge illuminators
        if(numIlluminators < maxIlluminators)
        {
            if(illuminatorRechargeTimer <= 0)
            {
                numIlluminators++;
                illuminatorRechargeTimer = illuminatorRechargeTime;
            }
            illuminatorRechargeTimer -= Time.deltaTime;
        }
    }

    private void invincibilityHandler()
    {
        if(stunTimer <= 0 && !canMove)
        {
            canMove = true;
        }
        stunTimer -= Time.deltaTime;

        isInvincible = invincibleTimer > 0;
        invincibleTimer -= Time.deltaTime;
    }

    public void glowModify(float amount)
    {
        illuminationContainer.grow(amount);
    }

    private void spawnDust()
    {
        GameObject dust = Instantiate(dustPrefab, dustEmitterTransform.position, Quaternion.identity);
        float flip = facingRight ? -1 : 1;
        dust.transform.localScale = new Vector3(flip, 1, 1);
    }

    private void updateGravityScale()
    {
        if(rb.velocity.y > 0)
        {
            rb.gravityScale = upGravityScale;
        } else
        {
            rb.gravityScale = downGravityScale;
        }
    }

    void jumpingHandler()
    {
        if (!canMove) return;
        if (Input.GetKeyDown(KeyCode.Space) && numJumps > 0 && canMove)
        {
            Jump();
            numJumps--;
        }

        isGrounded = checkIsGrounded();
        if (isGrounded) numJumps = numJumpsMax;
    }

    void runningHandler()
    {
        if (!canMove) return;

        float horizontal = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(horizontal * runSpeed, rb.velocity.y);

        // Spawn dust if started running or flipped direction
        bool dust = false;
        if (Mathf.Abs(horizontal) > 0)
        {
            // dust = !isRunning;
            isRunning = true;
            if ((facingRight && horizontal < 0) || (!facingRight && horizontal > 0))
            {
                Flip();
                dust = true;
            }
        }
        else
        {
            isRunning = false;
        }

        if (dust && isGrounded) spawnDust();

        animator.SetBool("isRunning", isRunning);
    }

    void takeDamage(Vector2 source)
    {
        glowModify(-0.1f);
        animator.SetTrigger("damaged");

        Vector2 direction = (Vector2)transform.position - source;
        var kb = direction.normalized * knockBackAmount;
        kb.y += 13f;
        rb.velocity = kb;

        invincibleTimer = invincibleTime;
        canMove = false;
        stunTimer = stunTime;

        var fc = GameObject.FindObjectOfType<FootstepController>();

        myShake();
        if (fc != null)
        {
            fc.dead = true;
        }
    }

    void Die()
    {
        animator.SetBool("isDead", true);
        animator.SetTrigger("damaged");
        rb.velocity = new Vector2(0, 0);
        isDead = true;
        finisher.isGameOver = true;
        myShake();

        var fc = GameObject.FindObjectOfType<FootstepController>();
        if (fc != null)
        {
            fc.dead = true;
        }
    }

    void Illuminate()
    {
        animator.SetTrigger("attack");
        Instantiate(illuminatorPrefab, projEmitterTransform.position, Quaternion.identity);
    }

    void Attack()
    {
        if (selector.selectedWhiteHatHandler == null) return;
        
        animator.SetTrigger("attack");
        var mm = Instantiate(magicMissilePrefab, projEmitterTransform.position, Quaternion.identity);
        mm.layer = LayerMask.NameToLayer("Projectiles");

        var mmc = mm.GetComponent<MagicMissileController>();
        mmc.myTarget = selector.selectedWhiteHatHandler.gameObject;
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);

        var fc = GameObject.FindObjectOfType<FootstepController>();
        if (fc != null)
        {
            fc.jump = true;
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector2 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    bool checkIsGrounded()
    {
        // Cursed function
        // Debug.DrawRay(transform.position + new Vector3(-distanceToSide, 0, 0), new Vector3(0, -0.2f, 0), Color.green);
        // Debug.DrawRay(transform.position + new Vector3(distanceToSide, 0, 0), new Vector3(0, -0.2f, 0), Color.green);

        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position + new Vector3(-distanceToSide, 0, 0),
            Vector2.down, 0.3f, LayerMask.GetMask("Ground", "White Hats", "White Hats Evil"));

        RaycastHit2D hitRight = Physics2D.Raycast(transform.position + new Vector3(distanceToSide, 0, 0),
            Vector2.down, 0.3f, LayerMask.GetMask("Ground", "White Hats", "White Hats Evil"));

        return hitLeft.collider != null || hitRight.collider != null;
    }

    void Restart()
    {
        SceneManager.LoadScene(0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Evil") && !isInvincible)
        {
            takeDamage(collision.transform.position);
        }
    }
}
