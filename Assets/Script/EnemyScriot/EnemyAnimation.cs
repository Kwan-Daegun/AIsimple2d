using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    private EnemyAI ai;
    private EnemyAiHealth health;
    private SpriteRenderer sr;

    public Sprite[] idleRight;
    public Sprite[] walkRight;
    public Sprite[] attackRight;
    public Sprite[] runRight;
    public Sprite[] deathRight;

    public float idleFPS = 6f;
    public float walkFPS = 8f;
    public float attackFPS = 10f;
    public float runFPS = 12f;
    public float deathFPS = 10f;

    private float timer;
    private int frame;
    private Vector2 lastDir = Vector2.right;

    private bool deathStarted;

    void Start()
    {
        ai = GetComponent<EnemyAI>();
        health = GetComponent<EnemyAiHealth>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (health != null && health.IsDead)
        {
            if (!deathStarted)
            {
                deathStarted = true;
                frame = 0;
                timer = 0f;
            }

            float frameRate = 1f / deathFPS;

            timer += Time.deltaTime;

            if (timer >= frameRate)
            {
                timer = 0f;
                frame++;
            }

            if (deathRight != null && deathRight.Length > 0)
            {
                if (frame >= deathRight.Length)
                {
                    Destroy(gameObject);
                    return;
                }

                sr.sprite = deathRight[frame];
            }

            return;
        }

        if (ai == null) return;

        Vector2 dir = Vector2.zero;

        if (ai.player != null)
            dir = (ai.player.position - transform.position).normalized;

        if (dir.sqrMagnitude > 0.01f)
            lastDir = dir;

        sr.flipX = lastDir.x < 0;

        Sprite[] current = null;
        float currentFPS = 8f;

        switch (ai.currentState)
        {
            case EnemyAI.EnemyState.Idle:
                current = idleRight;
                currentFPS = idleFPS;
                break;

            case EnemyAI.EnemyState.Patrol:
            case EnemyAI.EnemyState.Chase:
                current = walkRight;
                currentFPS = walkFPS;
                break;

            case EnemyAI.EnemyState.Attack:
                current = attackRight;
                currentFPS = attackFPS;
                break;

            case EnemyAI.EnemyState.RunAway:
                current = runRight;
                currentFPS = runFPS;
                break;
        }

        float frameRateNormal = 1f / currentFPS;

        timer += Time.deltaTime;

        if (timer >= frameRateNormal)
        {
            timer = 0f;
            frame++;
        }

        if (current != null && current.Length > 0)
            sr.sprite = current[frame % current.Length];
    }
}