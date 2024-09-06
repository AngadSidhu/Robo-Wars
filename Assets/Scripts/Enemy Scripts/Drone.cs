using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour
{
    private float health = 20;
    public bool canAttack = true;
    private bool dead = false;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator anim;
    [SerializeField] private BoxCollider2D box;
    [SerializeField] private TrailRenderer tr;
    [SerializeField] private AIDestinationSetter ai;
    [SerializeField] private float damage;
    [SerializeField] private float heal;
    [SerializeField] private GameObject explosion;
    private Spawner spawner;

    private void Start()
    {
        spawner = GameObject.FindGameObjectWithTag("GameController").GetComponent<Spawner>();
        ai.target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
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
            Instantiate(explosion, transform.position, Quaternion.identity);
            spawner.spawnScrap(1, transform.position, Quaternion.identity);
            StartCoroutine(Death());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Life life = collision.gameObject.GetComponent<Life>();
            if (life != null && canAttack)
            {
                life.TakeDamage(damage);
                StartCoroutine(Cooldown());
            }
        }
        else
        {
            return;
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
    }

    private IEnumerator Cooldown()
    {
        canAttack = false;
        rb.bodyType = RigidbodyType2D.Static;
        box.isTrigger = true;
        anim.SetTrigger("hit");
        yield return new WaitForSeconds(0.5f);
        box.isTrigger = false;
        yield return new WaitForSeconds(1.5f);
        rb.bodyType = RigidbodyType2D.Dynamic;
        canAttack = true;
    }

    private IEnumerator Death() 
    {
        Life life = GameObject.FindGameObjectWithTag("Player").GetComponent<Life>();
        life.Heal(heal);
        rb.gravityScale = 10f;
        anim.SetTrigger("death");
        tr.emitting = false;
        canAttack = false;
        box.enabled = false;
        yield return new WaitForSeconds(0.75f);
        Destroy(gameObject);
    }
}
