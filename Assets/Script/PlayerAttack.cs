using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    public Transform attackPoint;
    public float attackRadius = 1.2f;
    public int damage = 20;

    public LayerMask enemyLayer;

    public void Attack(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        Debug.Log("Player attack triggered");

        Collider2D[] enemies = Physics2D.OverlapCircleAll(
            attackPoint.position,
            attackRadius,
            enemyLayer
        );

        foreach (Collider2D enemy in enemies)
        {
            Debug.Log("Enemy hit: " + enemy.name);

            EnemyAiHealth hp = enemy.GetComponent<EnemyAiHealth>();

            if (hp != null)
            {
                hp.TakeDamage(damage);
                Debug.Log("Damage applied: " + damage);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
}