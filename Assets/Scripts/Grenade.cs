using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    [SerializeField] private GameObject explosion;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private CapsuleCollider2D cap;
    public Transform pos;
    private float time = 5f;
    public float speed = 5f;
    public float radius = 5f;
    public bool hit;
    private bool smacked = false;

    public LayerMask layersToHit;

    RaycastHit2D canSeePlayer;

    public bool inVision;

    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.up * speed;
        cap.size = new Vector2(0f, 0f);
        StartCoroutine(Timer());
    }

    private void Update()
    {
        if (hit)
        {
            hit = false;
            StopAllCoroutines();
            StartCoroutine(Explode());
        }
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(time);
        Instantiate(explosion, pos.position, pos.rotation);
        cap.size = new Vector2(radius, radius);
        yield return new WaitForSeconds(0.25f);
        Destroy(gameObject);
    }

    private IEnumerator Explode()
    {
        Instantiate(explosion, pos.position, pos.rotation);
        cap.size = new Vector2(radius, radius);
        yield return new WaitForSeconds(0.25f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Transform target = collision.GetComponent<Transform>();

        canSeePlayer = Physics2D.Raycast(transform.position, target.position - transform.position, radius, layersToHit);

        if (canSeePlayer.collider != null)
        {
            if (canSeePlayer.collider.gameObject.CompareTag("Ground"))
            {
                Debug.DrawRay(transform.position, target.position - transform.position, Color.red);
                inVision = false;
            }
            else
            {
                Debug.DrawRay(transform.position, target.position - transform.position, Color.green);
                inVision = true;
            }
        }
        else
        {
            inVision = false;
        }

        if (inVision)
        {
            if (collision.gameObject.CompareTag("Drone"))
            {
                Drone drone = collision.gameObject.GetComponent<Drone>();
                drone.TakeDamage(200f);
            }
            if (collision.gameObject.CompareTag("Drone_Spawner"))
            {
                Drone_Spawner drone_spawner = collision.gameObject.GetComponent<Drone_Spawner>();
                drone_spawner.TakeDamage(200f);
            }
            if (collision.gameObject.CompareTag("Drone_Gunner"))
            {
                Drone_Gunner drone_gunner = collision.gameObject.GetComponent<Drone_Gunner>();
                drone_gunner.TakeDamage(200f);
            }
            if (collision.gameObject.CompareTag("Player"))
            {
                Life life = collision.gameObject.GetComponent<Life>();
                life.TakeDamage(50f);
            }
            if (collision.gameObject.CompareTag("Enemy"))
            {
                Enemy enemy = collision.gameObject.GetComponent<Enemy>();
                enemy.TakeDamage(200f);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!smacked)
        {
            if (collision.gameObject.CompareTag("Laser"))
            {
                return;
            }
            if (collision.gameObject.CompareTag("Drone"))
            {
                Drone drone = collision.gameObject.GetComponent<Drone>();
                drone.TakeDamage(20f);
                smacked = true;
            }
            if (collision.gameObject.CompareTag("Drone_Gunner"))
            {
                Drone_Gunner drone_gunner = collision.gameObject.GetComponent<Drone_Gunner>();
                drone_gunner.TakeDamage(20f);
                smacked = true;
            }
            else if (collision.gameObject.CompareTag("Drone_Spawner"))
            {
                Drone_Spawner drone_spawner = collision.gameObject.GetComponent<Drone_Spawner>();
                drone_spawner.TakeDamage(20f);
                smacked = true;
            }
            if (collision.gameObject.CompareTag("Enemy"))
            {
                Enemy enemy = collision.gameObject.GetComponent<Enemy>();
                enemy.TakeDamage(20f);
                smacked = true;
            }
            if (collision.gameObject.CompareTag("Wall"))
            {
                smacked = true;
            }
            if (collision.gameObject.CompareTag("Ground"))
            {
                smacked = true;
            }
        }
    }
}
