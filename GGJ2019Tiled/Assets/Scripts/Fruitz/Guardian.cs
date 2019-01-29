using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Fruitz;
using UnityEngine;

public class Guardian : MonoBehaviour
{
    public Identity Identity { get; set; }

    public float shootTime = 2.0f;
    public GameObject projectilePrefab;

    public bool Activated = false;

    private Animator animator;
    private float timer = 0.0f;

    protected Vector2 direction = Vector2.left;

    private Transform throwPoint;

    public GuardianPlacementController placer;

    // Use this for initialization
    void Awake()
    {
        animator = GetComponent<Animator>();

        alphaTime = 0.0f;

        throwPoint = transform.Find("ThrowPoint");
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir;

        // rotate to look direction 
        Vector3 look = new Vector3(direction.x, direction.y, 0.0f);

        //transform.right = look;
        float rot_z = Mathf.Atan2(look.y, look.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);

        Activated = true;
        timer = 0.0f; // shoot immediately
        
        Color tmp = GetComponent<SpriteRenderer>().color;
        tmp.a = 1f;
        GetComponent<SpriteRenderer>().color = tmp;

        placer = null;
    }

        // Update is called once per frame
        void Update()
    {
        if (Activated)
        {
            timer -= Time.deltaTime;
            if (timer <= 0.0f)
            {
                ShootProjectile();

                timer = shootTime;
            }
        }
        else
        {
            UpdateAlphaFlash();
        }
    }

    protected void ShootProjectile()
    {
        var projectile = Instantiate(projectilePrefab, throwPoint.position, Quaternion.identity).GetComponent<Projectile>();
        projectile.SetDirection(direction);
        projectile.Guardian = this;

        animator.SetTrigger("shoot");

        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }

    float alphaTime = 0.0f;
    protected void UpdateAlphaFlash()
    {
        alphaTime += Time.deltaTime * 6;

        Color tmp = GetComponent<SpriteRenderer>().color;
        tmp.a = 0.3f + 0.3f * (1 + (0.5f * Mathf.Sin(alphaTime)));
        GetComponent<SpriteRenderer>().color = tmp;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (placer != null)
        {
            placer.HandleEnterRegion(collision);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (placer != null)
        {
            placer.HandleExitRegion(collision);
        }
    }
}
