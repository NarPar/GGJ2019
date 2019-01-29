using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardianPlacementController : MonoBehaviour
{
    enum PlacementState { None, Positioning, Rotating };

    public float placeSpeed = 7.0f;
    public float rotateSpeed = 7.0f;

    public AudioClip PlacementSFX;
    public AudioClip InvalidSFX;

    private PlacementState state = PlacementState.None;

    Player player;
    Guardian guardian;
    Rigidbody2D gBod;

    Vector2 currentDir = Vector2.left;

    private bool placementValid = true;

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void StartPlacement(Guardian g, Player p)
    {
        //Debug.Log("TurretPlacementController -> Starting Placement!");

        player = p;
        guardian = g;
        gBod = guardian.GetComponent<Rigidbody2D>();

        g.placer = this;

        state = PlacementState.Positioning;
    }

    public void UpdatePlacement(Vector2 input, float deltaTime)
    {
        switch(state)
        {
            case (PlacementState.Positioning):
                UpdatePositioning(input);
                break;
            case (PlacementState.Rotating):
                UpdateRotating(input, deltaTime);
                break;
        }
    }

    void UpdatePositioning(Vector2 input)
    {
        // move it
        Vector2 force = input * placeSpeed;

        gBod.velocity = new Vector2(Mathf.Lerp(0, input.x * placeSpeed, 0.8f),
                                                Mathf.Lerp(0, input.y * placeSpeed, 0.8f));

        if (input.magnitude > 0.01f) // only change direction if has input
        {
            // rotate to look direction 
            Vector3 look = new Vector3(input.x, input.y, 0.0f);

            float rot_z = Mathf.Atan2(look.y, look.x) * Mathf.Rad2Deg;
            guardian.transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);

            currentDir = input;
        }
    }

    void UpdateRotating(Vector2 input, float deltaTime)
    {
        // TODO: Rotate towards direction, not by look

        float amount = rotateSpeed * ((input.x * -1) + input.y); 

        guardian.transform.RotateAround(guardian.transform.position, guardian.transform.forward, amount);

        /*
        // rotate to look direction 
        Vector3 look = new Vector3(input.x, input.y, 0.0f);

        float rot_z = Mathf.Atan2(look.y, look.x) * Mathf.Rad2Deg;
        guardian.transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
            
        currentDir = input;
        */

        currentDir = guardian.transform.up;
    }

    public void HandleEnterRegion(Collider2D col)
    {
        if (col.tag == "InvalidPlacementRegion")
        {
            placementValid = false;
            guardian.GetComponent<SpriteRenderer>().color = Color.red;
        }
    }

    public void HandleExitRegion(Collider2D col)
    {
        if (col.tag == "InvalidPlacementRegion")
        {
            placementValid = true;
            guardian.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    public void HandleAccept()
    {
        if (placementValid)
        {
            audioSource.clip = PlacementSFX;
            audioSource.volume = 1f;
            audioSource.Play();

            switch (state)
            {
                case (PlacementState.Positioning):
                    guardian.GetComponent<Collider2D>().enabled = false;
                    gBod.velocity = Vector2.zero;

                    state = PlacementState.Rotating;
                    break;
                case (PlacementState.Rotating):
                    guardian.SetDirection(currentDir.normalized);

                    state = PlacementState.None;
                    player.DonePlacement();
                    break;
            }
        }
        else
        {
            audioSource.clip = InvalidSFX;
            audioSource.volume = 0.5f;
            audioSource.Play();
        }
    }
}
