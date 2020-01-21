using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Movement Stuff")]

    public float moveSpeed;
    public float reflectSpeed;

    [Header("Lifetime")]
    public float lifeTime;
    private float lifeTimeSeconds;
    public Rigidbody2D myRigidbody;

    [SerializeField] string otherTag;
    [SerializeField] Vector2 moveDir = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        lifeTimeSeconds = lifeTime;
    }

    // Update is called once per frame
    void Update()
    {

        lifeTimeSeconds -= Time.deltaTime;
        if (lifeTimeSeconds <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void Launch(Vector2 initialVel)
    {
        myRigidbody.velocity = initialVel.normalized * moveSpeed;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Enemy") || other.CompareTag("Projectile"))
        {
            Destroy(this.gameObject);
        }
    }

    //Separate to Reflectable-Projectiles
    public void OnCollisionEnter2D(Collision2D other)
    {
        gameObject.layer = 13;
    }
}