using UnityEngine;

public class EnemyAiHealth : MonoBehaviour
{
    public int maxHealth = 50;
    private int currentHealth;

    public HealthBar healthBar;

    private bool dead = false;

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

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    void Die()
    {
        dead = true;
    }
}