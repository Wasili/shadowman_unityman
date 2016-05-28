using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputHandler : MonoBehaviour
{
    public bool mobileVersion = false;
    [System.NonSerialized]
    public bool jump, left, right, up, down, fire, pause, rotateLeft, rotateRight;
    [System.NonSerialized]
    public Touch shootTouch;
    List<Touch> badTouches;

    // Update is called once per frame
    void Update()
    {
        if (!mobileVersion)
        {
            jump = Input.GetKeyDown(KeyCode.Space);
            up = (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow));
            left = (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow));
            down = (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow));
            right = (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow));
            pause = Input.GetKey(KeyCode.Escape);
            fire = Input.GetMouseButton(0);
            rotateLeft = Input.GetKey(KeyCode.Q);
            rotateRight = Input.GetKey(KeyCode.E);
        }
        else
        {
            rotateLeft = Input.acceleration.x < -0.3f;
            rotateRight = Input.acceleration.x > 0.3f;
        }

        Touch[] myTouches = Input.touches;
        fire = false;

        for (int i = 0; i < myTouches.Length; i++)
        {
            shootTouch = myTouches[i];
            fire = !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(i) && shootTouch.phase != TouchPhase.Ended;
        }
    }

    public void SetLeft(bool clicked)
    {
        left = clicked;
    }

    public void SetRight(bool clicked)
    {
        right = clicked;
    }

    public void SetDown(bool clicked)
    {
        down = clicked;
    }

    public void SetUp(bool clicked)
    {
        up = clicked;
    }

    public void SetPause()
    {
        pause = true;
    }

    public void SetJump()
    {
        jump = true;
    }
}
