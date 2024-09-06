using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Drone_Gunner : MonoBehaviour
{
    private float health = 100;
    public bool dead = false;
    public bool hitRoof = false;
    public bool canShoot = true;
    private bool inVision;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator anim;
    [SerializeField] private PolygonCollider2D poly;
    [SerializeField] private TrailRenderer tr;
    [SerializeField] private AIDestinationSetter ai;
    [SerializeField] private Transform player;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform FirePoint;
    [SerializeField] private float range;
    [SerializeField] private AudioSource shot;
    [SerializeField] private GameObject explosion;
    private Spawner spawner;

    RaycastHit2D canSeePlayer;
    public LayerMask layersToHit;

    private void Start()
    {
        spawner = GameObject.FindGameObjectWithTag("GameController").GetComponent<Spawner>();
        ai.target = GameObject.FindGameObjectWithTag("Target 2").GetComponent<Transform>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        poly = GetComponent<PolygonCollider2D>();
        tr.emitting = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (health <= 0 && !dead)
        {
            dead = true;
            StartCoroutine(Death());
        }
        if (!hitRoof && (player.position.y > transform.position.y))
        {
            rb.velocity = new Vector2(rb.velocity.x, 10f);
        }
        if (inVision && canShoot && FirePoint != null)
        {
            canShoot = false;
            StartCoroutine(Shoot());
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

    public void TakeDamage(float damage)
    {
        health -= damage;
        anim.SetTrigger("TakeDamage");
        if (health <= 0 && !dead)
        {
            dead = true;
            Instantiate(explosion, transform.position, Quaternion.identity);
            spawner.spawnScrap(3, transform.position, Quaternion.identity);
            StartCoroutine(Death());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider == null)
        {
            hitRoof = false;
        }
        else
        {
            hitRoof = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider == null)
        {
            hitRoof = true;
        }
        else
        {
            hitRoof = false;
        }
    }

    private IEnumerator Shoot()
    {
        Instantiate(bulletPrefab, FirePoint.position, FirePoint.rotation);
        shot.Play();
        yield return new WaitForSeconds(0.75f);
        canShoot = true;
    }

    private IEnumerator Death()
    {
        Life life = GameObject.FindGameObjectWithTag("Player").GetComponent<Life>();
        life.Heal(10f);
        rb.gravityScale = 10f;
        anim.SetTrigger("death");
        tr.emitting = false;
        canShoot = false;
        yield return new WaitForSeconds(0.75f);
        Destroy(gameObject);
    }
}
