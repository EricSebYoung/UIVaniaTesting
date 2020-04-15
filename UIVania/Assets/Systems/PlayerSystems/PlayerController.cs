using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Components
    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D coll;
    private InputsController inputs;
    private AbilitiesController abilities;

    //States
    private enum State {idle, running, jumping, falling, hurt};
    private State state = State.idle;
    private bool paused = false;
    private bool facingRight = true;

    [SerializeField] private LayerMask ground;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float hHurtForce = 5f;
    [SerializeField] private float vHurtForce = 8f;

    //Sounds
    [SerializeField] private AudioSource footstepSound;
    [SerializeField] private AudioSource jumpSound;
    [SerializeField] private AudioSource healSound;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        inputs = GetComponent<InputsController>();
        abilities = GetComponent<AbilitiesController>();
    }

    private void Update()
    {
        if (!PauseScript.GamePaused)
        {
            if (paused)
            {
                Resume();
                paused = false;
            }
            //If Game is not Paused
            if (state != State.hurt)
            {
                HandleInput();
            }
            VelocityState();
            anim.SetInteger("state", (int)state); //Sets animation based on state
        } else
        {
            if (!paused)
            {
                paused = true;
                Pause();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "CollectableItem")
        {
            Destroy(collision.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "DamageObject")
        {
            Damage damageObject = collision.gameObject.GetComponent<Damage>();
            state = State.hurt;
            if(collision.gameObject.transform.position.x > transform.position.x)
            {
                //Damage from Right, Move Left
                rb.velocity = new Vector2(-hHurtForce, vHurtForce);
            } else
            {
                //Damage from Left, Move Right
                rb.velocity = new Vector2(hHurtForce, vHurtForce);
            }
            Damage(damageObject.DoDamage());
        }
    }

    private void HandleInput()
    {
        float HDirection = inputs.HDirection;
        bool jump = inputs.jump;
        bool fireAbility = inputs.fireAbility;
        bool cycleAbilityLeft = inputs.cycleAbilityLeft;
        bool cycleAbilityRight = inputs.cycleAbilityRight;

        //Move Left
        if (HDirection < 0)
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            if (facingRight)
            {
                Flip();
            }
        }

        //Move Right
        else if ((HDirection > 0))
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            if (!facingRight)
            {
                Flip();
            }
        }

        //Jump
        if (jump)
        {
            RaycastHit2D hit = Physics2D.Raycast(rb.position, Vector2.down, 1.3f, ground);
            if (hit.collider != null)
                Jump();
        }

        //Use Ability
        if (fireAbility)
        {
            abilities.FireAbility();
        }

        //Cycle Ability Left
        if (cycleAbilityLeft)
        {
            abilities.CycleEquip(false);
        }

        //Cycle Ability Right
        else if (cycleAbilityRight)
        {
            abilities.CycleEquip(true);
        }
    }
    private void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    private void VelocityState()
    {
        if(state == State.jumping)
        {
            if (rb.velocity.y < 0.1f)
            {
                state = State.falling;
            }
        }

        else if(state == State.falling)
        {
            if(coll.IsTouchingLayers(ground))
            {
                state = State.idle;
                Footstep();
            }
        }

        else if(state == State.hurt)
        {
            if (Mathf.Abs(rb.velocity.x) < 0.1f)
            {
                state = State.idle;
            }
        }

        else if (!coll.IsTouchingLayers(ground))
        {
            state = State.falling;
        }

        else if(Mathf.Abs(rb.velocity.x) > 0.1f)
        {
            //Moving
            state = State.running;
        } else
        {
            state = State.idle;
        }
    }

    private void Footstep()
    {
        footstepSound.Play();
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        state = State.jumping;
        if (!jumpSound.isPlaying)
        {
            jumpSound.Play();
        }
    }


    public void Heal (int amountHealed)
    {
        HeartsVisual.playerHeartsStatic.Heal(amountHealed);
    }

    private void Damage(int damageTaken)
    {
        HeartsVisual.playerHeartsStatic.Damage(damageTaken);
    }

    private void Death()
    {
        Destroy(this.gameObject);
    }

    private Vector2 pausedVelocity;
    private float pausedAngularVelocity;

    private void Pause()
    {
        pausedVelocity = rb.velocity;
        pausedAngularVelocity = rb.angularVelocity;
        rb.isKinematic = true;

        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;

        anim.enabled = false;
    }

    private void Resume()
    {
        rb.isKinematic = false;
        rb.velocity = pausedVelocity;
        rb.angularVelocity = pausedAngularVelocity;

        anim.enabled = true;
    }
}
