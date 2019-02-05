using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangProjectile : Projectile
{
    //public float ReturnSpeed = 10f;

    //private float MaxAngle = 45f; // degrees
    //private float AngleSpeed = 90f;

    public float FlyTime = 2.0f;
    public float Range = 7.0f;

    private Vector2 startPoint;
    private Vector2 targetPoint;
    protected float flyT = 0.0f;

    protected bool returning = false;

    protected float travelT = 0.0f;

    //protected float currentSpeed = 0.0f;
    //protected float currentAngle = 0.0f;
    //protected float angleT = 0.0f;

    //protected Vector2 currDirection;
    //protected Vector2 startDirection;
    //protected Vector2 endDirection;

    //Vector3 beginPoint;
    //Vector3 finalPoint;
    //Vector3 farPoint;



    // Start is called before the first frame update
    void Start()
    {
        travelT = FlyTime / 2;

        /*
        currentSpeed = speed;
        currentAngle = MaxAngle;

        startDirection = Quaternion.Euler(0, 0, -MaxAngle) * direction;
        endDirection = -startDirection;//Quaternion.Euler(0, 0, -MaxAngle) * direction;

        angleT = 0.0f;

        currDirection = startDirection;
        */
        UpdatePoints();
    }

    protected void UpdatePoints()
    {
        startPoint = transform.position;
        targetPoint = transform.position + (Vector3)direction * Range;

        //beginPoint = transform.position;
        //finalPoint = transform.position + (Vector3)direction * Range;
        //farPoint = transform.position;
    }

    public override void SetDirection(Vector2 dir)
    {
        base.SetDirection(dir);

        UpdatePoints();
    }

    public override void Init(Guardian g)
    {
        base.Init(g);
        Guardian.PauseShooting();
    }

    protected override void HandleUpdate()
    {
        flyT += Time.deltaTime;
        Vector3 newPos = Vector3.Lerp(startPoint, targetPoint, flyT / travelT);
        rb2d.MovePosition(newPos);

        if (!returning && flyT > travelT)
        {
            StartReturn();
        }



        //currentSpeed -= ReturnSpeed * Time.deltaTime;
        //currentSpeed = Mathf.Max(-1 * speed, currentSpeed);

        //angleT += Time.deltaTime;


        /*
        Vector3 center = (beginPoint + finalPoint) * 0.5F;
        center -= farPoint;

        Vector3 riseRelCenter = beginPoint - center;
        Vector3 setRelCenter = finalPoint - center;

        Vector3 newPos = Vector3.Slerp(riseRelCenter, setRelCenter, angleT / FlyTime);
        newPos += center;

        rb2d.MovePosition(newPos);
        */

        //currentAngle = MaxAngle * Mathf.Cos(angleT);//AngleSpeed * Time.deltaTime;
        //currDirection = Quaternion.Euler(0, 0, MaxAngle * Time.deltaTime) * currDirection;//Vector2.Lerp(currDirection, Quaternion.Euler(0, 0, 20) * direction, angleT);//Quaternion.Euler(0, 0, currentAngle) * direction;

        /*
        if (angleT < 270)
        {
            float rotateSpeed = 270 / FlyTime;
            angleT += rotateSpeed * Time.deltaTime;

            float t = Mathf.Sin(angleT / 270.0f);

            currDirection = Quaternion.Euler(0, 0, rotateSpeed * Time.deltaTime) * currDirection;
        }
        */

        //angleT += Time.deltaTime;//AngleSpeed * Time.deltaTime;
        //angleT = Mathf.Min(angleT, 1.0f);

        //currDirection = Vector2.Lerp(currDirection, Guardian.gameObject.transform.position - transform.up, Mathf.Sin(angleT));

        /*
        Debug.DrawLine(transform.position, transform.position + 10 * (Vector3)currDirection, Color.red);

        rb2d.velocity = new Vector2(Mathf.Lerp(0, currDirection.x * currentSpeed, 0.8f),
                                                Mathf.Lerp(0, currDirection.y * currentSpeed, 0.8f));
        */
        // rotate to look direction 
        Vector3 look = new Vector3(direction.x, direction.y, 0.0f);

        float rot_z = Mathf.Atan2(look.y, look.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
    }

    protected void StartReturn()
    {
        if (!returning)
        {
            returning = true;
            flyT = 0.0f;

            startPoint = transform.position;
            targetPoint = Guardian.transform.position;

            Vector2 vDist = targetPoint - startPoint;
            float fDist = vDist.magnitude;

            travelT = (FlyTime / 2) * (fDist / Range); // scale the time by the distance to return, so the speed is the same

            Debug.Log("Starting return! travelT =  " + travelT + " fDist = " + fDist);
        }
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
        Guardian.PlayAnimation();
        Guardian.ResumeShooting();
        Destroy(gameObject);
    }

    protected override void HandleCollision(Collision2D col)
    {
        if (col.gameObject.tag == "Wall")
        {
            StartReturn();
        }
        else
        {
            base.HandleCollision(col);
        }
    }

    protected override void OnHit()
    {
        Guardian.ResumeShooting();

        base.OnHit();
    }
}
