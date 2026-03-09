using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum EnemyState
    {
        Idle,
        Patrol,
        Chase,
        Attack,
        RunAway
    }

    public EnemyState currentState = EnemyState.Idle;

    public Transform player;
    public Transform princess;

    private Transform currentTarget;
    private EnemyAiHealth health;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    public float speed = 3f;
    public float chaseDistance = 6f;
    public float attackDistance = 1.5f;
    public float chaseSpeed = 4.5f;

    public int damage = 10;
    public float attackCooldown = 1f;

    public int runAwayHealthThreshold = 10;

    public Transform[] patrolPoints;
    public float patrolDistance = 3f;
    public float idleTime = 2f;

    public AudioSource attackSound;

    private float lastAttackTime;
    private int patrolIndex = 0;
    private float idleTimer;

    private Vector2 runDirection;
    private bool runDirectionSet = false;

    void Start()
    {
        health = GetComponent<EnemyAiHealth>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        princess = GameObject.FindGameObjectWithTag("Princess")?.transform;

        if (patrolPoints == null || patrolPoints.Length == 0)
        {
            patrolPoints = new Transform[2];

            GameObject left = new GameObject("AutoPatrolLeft");
            GameObject right = new GameObject("AutoPatrolRight");

            left.transform.position = transform.position + Vector3.left * patrolDistance;
            right.transform.position = transform.position + Vector3.right * patrolDistance;

            patrolPoints[0] = left.transform;
            patrolPoints[1] = right.transform;
        }
    }

    void Update()
    {
        if (player == null && princess == null) return;

        if (health != null && health.CurrentHealth <= runAwayHealthThreshold)
        {
            if (currentState != EnemyState.RunAway)
            {
                currentState = EnemyState.RunAway;
                runDirection = Random.insideUnitCircle.normalized;
                runDirectionSet = true;
            }
        }
        else
        {
            ChooseTarget();

            if (currentTarget == null) return;

            float distance = Vector2.Distance(rb.position, currentTarget.position);

            if (distance <= attackDistance)
                currentState = EnemyState.Attack;
            else if (distance <= chaseDistance)
                currentState = EnemyState.Chase;
            else if (patrolPoints.Length > 0)
                currentState = EnemyState.Patrol;
            else
                currentState = EnemyState.Idle;
        }

        switch (currentState)
        {
            case EnemyState.Idle:
                Idle();
                break;

            case EnemyState.Patrol:
                Patrol();
                break;

            case EnemyState.Chase:
                Chase();
                break;

            case EnemyState.Attack:
                Attack();
                break;

            case EnemyState.RunAway:
                RunAway();
                break;
        }
    }

    void ChooseTarget()
    {
        if (player == null && princess == null)
        {
            currentTarget = null;
            return;
        }

        if (player != null && princess == null)
        {
            currentTarget = player;
            return;
        }

        if (player == null && princess != null)
        {
            currentTarget = princess;
            return;
        }

        float playerDistance = Vector2.Distance(rb.position, player.position);
        float princessDistance = Vector2.Distance(rb.position, princess.position);

        if (Mathf.Abs(playerDistance - princessDistance) < 0.3f)
            currentTarget = Random.value > 0.5f ? player : princess;
        else
            currentTarget = playerDistance < princessDistance ? player : princess;
    }

    void Idle()
    {
        idleTimer += Time.deltaTime;

        if (idleTimer >= idleTime)
        {
            idleTimer = 0f;

            if (patrolPoints.Length > 0)
                currentState = EnemyState.Patrol;
        }
    }

    void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        Transform target = patrolPoints[patrolIndex];

        Vector2 dir = (target.position - transform.position).normalized;
        FaceDirection(dir);

        rb.MovePosition(
            Vector2.MoveTowards(
                rb.position,
                target.position,
                speed * Time.deltaTime
            )
        );

        float distance = Vector2.Distance(rb.position, target.position);

        if (distance < 0.1f)
        {
            patrolIndex = (patrolIndex + 1) % patrolPoints.Length;
            currentState = EnemyState.Idle;
        }
    }

    void Chase()
    {
        float distance = Vector2.Distance(rb.position, currentTarget.position);

        if (distance <= attackDistance)
            return;

        Vector2 dir = (currentTarget.position - transform.position).normalized;

        FaceDirection(dir);

        rb.MovePosition(rb.position + dir * chaseSpeed * Time.deltaTime);
    }

    void Attack()
    {
        if (Time.time - lastAttackTime < attackCooldown)
            return;

        lastAttackTime = Time.time;

        if (attackSound != null)
            attackSound.Play();

        PlayerHealth playerHp = currentTarget.GetComponent<PlayerHealth>();
        if (playerHp != null)
            playerHp.TakeDamage(damage);

        PrincessHealth princessHp = currentTarget.GetComponent<PrincessHealth>();
        if (princessHp != null)
            princessHp.TakeDamage(damage);
    }

    void RunAway()
    {
        if (!runDirectionSet)
        {
            runDirection = Random.insideUnitCircle.normalized;
            runDirectionSet = true;
        }

        FaceDirection(runDirection);

        rb.velocity = runDirection * speed;
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (currentState != EnemyState.RunAway) return;
        if (collision.collider.isTrigger) return;

        runDirection = Random.insideUnitCircle.normalized;
    }

    void FaceDirection(Vector2 dir)
    {
        if (dir.x < 0)
            sr.flipX = true;
        else if (dir.x > 0)
            sr.flipX = false;
    }
}