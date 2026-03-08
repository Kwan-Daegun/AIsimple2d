using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorFade : MonoBehaviour
{
    public float fadeSpeed = 2f;

    private SpriteRenderer sr;
    private bool fading = false;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (!fading) return;

        Color c = sr.color;
        c.a -= fadeSpeed * Time.deltaTime;
        sr.color = c;

        if (c.a <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void FadeOut()
    {
        fading = true;
    }
}
