using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public float health;
    public float shielding;
    public bool heal;
    public bool shield;
    private Rigidbody2D rb;
    private Spawner spawner;

    private void Start()
    {
        spawner = GameObject.FindGameObjectWithTag("GameController").GetComponent<Spawner>();
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(Lifetime());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.CompareTag("Player"))
            {
                Life life = collision.gameObject.GetComponent<Life>();
                if (heal && life.currentHealth < life.maxHealth)
                {
                    life.Heal(health);
                    spawner.pickupNum--;
                    Destroy(gameObject);
                }
                else if (shield && life.currentShield < life.maxShield)
                {
                    life.Shield(shielding);
                    spawner.pickupNum--;
                    Destroy(gameObject);
                }
            }
            if (collision.CompareTag("Ground"))
            {
                rb.bodyType = RigidbodyType2D.Static;
            }
        }
    }

    private IEnumerator Lifetime()
    {
        yield return new WaitForSeconds(60f);
        spawner.pickupNum--;
        Destroy(gameObject);
    }
}
