using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Enemy_Sniper_Bullet : MonoBehaviour
{
    public float speed = 20f;
    public Rigidbody2D rb;
    [SerializeField] GameObject particle;

    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.up * speed;
        StartCoroutine(Lifetime());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Laser"))
        {
            return;
        }
        if (collision.gameObject.CompareTag("Rifle"))
        {
            return;
        }
        if (collision.gameObject.CompareTag("Shotgun"))
        {
            return;
        }
        if (collision.gameObject.CompareTag("Fusion"))
        {
            return;
        }
        if (collision.gameObject.CompareTag("Sniper"))
        {
            return;
        }
        if (collision.gameObject.CompareTag("Drone"))
        {
            Drone drone = collision.gameObject.GetComponent<Drone>();
            drone.TakeDamage(50f);
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            Life life = collision.gameObject.GetComponent<Life>();
            life.TakeDamage(50f);
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            return;
        }
        if (collision.gameObject.CompareTag("Bubble"))
        {
            Instantiate(particle, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        Instantiate(particle, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    private IEnumerator Lifetime()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }
}
