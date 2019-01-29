using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class Player : MonoBehaviour
{
    public enum PlayerState { Waiting, Invading, Placing };
    
    public GameManager GameManager { get; set; }

    public int inputID = 1; // or 2. Set by GameManager?

    public PlayerState State
    {
        get { return state; }
    }

    PlayerState state = PlayerState.Waiting;

    private PlayerNameText nameText;
    private Invader invader;
    private Guardian guardian;
    private GuardianPlacementController turretPlacer;

    // Start is called before the first frame update
    public void Init(GameManager gm, int id)
    {
        GameManager = gm;
        inputID = id;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (state)
        {
            case PlayerState.Waiting:
                UpdateWaiting();
                break;
            case PlayerState.Invading:
                UpdateInvading();
                break;
            case PlayerState.Placing:
                UpdatePlacing();
                break;
        }
    }

    void UpdateWaiting()
    {
        // wait for player join
        if (Input.GetButton("Jump" + inputID))
        {
            Debug.Log("Player " + inputID + " Joining!");

            NewIdentity();
            state = PlayerState.Invading; // move to next state
        }
    }

    void NewIdentity()
    {
        invader = GameManager.SpawnInvader();
        invader.SetPlayer(this);

        nameText = GameManager.SpawnNameText();
        nameText.Set(invader.transform, invader.Identity);
    }

    void UpdateInvading()
    {
        Vector2 move = Vector2.zero;
        // get input
        move.x = Input.GetAxis("Horizontal" + inputID);
        move.y = Input.GetAxis("Vertical" + inputID);

        invader.UpdateMovement(move);
    }

    public void HandleInvaderKilled()
    {
        state = PlayerState.Waiting;

        GameManager.RemoveIdentity(invader.Identity);

        Destroy(nameText.gameObject);
        Destroy(invader.gameObject);

        GameManager.UpdateScoreBoard();
    }

    public void HandleEnterGoalRegion()
    {
        guardian = GameManager.SpawnGuardian(invader.Identity);
        nameText.Set(guardian.transform, invader.Identity);

        

        Destroy(invader.gameObject);

        turretPlacer = GameManager.SpawnGuardianPlacementController();

        turretPlacer.StartPlacement(guardian, this);

        state = PlayerState.Placing;
    }

    void UpdatePlacing()
    {
        Vector2 input = Vector2.zero;
        // get input
        input.x = Input.GetAxis("Horizontal" + inputID);
        input.y = Input.GetAxis("Vertical" + inputID);

        turretPlacer.UpdatePlacement(input, Time.deltaTime);

        if (Input.GetButtonUp("Jump" + inputID))
        {
            turretPlacer.HandleAccept();
        }
    }

    public void DonePlacement()
    {
        state = PlayerState.Waiting;

        Destroy(turretPlacer);

        GameManager.UpdateScoreBoard();
    }
}
