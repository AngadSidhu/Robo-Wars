using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    public float speed;
    public float stoppingDistance;
    public float retreatDistance;
    private float health = 100;
    public bool dead = true;
    private bool canJump = true;

    public Transform player;
    private Transform target;
    public Rigidbody2D rb;
    public LayerMask layersToHit;

    [SerializeField] private LayerMask jumpableGround;

    MovementState state;

    private bool isTouchingWall;
    public float wallCheckDistance;
    public Transform wallCheck;
    public Transform ledgeCheck;
    [SerializeField] private Transform direction;
    [SerializeField] private Transform enemy;
    [SerializeField] private Enemy_Shooting shooting;
    [SerializeField] private BoxCollider2D box;
    [SerializeField] private bool rifleman;
    [SerializeField] private bool shotgunner;
    [SerializeField] private bool fusioneer;
    [SerializeField] private bool sniper;
    [SerializeField] private float range;
    [SerializeField] private GameObject explosion;
    private Spawner spawner;

    private Enemy_Shooting enemy_Shooting;

    private float jumpForce = 6;
    private bool canMove = true;

    [SerializeField] Animator anim;

    private enum MovementState { idle, running, jumping, falling }

    RaycastHit2D canSeePlayer;

    public bool inVision;

    private void Start()
    {
        spawner = GameObject.FindGameObjectWithTag("GameController").GetComponent<Spawner>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        target = GameObject.FindGameObjectWithTag("Target").transform;
        rb = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();

        if(rifleman)
        {
            health = 100;
        }
        else if (shotgunner)
        {
            health = 200;
        }
        else if (fusioneer)
        {
            health = 150;
        }
        else if (sniper)
        {
            health = 50;
        }
        dead = false;
    }

    private void Update()
    {
        if (health <= 0 && !dead)
        {
            dead = true;
            Instantiate(explosion, transform.position, Quaternion.identity);
            StartCoroutine(Death());
        }
        if (canMove)
        {
            target.position = new Vector2(player.position.x, transform.position.y);
            if (Vector2.Distance(transform.position, player.position) > stoppingDistance)
            {
                transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
                state = MovementState.running;
            }
            else if (Vector2.Distance(transform.position, player.position) < stoppingDistance && Vector2.Distance(transform.position, player.position) > retreatDistance)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
                state = MovementState.idle;
            }
            else if (Vector2.Distance(transform.position, player.position) < retreatDistance)
            {
                transform.position = Vector2.MoveTowards(transform.position, target.position, -speed * Time.deltaTime);
                state = MovementState.running;
            }
            if (Vector2.Distance(transform.position, player.position) < (retreatDistance/2f))
            {
                rb.velocity = new Vector2(0, jumpForce);
            }
        }
        if (isTouchingWall && canJump)
        {
            canMove = false;
            rb.velocity = new Vector2(0, jumpForce);
            StartCoroutine(JumpCD());
        }
        if (!isTouchingWall)
        {
            canMove = true;
        }
        Check();
        UpdateAnimationState();
        anim.SetInteger("state", (int)state);
    }

    private void Check()
    {
        if (enemy.transform.localScale.x <= -1f)
        {
            direction.transform.rotation = new Quaternion(0, 0, 180, 0);
            isTouchingWall = Physics2D.Raycast(wallCheck.position, direction.transform.right, wallCheckDistance, jumpableGround);
            Debug.DrawRay(wallCheck.position, direction.transform.right, Color.red);
        }

        else
        {
            direction.transform.rotation = new Quaternion(0, 0, 0, 0);
            isTouchingWall = Physics2D.Raycast(wallCheck.position, direction.transform.right, wallCheckDistance, jumpableGround);
            Debug.DrawRay(wallCheck.position, direction.transform.right, Color.red);
        }

        canSeePlayer = Physics2D.Raycast(transform.position, player.position - transform.position, range, layersToHit);

        if (canSeePlayer.collider != null)
        {
            if (canSeePlayer.collider.gameObject.CompareTag("Ground"))
            {
                Debug.DrawRay(transform.position, player.position - transform.position, Color.red);
                inVision = false;
            }
            else
            {
                Debug.DrawRay(transform.position, player.position - transform.position, Color.green);
                inVision = true;
            }
        }
        else
        {
            inVision = false;
        }
    }

    private void UpdateAnimationState()
    {
        if (rb.velocity.x > 0f)
        {
            state = MovementState.running;
        }
        else if (rb.velocity.x < 0f)
        {
            state = MovementState.running;
        }

        if (rb.velocity.y > 0.1f)
        {
            state = MovementState.jumping;
        }
        else if (rb.velocity.y < -0.1f)
        {
            state = MovementState.falling;
        }
    }

    private IEnumerator JumpCD()
    {
        yield return new WaitForSeconds(1.5f);
        canJump = false;
        yield return new WaitForSeconds(1.5f);
        canJump = true;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0 && !dead)
        {
            dead = true;
            spawner.spawnScrap(5, transform.position, Quaternion.identity);
            StartCoroutine(Death());
        }
    }

    private IEnumerator Death()
    {
        Life life = GameObject.FindGameObjectWithTag("Player").GetComponent<Life>();
        life.Heal(10f);
        shooting.canShoot = false;
        rb.gravityScale = 1f;
        anim.SetTrigger("death");
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}

