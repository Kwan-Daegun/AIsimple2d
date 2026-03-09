using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    public Transform attackPoint;
    public float attackRadius = 1.2f;
    public int damage = 20;

    public float attackCooldown = 0.5f;

    public LayerMask enemyLayer;

    public AudioSource attackSound;

    private float lastAttackTime;

    private Rigidbody2D rb;
    private Vector3 attackStart;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (attackPoint != null)
            attackStart = attackPoint.localPosition;
    }

    void Update()
    {
        if (attackPoint == null || rb == null) return;

        float x = rb.velocity.x;

        if (x != 0)
        {
            bool facingLeft = x < 0;

            attackPoint.localPosition = new Vector3(
                facingLeft ? -attackStart.x : attackStart.x,
                attackStart.y,
                attackStart.z
            );
        }
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (Time.time < lastAttackTime + attackCooldown)
            return;

        lastAttackTime = Time.time;

        if (attackSound != null)
            attackSound.Play();

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