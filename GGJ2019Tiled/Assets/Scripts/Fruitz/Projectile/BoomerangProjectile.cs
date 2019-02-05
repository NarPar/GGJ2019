using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangProjectile : Projectile
{
    public float ReturnSpeed = 10f;
    private float MaxAngle = 45f; // degrees
    private float AngleSpeed = 1f;

    protected float currentSpeed = 0.0f;
    protected float currentAngle = 0.0f;
    protected float angleT = 0.0f;

    protected Vector2 currDirection;
    protected Vector2 startDirection;
    protected Vector2 endDirection;

    // Start is called before the first frame update
    void Start()
    {
        currentSpeed = speed;
        currentAngle = MaxAngle;

        startDirection = Quaternion.Euler(0, 0, MaxAngle) * direction;
        endDirection = -startDirection;//Quaternion.Euler(0, 0, -MaxAngle) * direction;

        currDirection = startDirection;
    }

    public override void Init(Guardian g)
    {
        base.Init(g);
        Guardian.PauseShooting();
    }

    protected override void HandleUpdate()
    {
        //currentSpeed -= ReturnSpeed * Time.deltaTime;
        currentSpeed = Mathf.Max(-1 * speed, currentSpeed);

        angleT += AngleSpeed * Time.deltaTime;
        //currentAngle = MaxAngle * Mathf.Cos(angleT);//AngleSpeed * Time.deltaTime;
        currDirection = Quaternion.Euler(0, 0, MaxAngle * Time.deltaTime) * currDirection;//Vector2.Lerp(currDirection, Quaternion.Euler(0, 0, 20) * direction, angleT);//Quaternion.Euler(0, 0, currentAngle) * direction;

        Debug.DrawLine(transform.position, transform.position + 10 * (Vector3)currDirection, Color.red);

        rb2d.velocity = new Vector2(Mathf.Lerp(0, currDirection.x * currentSpeed, 0.8f),
                                                Mathf.Lerp(0, currDirection.y * currentSpeed, 0.8f));

        // rotate to look direction 
        Vector3 look = new Vector3(direction.x, direction.y, 0.0f);

        float rot_z = Mathf.Atan2(look.y, look.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
    }

    bool exited = false;
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Guardian>() == Guardian)
        {
            exited = true;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (exited && collision.gameObject.GetComponent<Guardian>() == Guardian)
        {
            Catch();
        }
    }

    protected void Catch()
    {
        Guardian.ResumeShooting();
        Destroy(gameObject);
    }

    protected override void OnHit()
    {
        Guardian.ResumeShooting();

        base.OnHit();
    }
}
