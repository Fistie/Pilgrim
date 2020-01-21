using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Movement
{
    [SerializeField] private AnimatorController anim;
    [SerializeField] private StateMachine myState;
    [SerializeField] private float WeaponAttackDuration;
    [SerializeField] private ReceiveItem myItem;
    [SerializeField] private GenericAbility currentAbility;

    [SerializeField] private BoxCollider2D hurtBox;
    [SerializeField] private bool hasDashed;
    [SerializeField] private SpriteRenderer mySprite;
    [SerializeField] private GenericAbility currentPhase;
    [SerializeField] private bool hasPhased;

    private Vector2 tempMovement = Vector2.down;
    private Vector2 facingDirection = Vector2.down;

    // Start is called before the first frame update
    void Start()
    {
        myState.ChangeState(GenericState.idle);
    }

    // Update is called once per frame
    void Update()
    {
        if (myState.myState == GenericState.receiveItem)
        {
            if (Input.GetButtonDown("Check"))
            {
                myState.ChangeState(GenericState.idle);
                anim.SetAnimParameter("receiveItem", false);
                myItem.ChangeSpriteState();
            }
            return;
        }
        GetInput();
        SetAnimation();
    }


    void SetState(GenericState newState)
    {
        myState.ChangeState(newState);
    }


    void GetInput()
    {
        if (Input.GetButtonDown("Weapon Attack") && myState.myState != GenericState.ability && myState.myState != GenericState.stun)
        {
            StartCoroutine(WeaponCo());
            tempMovement = Vector2.zero;
            Motion(tempMovement);
        }
        else if (Input.GetButtonDown("Dash") && hasDashed != true)
        {
            if (currentAbility)
            {
                StartCoroutine(DashCo(currentAbility.duration));
            }
        }
        else if (Input.GetButtonDown("Phase") && hasPhased != true)
        {
            if (currentPhase)
            {
                StartCoroutine(PhaseCo(currentPhase.duration));
            }
        }
        else if (myState.myState != GenericState.attack)
        {
            tempMovement.x = Input.GetAxisRaw("Horizontal");
            tempMovement.y = Input.GetAxisRaw("Vertical");
            Motion(tempMovement);
        }
        else
        {
            tempMovement = Vector2.zero;
            Motion(tempMovement);
        }
    }

    void SetAnimation()
    {
        //returns to walk if not stunned or dashing
        if (tempMovement.magnitude > 0 && myState.myState != GenericState.stun && myState.myState != GenericState.ability)
        {
            anim.SetAnimParameter("moveX", Mathf.Round(tempMovement.x));
            anim.SetAnimParameter("moveY", Mathf.Round(tempMovement.y));
            anim.SetAnimParameter("moving", true);
            SetState(GenericState.walk);
            facingDirection = tempMovement;
        }
        else
        {
            //Returns state to idle if not attacking, stunned, or dashing
            anim.SetAnimParameter("moving", false);
            if (myState.myState != GenericState.attack && myState.myState != GenericState.stun && myState.myState != GenericState.ability)
            {
                SetState(GenericState.idle);
            }
        }
    }

    public IEnumerator WeaponCo()
    {
        //TODO **NEEDS ANIMATIONS**
        myState.ChangeState(GenericState.attack);
        anim.SetAnimParameter("attacking", true);
        yield return new WaitForSeconds(WeaponAttackDuration);
        myState.ChangeState(GenericState.idle);
        anim.SetAnimParameter("attacking", false);
    }

    //
    public IEnumerator DashCo(float abilityDuration)
    {
        //TODO **NEEDS ANIMATIONS** checks Dash cooldown, if not on CD, changes color, disables player Hurtbox,
        //changes state, Dashes, waits for Duration, resets state, enables Hurtbox, returns color, begins hasDashCo.
        hasDashed = true;
        hurtBox.enabled = false;
        mySprite.color = Color.black;
        myState.ChangeState(GenericState.ability);
        currentAbility.Ability(transform.position, facingDirection, anim.anim, myRigidbody);
        yield return new WaitForSeconds(abilityDuration);
        myState.ChangeState(GenericState.idle);
        mySprite.color = Color.white;
        hurtBox.enabled = true;
        StartCoroutine(HasDashedCo());
    }

    public IEnumerator PhaseCo(float abilityDuration)
    {
        //TODO **NEEDS ANIMATIONS, NEEDS SCREEN EFFECTS, NEEDS BALANCING** RECALL EFFECT?
        hasPhased = true;
        hurtBox.enabled = false;
        mySprite.color = Color.black;
        currentPhase.Ability(transform.position, facingDirection, anim.anim, myRigidbody);
        yield return new WaitForSeconds(abilityDuration);
        mySprite.color = Color.white;
        hurtBox.enabled = true;
        StartCoroutine(HasPhasedCo());
    }

    public IEnumerator HasDashedCo()
    {
        //waits for cooldown, sets hasDashed bool to false
        yield return new WaitForSeconds(.7f);
        hasDashed = false;
    }

    public IEnumerator HasPhasedCo()
    {
        yield return new WaitForSeconds(2f);
        hasPhased = false;
    }
}