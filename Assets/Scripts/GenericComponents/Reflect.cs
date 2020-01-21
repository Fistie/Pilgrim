using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reflect : MonoBehaviour
{
    [SerializeField] string otherTag;
    [SerializeField] Vector3 oldVelocity;
    [SerializeField] Rigidbody2D thisProjectile;
    [SerializeField] public float moveSpeed;
    [SerializeField] Vector2 moveDir = Vector2.zero;

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag(otherTag))
        {
            Vector2 projContact = other.contacts[0].normal;
            moveDir = Vector2.Reflect(projContact,thisProjectile.velocity).normalized;

            thisProjectile.velocity = moveDir * moveSpeed;
        }
    }
}
