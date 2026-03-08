using UnityEngine;

public class Portal : MonoBehaviour
{
    public Sprite[] frames;        
    public float frameRate = 0.12f;

    private SpriteRenderer sr;

    private bool active = false;
    private float timer;
    private int frameIndex;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.enabled = false;
    }

    public void ActivatePortal()
    {
        active = true;
        sr.enabled = true;
        frameIndex = 0;

        Debug.Log("Portal Activated");
    }

    void Update()
    {
        if (!active) return;

        timer += Time.deltaTime;

        if (timer >= frameRate)
        {
            timer = 0f;
            frameIndex++;

            if (frameIndex >= frames.Length)
                frameIndex = 0;

            sr.sprite = frames[frameIndex];
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!active) return;

        if (other.CompareTag("Player"))
        {
            Debug.Log("Level Complete");
        }
    }
}