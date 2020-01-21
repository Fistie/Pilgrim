using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyKnockback : MonoBehaviour
{

    [SerializeField] string otherTag;
    [SerializeField] float knockTime;
    [SerializeField] float knockStrength;

    [SerializeField] AnimatorController anim;
    [SerializeField] StateMachine myState;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(otherTag) && other.isTrigger)
        {
            Rigidbody2D temp = other.GetComponentInParent<Rigidbody2D>();
            if (temp)
            {
                myState.ChangeState(GenericState.stun);
                anim.SetAnimParameter("stunned", true);
                Vector2 direction = other.transform.position - transform.position;
                temp.DOMove((Vector2)other.transform.position +
                    (direction.normalized * knockStrength), knockTime);
                StartCoroutine(KnockAnimCo());
            }
        }
    }

    public IEnumerator KnockAnimCo()
    {
        yield return new WaitForSeconds(knockTime);
        anim.SetAnimParameter("stunned", false);
        myState.ChangeState(GenericState.idle);
    }
}