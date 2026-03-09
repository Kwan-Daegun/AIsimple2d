using UnityEngine;

public class PrincessAnimation : MonoBehaviour
{
    public Sprite[] idleRight;
    public Sprite[] walkRight;
    public Sprite[] deathRight;

    public float idleFPS = 4f;
    public float walkFPS = 8f;
    public float deathFPS = 10f;

    private SpriteRenderer sr;
    private PrincessHealth health;

    private float timer;
    private int frame;

    private bool deathStarted;

    private Vector2 lastPosition;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        health = GetComponent<PrincessHealth>();

        lastPosition = transform.position;
    }

    void Update()
    {
        if (sr == null) return;

        if (health != null && health.IsDead)
        {
            PlayDeath();
            return;
        }

        Vector2 v = (Vector2)transform.position - lastPosition;
        lastPosition = transform.position;

        if (v.x != 0)
            sr.flipX = v.x < 0;

        if (v.sqrMagnitude < 0.0001f)
        {
            Animate(idleRight, idleFPS);
            return;
        }

        Animate(walkRight, walkFPS);
    }

    void PlayDeath()
    {
        if (!deathStarted)
        {
            deathStarted = true;
            frame = 0;
            timer = 0f;
        }

        Animate(deathRight, deathFPS, true);
    }

    void Animate(Sprite[] sprites, float fps, bool destroyAfter = false)
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
            if (destroyAfter)
            {
                Destroy(gameObject);
                return;
            }

            frame = 0;
        }

        sr.sprite = sprites[frame];
    }
}