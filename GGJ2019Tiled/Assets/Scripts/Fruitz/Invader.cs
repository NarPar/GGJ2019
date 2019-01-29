using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Fruitz;
using UnityEngine;

public class Invader : MonoBehaviour
{
    public Identity Identity { get; set; }

    public float speed = 7;
    public GameObject squashed;

    protected Rigidbody2D rb2d;
    private Animator animator;

    private float curSpeed;

    private Player player;

    void OnEnable()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    public void SetPlayer(Player p)
    {
        player = p;
    }

    // Update is called once per frame
    public void UpdateMovement(Vector2 input)
    {
        // move it
        Vector2 force = input * speed;

        curSpeed = speed;
        rb2d.velocity = new Vector2(Mathf.Lerp(0, input.x * curSpeed, 0.8f),
                                                Mathf.Lerp(0, input.y * curSpeed, 0.8f));
        // rotate to look direction 
        Vector2 moveNorm = input.normalized;
        Vector3 look = new Vector3(moveNorm.x, moveNorm.y, 0.0f);

        if (force.magnitude > 0.01f) // only change direction if has input
        {
            //transform.right = look;
            float rot_z = Mathf.Atan2(look.y, look.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
        }

        animator.SetFloat("velocity", Mathf.Abs(force.magnitude) / speed);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        //Debug.Log("FruitProjectile -> OnCollisionEnter2D ; " + col.gameObject.name + ", Tag " + col.gameObject.tag + ", Layer " + col.gameObject.layer);
        if (col.gameObject.tag == "Projectile") // Projectile layer
        {
            OnHit();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Goal") // Enter goal region! Winner!
        {

            player.HandleEnterGoalRegion();
        }
    }

    public void OnHit()
    {
        Instantiate<GameObject>(squashed, transform.position, transform.rotation);
        player.HandleInvaderKilled();
    }
}
