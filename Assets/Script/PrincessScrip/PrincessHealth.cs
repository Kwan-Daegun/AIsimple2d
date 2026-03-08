using UnityEngine;

public class PrincessHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    public HealthBar healthBar;

    void Start()
    {
        currentHealth = maxHealth;

        if (healthBar != null)
            healthBar.SetHealth(currentHealth, maxHealth);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (healthBar != null)
            healthBar.SetHealth(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Debug.Log("Princess died");
            Destroy(gameObject);
        }
    }
}