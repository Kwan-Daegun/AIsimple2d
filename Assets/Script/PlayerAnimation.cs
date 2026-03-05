using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public Sprite[] walkDown;
    public Sprite[] walkUp;
    public Sprite[] walkLeft;
    public Sprite[] walkRight;

    public float frameRate = 0.1f;

    private SpriteRenderer sr;
    private Rigidbody2D rb;

    private float timer;
    private int frame;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector2 v = rb.velocity;

        if (v.sqrMagnitude < 0.01f)
        {
            frame = 0;
            timer = 0;
            return;
        }

        timer += Time.deltaTime;

        if (timer >= frameRate)
        {
            timer = 0f;
            frame++;
        }

        Sprite[] current = null;

        if (Mathf.Abs(v.x) > Mathf.Abs(v.y))
        {
            current = v.x > 0 ? walkRight : walkLeft;
        }
        else
        {
            current = v.y > 0 ? walkUp : walkDown;
        }

        if (current != null && current.Length > 0)
        {
            sr.sprite = current[frame % current.Length];
        }
    }
}