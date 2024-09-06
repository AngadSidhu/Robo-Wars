using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private Animator anim;
    public Transform wallCheck;
    public Transform ledgeCheck;

    private float dirX = 0f;
    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 12f;
    private float dashingTime = 0.1f;
    private float dashingCooldown = 3f;
    private float xValue;
    private float yValue;
    private bool dashed;
    private bool inAir;
    private float x;
    private float y;
    private bool IsGrounded;
    private bool isTouchingWall;
    public float wallCheckDistance;
    private bool isTouchingLedge;
    private bool ledgeDetected;
    private bool canMove = true;
    public bool canClimb = true;
    private bool canBubble = true;
    public bool bubbled = false;
    private float bubbleTime = 5f;
    private float bubbleCooldown = 10f;

    [SerializeField] private PhysicsMaterial2D mat;

    [SerializeField] private Animator bubble_anim;

    [SerializeField] private UnityEngine.UI.Image ability1;

    [SerializeField] private UnityEngine.UI.Image ability2;

    [SerializeField] private UnityEngine.UI.Image ability2Using;

    [SerializeField] private Shooting shooting;

    [SerializeField] private CircleCollider2D circle;

    [SerializeField] private Life life;

    [SerializeField] public Transform player;

    [SerializeField] private Transform direction;

    [SerializeField] private TrailRenderer tr;

    [SerializeField] private GameObject rifle;

    [SerializeField] private GameObject shotgun;

    [SerializeField] private GameObject fusion;

    [SerializeField] private GameObject sniper;

    [SerializeField] private GameObject unnarmed;

    [SerializeField] private GameObject grenade;

    [SerializeField] private GameObject bubble;

    [SerializeField] private LayerMask jumpableGround;

    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private float acceleration = 2f;
    [SerializeField] private float deacceleration = 2f;
    [SerializeField] private float velPower = 0.2f;

    private enum MovementState { idle, running, jumping, falling }

    // Start is called before the first frame update
    private void Start()
    {
        circle = GameObject.FindGameObjectWithTag("Bubble").GetComponent<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        ability1.fillAmount = 0;
        ability2.fillAmount = 0;
        ability2Using.fillAmount = 0;
    }

    // Update is called once per frame
    private void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        xValue = (mousePosition.x - transform.position.x) / 2.5f;
        yValue = (mousePosition.y - transform.position.y) / 5f;

        if (shooting.unnarmedEquipped)
        {
            moveSpeed = 4.5f;
            jumpForce = 7.5f;
        }

        else
        {
            moveSpeed = 4f;
            jumpForce = 7f;
        }

        if (isDashing)
        {
            return;
        }

        if (IsGrounded || isTouchingWall || isTouchingLedge)
        {
            inAir = false;
            dashed = false;
        }

        if (!IsGrounded)
        {
            inAir = true;
        }

        if (canMove)
        {
            dirX = Input.GetAxisRaw("Horizontal");

            float targetspeed = dirX * moveSpeed;
            float speedDif = targetspeed - rb.velocity.x;
            float accelRate = (Mathf.Abs(targetspeed) > 0.01f) ? acceleration : deacceleration;
            float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);
            rb.AddForce(movement * Vector2.right);

            rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

            if (Input.GetButtonDown("Jump") && IsGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }

            if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            }

            if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && inAir)
            {
                StartCoroutine(Dash());
            }

            if (Input.GetKeyDown(KeyCode.E) && canBubble && !bubbled)
            {
                ActivateBubble();
            }

            if (dashed)
            {
                rb.velocity = new Vector2(((x * dashingPower) / 2f) + (2.5f * dirX), rb.velocity.y);
            }

            if (player.transform.localScale.z > 0f)
            {
                Vector2 direction = new Vector2(0, 0);
                wallCheck.transform.right = direction;
            }

            else
            {
                Vector2 direction = new Vector2(10, 10);
                wallCheck.transform.right = direction;
            }
        }

        if (!canDash)
        {
            ability1.fillAmount -= 1 / (dashingCooldown) * Time.deltaTime;

            if (ability1.fillAmount <= 0)
            {
                ability1.fillAmount = 0;
            }
        }

        if (bubbled)
        {
            ability2Using.fillAmount -= 1 / (bubbleTime) * Time.deltaTime;

            if (ability2Using.fillAmount <= 0)
            {
                ability2Using.fillAmount = 0;
            }
        }

        if (!canBubble)
        {
            ability2.fillAmount -= 1 / (bubbleCooldown - 1) * Time.deltaTime;

            if (ability2.fillAmount <= 0)
            {
                ability2.fillAmount = 0;
            }
        }

        if (IsGrounded)
        {
            //Deal with slopes, baby!

            //Using raycast with layer mask to find ground beneath
            RaycastHit2D hit = Physics2D.Raycast(this.transform.position, -Vector2.up, Mathf.Infinity, jumpableGround);
            if (hit && Mathf.Abs(hit.normal.x) > 0.1)
            {
                //Apply the opposite force against the slope force 
                //Reduce normal factor by 0.6 to deal with surface's friction
                rb.velocity = new Vector2 ((float)(rb.velocity.x - hit.normal.x * 0.6), rb.velocity.y);

                //Move Player up or down to compensate for the slope below them
                //transform.position = new Vector3(rb.position.x, rb.velocity.y + -hit.normal.x * Mathf.Abs(rb.velocity.x) * Time.deltaTime * (rb.velocity.x - hit.normal.x > 0 ? 1 : -1), 0);
            }
        }

        UpdateAnimationState();

        checkSurroundings();
    }

    private void UpdateAnimationState()
    {
        MovementState state;

        if (dirX > 0f)
        {
            state = MovementState.running;
        }
        else if (dirX < 0f)
        {
            state = MovementState.running;
        }
        else
        {
            state = MovementState.idle;

        }

        if (rb.velocity.y > 0.1f && !IsGrounded)
        {
            state = MovementState.jumping;
        }
        else if (rb.velocity.y < -0.1f && !IsGrounded)
        {
            state = MovementState.falling;
        }

        anim.SetInteger("state", (int)state);
    }

    private void checkSurroundings()
    {
        IsGrounded = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);

        if (player.transform.localScale.x == -1f)
        {
            direction.transform.rotation = new Quaternion(0, 0, 180, 0);
            isTouchingWall = Physics2D.Raycast(wallCheck.position, direction.transform.right, wallCheckDistance, jumpableGround);
            isTouchingLedge = Physics2D.Raycast(ledgeCheck.position, direction.transform.right, wallCheckDistance, jumpableGround);
        }

        else
        {
            direction.transform.rotation = new Quaternion(0, 0, 0, 0);
            isTouchingWall = Physics2D.Raycast(wallCheck.position, direction.transform.right, wallCheckDistance, jumpableGround);
            isTouchingLedge = Physics2D.Raycast(ledgeCheck.position, direction.transform.right, wallCheckDistance, jumpableGround);
        }

        if (isTouchingWall && !isTouchingLedge && !ledgeDetected && canClimb)
        {
            Mantle();
            ledgeDetected = false;
        }
    }

    private IEnumerator Dash()
    {
        x = xValue;
        y = yValue;

        if (x > 3)
        {
            x = 3;
        }
        if (x < -3)
        {
            x = -3;
        }
        if (y > 3)
        {
            y = 3;
        }
        if (y < -3)
        {
            y = -3;
        }

        canDash = false;
        isDashing = true;
        ability1.fillAmount = 1f;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(x * dashingPower, y * dashingPower);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rb.velocity = new Vector2(x * dashingPower * -5f, y * dashingPower);
        rb.gravityScale = originalGravity;
        isDashing = false;
        dashed = true;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    private void Mantle()
    {
        canMove = false;
        rb.velocity = new Vector2(0f, 0f);
        rb.gravityScale = 0f;
        shooting.canSwitch = false;
        unnarmed.SetActive(true);
        shooting.stop = true;
        anim.SetTrigger("mantle");
        rb.velocity = new Vector2(0f, jumpForce);
        rb.gravityScale = 1f;
        canMove = true;
        StartCoroutine(Disable());
    }

    private IEnumerator Disable()
    {
        rifle.SetActive(false);
        shotgun.SetActive(false);
        fusion.SetActive(false);
        sniper.SetActive(false);
        grenade.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        if (shooting.rifleEquipped)
        {
            unnarmed.SetActive(false);
            rifle.SetActive(true);
        }
        if (shooting.shotgunEquipped)
        {
            unnarmed.SetActive(false);
            shotgun.SetActive(true);
        }
        if (shooting.fusionEquipped)
        {
            unnarmed.SetActive(false);
            fusion.SetActive(true);
        }
        if (shooting.sniperEquipped)
        {
            unnarmed.SetActive(false);
            sniper.SetActive(true);
        }
        if (shooting.grenadeEquipped)
        {
            unnarmed.SetActive(false);
            grenade.SetActive(true);
        }
        shooting.canSwitch = true;
        shooting.stop = false;
    }

    private void ActivateBubble()
    {
        bubbled = true;
        bubble_anim.SetBool("bubbled", true);
        shooting.canFire = false;
        circle.radius = 0.3895557f;
        ability2Using.fillAmount = 1f;
        StartCoroutine(DeactivateBubble());
    }

    private IEnumerator DeactivateBubble()
    {
        life.canTakeDamage = false;
        yield return new WaitForSeconds(bubbleTime);
        ability2.fillAmount = 1f;
        canBubble = false;
        bubbled = false;
        bubble_anim.SetBool("bubbled", false);
        circle.radius = 0f;
        shooting.canFire = true;
        life.canTakeDamage = true;
        StartCoroutine(BubbleCD());
    }

    private IEnumerator BubbleCD()
    {
        yield return new WaitForSeconds(bubbleCooldown);
        canBubble = true;
    }


}