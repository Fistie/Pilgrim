using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFollow : Movement
{

    [SerializeField] private string targetTag;
    private Transform target;
    [SerializeField] private float chaseRadius;
    [SerializeField] private float attackRadius;
    private float targetDistance;
    [SerializeField] private StateMachine myState;
    [SerializeField] private AnimatorController anim;

    private Vector2 tempMovement = Vector2.down;
    private Vector2 facingDirection = Vector2.down;

    // Start is called before the first frame update
    void Start()
    {
        myState.ChangeState(GenericState.idle);
        target = GameObject.FindGameObjectWithTag(targetTag).GetComponent<Transform>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        targetDistance = Vector3.Distance(transform.position, target.position);

        if (targetDistance < chaseRadius && targetDistance > attackRadius)
        {
            if (myState.myState == GenericState.idle || myState.myState == GenericState.walk
                && myState.myState != GenericState.stun)
            {
                anim.SetAnimParameter("wakeup", true);
                SetState (GenericState.walk);
                Vector2 temp = (Vector2)(target.position - transform.position);
                //Vector3 temp = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
                Motion(temp);
            }

        }
        else if (targetDistance > chaseRadius)
        {
            myRigidbody.velocity = Vector2.zero;
            anim.SetAnimParameter("wakeup", false);
            anim.SetAnimParameter("stunned", false);
            SetState(GenericState.idle);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, chaseRadius);
    }

        void SetState(GenericState newState)
    {
        myState.ChangeState(newState);
    }
}