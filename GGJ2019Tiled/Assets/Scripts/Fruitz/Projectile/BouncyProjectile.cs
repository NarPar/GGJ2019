using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyProjectile : Projectile
{
    public int NumBounces = 2;

    protected int bounces = 0;

    // Start is called before the first frame update
    void Start()
    {
        bounces = NumBounces;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // called when the cube hits the floor
    protected override void HandleCollision(Collision2D col)
    {
        if (col.gameObject.tag == "Wall" && bounces > 0)
        {
            Bounce(col);
        }
        else
        {
            base.HandleCollision(col);
        }
    }

    protected void Bounce(Collision2D col)
    {
        bounces--;

        ContactPoint2D contact = col.contacts[0];

        Vector2 reflection = Vector2.Reflect(direction, contact.normal);

        /*
        float dot = Vector3.Dot(contact.normal, (-transform.forward));
        dot *= 2;
        Vector3 reflection = contact.normal * dot;
        reflection = reflection + transform.forward;
        */

        SetDirection(reflection.normalized);
    }
}
