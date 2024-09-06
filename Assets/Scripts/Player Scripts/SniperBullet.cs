using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class SniperBullet : MonoBehaviour
{
    public float speed = 30f;
    public Rigidbody2D rb;
    [SerializeField] GameObject particle;
    private float damage = 300f;

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
            drone.TakeDamage(damage);
            Instantiate(particle, transform.position, transform.rotation);
        }
        if (collision.gameObject.CompareTag("Drone_Spawner"))
        {
            Drone_Spawner drone_spawner = collision.gameObject.GetComponent<Drone_Spawner>();
            drone_spawner.TakeDamage(damage);
            Instantiate(particle, transform.position, transform.rotation);
        }
        if (collision.gameObject.CompareTag("Drone_Gunner"))
        {
            Drone_Gunner drone_gunner = collision.gameObject.GetComponent<Drone_Gunner>();
            drone_gunner.TakeDamage(damage);
            Instantiate(particle, transform.position, transform.rotation);
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            enemy.TakeDamage(damage);
            Instantiate(particle, transform.position, transform.rotation);
        }
        if (collision.gameObject.CompareTag("Grenade"))
        {
            Grenade grenade = collision.gameObject.GetComponent<Grenade>();
            grenade.hit = true;
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            return;
        }
        if (collision.gameObject.CompareTag("Ground"))
        {
            Instantiate(particle, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    private IEnumerator Lifetime()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }
}
