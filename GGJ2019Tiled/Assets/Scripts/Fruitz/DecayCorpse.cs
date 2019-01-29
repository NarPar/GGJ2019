using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecayCorpse : MonoBehaviour
{
    protected SpriteRenderer sprite;

    protected Color startColour;
    protected Color endColour;

    private float t = 0.0f;
    private float speed = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();

        startColour = sprite.color;
        endColour = new Color(0.5f, 0.5f, 0.5f, 0.5f);

        t = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (t < 1)
        {
            t += speed * Time.deltaTime;
            Color current = Color.Lerp(startColour, endColour, Mathf.Clamp(Mathf.Pow(t,2), 0, 1));
            sprite.color = current;
        }
    }
}
