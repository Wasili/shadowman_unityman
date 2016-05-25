using UnityEngine;
using System.Collections;

public class InputHandler : MonoBehaviour
{
    [System.NonSerialized]
    public bool jump, left, right, up, down, fire, pause;

    // Update is called once per frame
    void Update()
    {
        jump = Input.GetKeyDown(KeyCode.Space);
        up = (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow));
        left = (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow));
        down = (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow));
        right = (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow));
        fire = Input.GetMouseButton(0);
        pause = Input.GetKey(KeyCode.Escape);
    }
}
