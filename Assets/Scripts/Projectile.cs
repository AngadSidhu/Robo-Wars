using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public Rigidbody2D rb;
    public Transform gun;
    public bool hit;
    public bool dead;
    private bool grace = true;
    private float x;
    private float y;
    [SerializeField] private SpriteRenderer sp;
    [SerializeField] private SpriteRenderer sp2;
    [SerializeField] private bool multipleSprites;
    [SerializeField] private BoxCollider2D box;

    // Start is called before the first frame update
    void Start()
    {
        x = box.size.x;
        y = box.size.y;
        box.size = new Vector2(0f, 0f);
        rb.velocity = transform.up * speed;
        StartCoroutine(GracePeriod());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!grace)
        {
            box.size = new Vector2 (x, y);
        }
        if (!hit)
        {
            if (collision.gameObject.CompareTag("Laser"))
            {
                return;
            }
            if (collision.gameObject.CompareTag("Drone"))
            {
                Drone drone = collision.gameObject.GetComponent<Drone>();
                drone.TakeDamage(20f);
                hit = true;
            }
            if (collision.gameObject.CompareTag("Drone_Gunner"))
            {
                Drone_Gunner drone_gunner = collision.gameObject.GetComponent<Drone_Gunner>();
                drone_gunner.TakeDamage(20f);
                hit = true;
            }
            else if (collision.gameObject.CompareTag("Drone_Spawner"))
            {
                Drone_Spawner drone_spawner = collision.gameObject.GetComponent<Drone_Spawner>();
                drone_spawner.TakeDamage(20f);
                hit = true;
            }
            if (collision.gameObject.CompareTag("Enemy"))
            {
                Enemy enemy = collision.gameObject.GetComponent<Enemy>();
                enemy.TakeDamage(20f);
                hit = true;
            }
            if (collision.gameObject.CompareTag("Wall"))
            {
                hit = true;
            }
            if (collision.gameObject.CompareTag("Ground"))
            {
                hit = true;
            }
        }
        if(hit && !dead)
        {
            dead = true;
            StartCoroutine(Die());
        }
    }

    private IEnumerator GracePeriod()
    {
        yield return new WaitForSeconds(1);
        grace = false;
    }

    private IEnumerator Die()
    {
        yield return new WaitForSeconds(5f);
        if (!multipleSprites)
        {
            for (float i = 1f; i >= -0.05f; i -= 0.05f)
            {
                Color c = sp.material.color;
                c.a = i;
                sp.material.color = c;
                yield return new WaitForSeconds(0.1f);
            }
        }
        else
        {
            for (float i = 1f; i >= -0.05f; i -= 0.05f)
            {
                Color c = sp.material.color;
                Color d = sp2.material.color;
                c.a = i;
                d.a = i;
                sp.material.color = c;
                sp2.material.color = d;
                yield return new WaitForSeconds(0.1f);
            }
        }
        Destroy(gameObject);
    }
}
