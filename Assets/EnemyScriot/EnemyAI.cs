using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float speed = 3f;
    public float stopDistance = 1.2f;

    void Update()
    {
        if (player == null) return;

        Vector2 enemyPos = transform.position;
        Vector2 playerPos = player.position;

        float distance = Vector2.Distance(enemyPos, playerPos);

        if (distance > stopDistance)
        {
            Vector2 direction = (playerPos - enemyPos).normalized;

            float moveStep = speed * Time.deltaTime;

            
            if (distance - moveStep < stopDistance)
                moveStep = distance - stopDistance;

            transform.position += (Vector3)(direction * moveStep);
        }
    }
}