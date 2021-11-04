using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicMissileController : MonoBehaviour
{
    private float speed;
    public float startSpeed = 10f;
    public float endSpeed = 20f;
    public float timeSpeed = 1f;
    private float rotateSpeed;
    public float startRotateSpeed = 100f;
    public float endRotateSpeed = 400f;
    public float timeRotateSpeed = 1f;
    public float spread = 50f;

    public GameObject myTarget;

    Rigidbody2D rb;
    public Vector3 target;

    private void Start()
    {
        rotateSpeed = startRotateSpeed;
        speed = startSpeed;

        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;

        target = myTarget.transform.position;

        transform.right = target - transform.position;
        transform.Rotate(new Vector3(0, 0, Random.Range(-spread, spread)));
    }

    private void Update()
    {
        target = myTarget.transform.position;

        rb.velocity = transform.right * speed;

        Vector3 targetVelocity = target - transform.position;
        float rotatingIndex = Vector3.Cross(targetVelocity, transform.right).z;
        rb.angularVelocity = -1 * rotatingIndex * rotateSpeed;

        rotateSpeed += (endRotateSpeed - startRotateSpeed) / timeRotateSpeed * Time.deltaTime;
        rotateSpeed = Mathf.Min(rotateSpeed, endRotateSpeed);

        speed += (endSpeed - startSpeed) / timeSpeed * Time.deltaTime;
        speed = Mathf.Min(speed, endSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var whh = collision.gameObject.GetComponent<WhiteHatHandler>();
        if(whh != null && whh.gameObject == myTarget)
        {
            whh.masterTakeDamage(1);
            Destroy(gameObject);
        }
    }
}