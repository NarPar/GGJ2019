using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveProjectile : Projectile
{
    public float Radius = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void OnHit()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, Radius);
        int i = 0;
        while (i < hitColliders.Length)
        {
            if (hitColliders[i].tag == "Player")
            {
                hitColliders[i].GetComponent<Invader>().OnHit();
            }
            i++;
        }

        base.OnHit();
    }
}
