using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
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
        else if (collision.gameObject.CompareTag("Rifle"))
        {
            return;
        }
        else if (collision.gameObject.CompareTag("Shotgun"))
        {
            return;
        }
        else if (collision.gameObject.CompareTag("Fusion"))
        {
            return;
        }
        else if (collision.gameObject.CompareTag("Sniper"))
        {
            return;
        }
        else if (collision.gameObject.CompareTag("Pickup"))
        {
            return;
        }
        else if (collision.gameObject.CompareTag("Drone"))
        {
            Drone drone = collision.gameObject.GetComponent<Drone>();
            drone.TakeDamage(20f);
        }
        else if (collision.gameObject.CompareTag("Drone_Spawner"))
        {
            Drone_Spawner drone_spawner = collision.gameObject.GetComponent<Drone_Spawner>();
            drone_spawner.TakeDamage(20f);
        }
        else if (collision.gameObject.CompareTag("Drone_Gunner"))
        {
            Drone_Gunner drone_gunner = collision.gameObject.GetComponent<Drone_Gunner>();
            drone_gunner.TakeDamage(20f);
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            return;
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            enemy.TakeDamage(20f);
        }
        else if (collision.gameObject.CompareTag("Grenade"))
        {
            Grenade grenade = collision.gameObject.GetComponent<Grenade>();
            grenade.hit = true;
        }
        Instantiate(particle, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    private IEnumerator Lifetime()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
