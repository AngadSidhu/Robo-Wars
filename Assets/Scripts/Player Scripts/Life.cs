using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Life : MonoBehaviour
{
    public float currentHealth = 100f;
    public float currentShield = 150f;
    public float maxHealth = 100f;
    public float maxShield = 150f;
    private float intensity;
    public bool shieldActive;
    private Rigidbody2D rb;
    public Image healthBar;
    public Image shieldBar;
    public Image screenDamage;
    public bool canTakeDamage = true;
    private IEnumerator routine;

    [SerializeField] private Volume vol;
    private ChromaticAberration cA;
    private LensDistortion lD;

    private void Start()
    {
        vol.profile.TryGet(out cA);
        vol.profile.TryGet(out lD);
        routine = shieldRegen();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Boundry"))
        {
            Die();
        }
    }

    private void Update()
    {
        if (currentHealth <= 0)
        {
            Die();
        }
        if (currentShield <= 0)
        {
            shieldActive = false;
        }
        if (currentShield > 0)
        {
            shieldActive = true;
        }
        if (currentShield > maxShield)
        {
            currentShield = maxShield;
        }
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        shieldBar.fillAmount = currentShield / 100f;
        healthBar.fillAmount = currentHealth / 100f;
        Color c = screenDamage.color;
        c.a = 1 - currentHealth / 100f;
        screenDamage.color = c;
        intensity = 1 - (currentHealth / 100);
        cA.intensity.value = intensity;
        intensity = -0.51f - (currentHealth / -200);
        lD.intensity.value = intensity;
    }

    private void Die()
    {
        rb.bodyType = RigidbodyType2D.Static;
        Invoke("DeathScene", 0f);
    }

    public void TakeDamage(float damage)
    {
        if (canTakeDamage)
        {
            StopCoroutine(routine);
            StopAllCoroutines();
            if (shieldActive)
            {
                currentShield -= damage;
                shieldBar.fillAmount = currentShield / 100f;
            }
            if (!shieldActive)
            {
                currentHealth -= damage;
                healthBar.fillAmount = currentHealth / 100f;
            }
            if (currentShield < maxShield)
            {
                StartCoroutine(shieldRegen());
            }
            if (currentHealth < maxHealth)
            {
                StartCoroutine(healthRegen());
            }
        }
    }

    public void Heal(float heal)
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += heal;
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
        }
    }

    public void Shield(float shield)
    {
        if (currentShield < maxShield)
        {
            currentShield += shield;
            if (currentShield > maxShield)
            {
                currentShield = maxShield;
            }
        }
    }

    private IEnumerator shieldRegen()
    {
        yield return new WaitForSeconds(2.5f);
        for (int i = 0; i < 100; i++) 
        {
            if (currentShield < maxShield)
            {
                currentShield++;
                yield return new WaitForSeconds(0.05f);
            }
        }
    }

    private IEnumerator healthRegen()
    {
        yield return new WaitForSeconds(2.5f);
        for (int i = 0; i < 100; i++)
        {
            if (currentHealth < maxHealth)
            {
                currentHealth++;
                yield return new WaitForSeconds(0.25f);
            }
        }
    }

    private void DeathScene()
    {
        SceneManager.LoadScene("Death");
    }
}
