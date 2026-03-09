using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    public int CurrentHealth
    {
        get { return currentHealth; }
    }

    public bool IsDead { get; private set; }

    public HealthBar healthBar;

    public float hitFlashTime = 0.15f;

    private SpriteRenderer sr;
    private Color originalColor;

    void Start()
    {
        currentHealth = maxHealth;

        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;

        if (healthBar != null)
            healthBar.SetHealth(currentHealth, maxHealth);
    }
    public void Heal(int amount)
    {
        if (IsDead) return;

        currentHealth += amount;

        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        if (healthBar != null)
            healthBar.SetHealth(currentHealth, maxHealth);
    }
    public void TakeDamage(int damage)
    {
        if (IsDead) return;

        currentHealth -= damage;

        if (healthBar != null)
            healthBar.SetHealth(currentHealth, maxHealth);

        Debug.Log("Player took " + damage + " damage. Current health: " + currentHealth);

        StartCoroutine(HitFlash());

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    IEnumerator HitFlash()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(hitFlashTime);
        sr.color = originalColor;
    }

    void Die()
    {
        IsDead = true;
        Debug.Log("Player died");
    }
}