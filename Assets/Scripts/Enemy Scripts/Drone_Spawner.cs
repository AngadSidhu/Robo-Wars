using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Drone_Spawner : MonoBehaviour
{
    private float health = 300;
    public bool dead = false;
    public bool hitRoof = false;
    private bool spawning = false;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator anim;
    [SerializeField] private BoxCollider2D box;
    [SerializeField] private TrailRenderer tr;
    [SerializeField] private AIDestinationSetter ai;
    [SerializeField] private Transform player;
    [SerializeField] private Transform SpawnPoint;
    [SerializeField] private GameObject drone;
    [SerializeField] private float spawntime;
    [SerializeField] private GameObject explosion;
    private Spawner spawner;
    Vector3 expo;

    private void Start()
    {
        spawner = GameObject.FindGameObjectWithTag("GameController").GetComponent<Spawner>();
        ai.target = GameObject.FindGameObjectWithTag("Target 2").GetComponent<Transform>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
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
        
        if (!spawning)
        {
            spawning = true;
            StartCoroutine(Spawn());
        }
        AstarPath.active.UpdateGraphs(box.bounds);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0 && !dead)
        {
            dead = true;
            for (int i = 0; i < 3; i++) 
            {
                expo = new Vector3 (transform.position.x + Random.Range(-1f,1f), transform.position.y + Random.Range(-1f, 1f), transform.position.z);
                Instantiate(explosion, expo, Quaternion.identity);
            }
            spawner.spawnScrap(10, transform.position, Quaternion.identity);
            StartCoroutine(Death());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
            hitRoof = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
            hitRoof = false;
    }

    private IEnumerator Death()
    {
        Life life = GameObject.FindGameObjectWithTag("Player").GetComponent<Life>();
        life.Heal(10f);
        rb.gravityScale = 10f;
        anim.SetTrigger("death");
        tr.emitting = false;
        box.enabled = false;
        yield return new WaitForSeconds(0.75f);
        Destroy(gameObject);
    }

    private IEnumerator Spawn()
    {
        Instantiate(drone, SpawnPoint.position, SpawnPoint.rotation);
        yield return new WaitForSeconds(spawntime);
        spawning = false;
    }

}
