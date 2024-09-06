using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Bullet : MonoBehaviour
{
    public float speed = 20f;
    public Rigidbody2D rb;
    [SerializeField] GameObject particle;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = Quaternion.Euler(0, 0, 0 + Random.Range(-7.5f, 7.5f)) * transform.position;
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
        if (collision.gameObject.CompareTag("Pickup"))
        {
            return;
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            Life life = collision.gameObject.GetComponent<Life>();
            life.TakeDamage(10f);
        }
        if (collision.gameObject.CompareTag("Grenade"))
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
