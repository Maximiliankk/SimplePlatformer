using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlatformerController : MonoBehaviour
{
    public enum SpeedPreset
    {
        custom,
        slow,
        medium,
        fast
    };
    //public SpeedPreset preset = SpeedPreset.custom;

    private static int globalId = 1;
    public PlayerIndex carbonInputId;
    Rigidbody rb;
    public bool onGround = false;
    public bool jumpOnCD = false;
    public float jumpForce = 100;
    public float moveForce = 20;
    public float aircontrolForce = 5;
    public float jumpCDdur = 0.5f;
    public float minMoveEpsilon = 0.4f;
    public float horizontalDrag = 0.4f;
    private bool toggleMenu = false;

    void Start()
    {
        carbonInputId = (PlayerIndex)globalId;
        //globalId++;

        rb = GetComponent<Rigidbody>();

        UpdatePreset();
    }

    private void UpdatePreset()
    {
        //switch (preset)
        //{
        //    case SpeedPreset.slow:
        //        rb.drag = 0.6f;
        //        jumpForce = 400;
        //        moveForce = 10;
        //        jumpCDdur = 0.5f;
        //        break;
        //    case SpeedPreset.medium:
        //        rb.drag = 0.8f;
        //        jumpForce = 600;
        //        moveForce = 20;
        //        jumpCDdur = 0.25f;
        //        break;
        //    case SpeedPreset.fast:
        //        rb.drag = 0.9f;
        //        jumpForce = 400;
        //        moveForce = 30;
        //        jumpCDdur = 0.25f;
        //        break;
        //}
    }

    // Update is called once per frame
    void Update()
    {
        var padState = GamePad.GetState();

        if (GamePad.AnyConnected()) // if using gamepad
        {
            // start button is pressed
            if (padState.Pressed(CButton.Start))
            {
                ToggleMenu();
            }
            // Right bumper is held, and right stick is pressed
            if (GamePad.GetButton(CButton.RB, carbonInputId) && padState.Pressed(CButton.LS))
            {
                // reload current level
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            // right bumper is held and left bumper is pressed
            if (GamePad.GetButton(CButton.RB, carbonInputId) && padState.Pressed(CButton.LB))
            {
                // go to first level
                SceneManager.LoadScene(0);
            }
        }
        else // if using keyboard
        {
            // start button is pressed
            if (Input.GetKeyDown(KeyCode.M))
            {
                ToggleMenu();
            }
            // Shift is held and R is pressed
            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.R))
            {
                // reload current level
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            // R is held and G is pressed
            if (Input.GetKey(KeyCode.R) && Input.GetKeyDown(KeyCode.G))
            {
                // go to first level
                SceneManager.LoadScene(0);
            }
        }

        // calculate input movement direction
        Vector2 move = new Vector2(
        GamePad.GetAxis(CAxis.LX, carbonInputId), 0);

        UpdateJumping();

        UpdateMovement(move);
    }

    private void UpdateMovement(Vector2 move)
    {
        if (onGround)
        {
            rb.AddForce(move * moveForce);
        }
        else
        {
            rb.AddForce(move * aircontrolForce);
        }
    }

    private void UpdateJumping()
    {
        // jump
        if (onGround && !jumpOnCD && GamePad.GetButton(CButton.A, carbonInputId))
        {
            rb.AddForce(Vector2.up * jumpForce);
            StartCoroutine(JumpCD());
        }
    }

    IEnumerator JumpCD()
    {
        jumpOnCD = true;
        yield return new WaitForSeconds(jumpCDdur);
        jumpOnCD = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        onGround = false;
    }
    private void OnCollisionStay(Collision other)
    {
        // points from this to other
        Vector2 distvec = new Vector2(
            other.transform.position.x - this.transform.position.x,
            other.transform.position.y - this.transform.position.y);

        // widths and heights added together
        Vector2 scalevec = new Vector2(
                other.transform.localScale.x / 2f + this.transform.localScale.x / 2f,
                other.transform.localScale.y / 2f + this.transform.localScale.y / 2f);

        // check to make sure this is above other (vertically) and also inside other (horizontally)
        if (distvec.y < 0
            && Mathf.Abs(distvec.y) > scalevec.y - 0.01f
            && distvec.x < scalevec.x)
        {
            // then we can safely assume this is on top of other and we can push off of it
            onGround = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        onGround = false;
    }

    private void ToggleMenu()
    {
        toggleMenu = !toggleMenu;
    }

    private void OnGUI()
    {
        var padState = GamePad.GetState();
        if (GamePad.AnyConnected())
        {
            if (GUI.Button(new Rect(5, 5, 150, 30), "Menu (press Start)"))
            {
                ToggleMenu();
            }
        }
        else
        {
            if (GUI.Button(new Rect(5, 5, 150, 30), "Menu (M)"))
            {
                ToggleMenu();
            }
        }

        if (toggleMenu)
        {
            var verticalPadding = 40;
            var verticalStartPos = 50;
            var horizontalStartPos = 60;
            var horizontalPadding = 250;
            if(GamePad.AnyConnected())
            {
                GUI.Label(new Rect(horizontalStartPos, verticalStartPos, horizontalPadding, verticalPadding), "Left Stick.....Move"); verticalStartPos += verticalPadding;
                GUI.Label(new Rect(horizontalStartPos, verticalStartPos, horizontalPadding, verticalPadding), "A.....Jump"); verticalStartPos += verticalPadding;
                GUI.Label(new Rect(horizontalStartPos, verticalStartPos, horizontalPadding, verticalPadding), "Hold right bumper + Press left bumper......Restart Level"); verticalStartPos += verticalPadding;
                GUI.Label(new Rect(horizontalStartPos, verticalStartPos, horizontalPadding, verticalPadding), "Hold right bumper + Press left stick......Restart Game"); verticalStartPos += verticalPadding;
            }
            else
            {
                GUI.Label(new Rect(horizontalStartPos, verticalStartPos, horizontalPadding, verticalPadding), "WASD.....Move"); verticalStartPos += verticalPadding;
                GUI.Label(new Rect(horizontalStartPos, verticalStartPos, horizontalPadding, verticalPadding), "Space.....Jump"); verticalStartPos += verticalPadding;
                GUI.Label(new Rect(horizontalStartPos, verticalStartPos, horizontalPadding, verticalPadding), "Shift + R......Restart Level"); verticalStartPos += verticalPadding;
                GUI.Label(new Rect(horizontalStartPos, verticalStartPos, horizontalPadding, verticalPadding), "R + G......Restart Game"); verticalStartPos += verticalPadding;
            }
        }
    }

}
