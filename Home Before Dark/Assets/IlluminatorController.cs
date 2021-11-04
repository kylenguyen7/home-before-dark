using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IlluminatorController : MonoBehaviour
{
    public float speed;
    public float rotateSpeed;
    public float startRotateSpeed = 100f;
    public float endRotateSpeed = 400f;
    public float timeRotateSpeed = 1f;

    Rigidbody2D rb;
    public Vector3 target;

    public float finishRadius = 0.1f;

    public GameObject illuminationPrefab;

    public GameObject sparklesPrefab;
    GameObject mySparkles;

    private void Start()
    {

        rotateSpeed = startRotateSpeed;

        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;

        Vector3 mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        mousePos.z += 10;
        target = mousePos;

        transform.Rotate(new Vector3(0, 0, Random.Range(0, 360f)));

        mySparkles = Instantiate(sparklesPrefab, transform);
    }

    private void Update()
    {
        rb.velocity = transform.right * speed;

        Vector3 targetVelocity = target - transform.position;
        float rotatingIndex = Vector3.Cross(targetVelocity, transform.right).z;
        rb.angularVelocity = -1 * rotatingIndex * rotateSpeed;

        rotateSpeed += (endRotateSpeed - startRotateSpeed) / timeRotateSpeed * Time.deltaTime;
        rotateSpeed = Mathf.Min(rotateSpeed, endRotateSpeed);

        if (Vector3.Distance(target, transform.position) <= finishRadius)
        {
            var ill = Instantiate(illuminationPrefab, transform.position, Quaternion.identity);

            mySparkles.transform.parent = ill.transform;

            ParticleSystem sparkles = mySparkles.GetComponent<ParticleSystem>();
            var em = sparkles.emission;
            em.enabled = false;

            Destroy(gameObject);
        }
    }
}