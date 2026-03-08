using UnityEngine;

public class PrincessAI : MonoBehaviour
{
    public Transform player;
    public float followDistance = 2f;
    public float speed = 2.5f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (player == null) return;

        float distance = Vector2.Distance(rb.position, player.position);

        if (distance > followDistance)
        {
            Vector2 dir = (player.position - transform.position).normalized;

            rb.MovePosition(rb.position + dir * speed * Time.fixedDeltaTime);
        }
    }
}