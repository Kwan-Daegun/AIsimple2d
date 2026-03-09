using UnityEngine;
using System.Collections;

public class EnemyAiHealth : MonoBehaviour
{
    public int maxHealth = 50;
    private int currentHealth;

    public HealthBar healthBar;

    public float hitFlashTime = 0.15f;

    private bool dead = false;

    private SpriteRenderer sr;
    private Color originalColor;

    public int CurrentHealth
    {
        get { return currentHealth; }
    }

    public bool IsDead
    {
        get { return dead; }
    }

    void Start()
    {
        currentHealth = maxHealth;

        sr = GetComponent<SpriteRenderer>();
        if (sr != null)
            originalColor = sr.color;

        if (healthBar != null)
            healthBar.SetHealth(currentHealth, maxHealth);
    }

    public void TakeDamage(int damage)
    {
        if (dead) return;

        currentHealth -= damage;

        if (healthBar != null)
            healthBar.SetHealth(currentHealth, maxHealth);

        Debug.Log(gameObject.name + " took " + damage + " damage. Current health: " + currentHealth);

        StartCoroutine(HitFlash());

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    IEnumerator HitFlash()
    {
        if (sr == null) yield break;

        sr.color = Color.red;
        yield return new WaitForSeconds(hitFlashTime);
        sr.color = originalColor;
    }

    void Die()
    {
        dead = true;
    }
}