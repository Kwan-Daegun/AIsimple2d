using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimation : MonoBehaviour
{
    public Sprite[] idleRight;
    public Sprite[] walkRight;
    public Sprite[] attackRight;
    public Sprite[] deathRight;

    public float idleFPS = 4f;
    public float walkFPS = 8f;
    public float attackFPS = 10f;
    public float deathFPS = 10f;

    public Transform attackPoint;

    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private PlayerHealth health;

    private float timer;
    private int frame;

    private bool attacking;
    private bool dead;

    private Vector3 attackPointStart;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<PlayerHealth>();

        if (attackPoint != null)
            attackPointStart = attackPoint.localPosition;
    }

    void Update()
    {
        if (sr == null || rb == null) return;

        if (health != null && health.IsDead)
        {
            PlayDeath();
            return;
        }

        if (attacking)
        {
            Animate(attackRight, attackFPS, true);
            return;
        }

        Vector2 v = rb.velocity;

        if (v.x != 0)
        {
            bool facingLeft = v.x < 0;

            sr.flipX = facingLeft;

            if (attackPoint != null)
            {
                attackPoint.localPosition = new Vector3(
                    facingLeft ? -attackPointStart.x : attackPointStart.x,
                    attackPointStart.y,
                    attackPointStart.z
                );
            }
        }

        if (v.sqrMagnitude < 0.01f)
        {
            Animate(idleRight, idleFPS);
            return;
        }

        Animate(walkRight, walkFPS);
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (attacking) return;

        attacking = true;
        frame = 0;
        timer = 0f;
    }

    void PlayDeath()
    {
        if (!dead)
        {
            dead = true;
            frame = 0;
            timer = 0f;
        }

        Animate(deathRight, deathFPS, true);
    }

    void Animate(Sprite[] sprites, float fps, bool stopAfter = false)
    {
        if (sprites == null || sprites.Length == 0) return;

        float frameRate = 1f / fps;

        timer += Time.deltaTime;

        if (timer >= frameRate)
        {
            timer = 0f;
            frame++;
        }

        if (frame >= sprites.Length)
        {
            if (stopAfter)
            {
                if (attacking)
                {
                    attacking = false;
                    frame = 0;
                    return;
                }

                Destroy(gameObject);
                return;
            }

            frame = 0;
        }

        sr.sprite = sprites[frame];
    }
}