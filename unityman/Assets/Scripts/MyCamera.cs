using UnityEngine;
using System.Collections;

public class MyCamera : MyGameObject
{
    Camera _cameraSceneNode;

    Player _player;
    Vector3 _cameraPos;
    public Vector3 _fixedCameraPos;
    Transform parentObject;
    InputHandler inputHandler;

    void Awake()
    {
        inputHandler = GameObject.FindObjectOfType<InputHandler>();
        _cameraSceneNode = GetComponent<Camera>();
        _player = null;
        parentObject = transform.parent;
    }

    void Start()
    {
        _player = GameObject.FindWithTag("Player").GetComponent<Player>();
        _cameraSceneNode.transform.LookAt(_player.transform.position);
    }

    Camera getSceneNode()
    {
        return _cameraSceneNode;
    }

    // Camera follows player around
    public void FixedUpdate()
    {
        // get current position
        //_cameraPos = transform.position;

        // update the camera position
        //_cameraPos = _player.transform.position + _fixedCameraPos;

        // set new position of the camera
        //_cameraSceneNode.transform.position = (_cameraPos);
        parentObject.transform.position = _player.transform.position;
        if (inputHandler.rotateLeft)
        {
            parentObject.eulerAngles = new Vector3(0, parentObject.eulerAngles.y + 45 * Time.deltaTime, 0);
        }
        else if (inputHandler.rotateRight)
        {
            parentObject.eulerAngles = new Vector3(0, parentObject.eulerAngles.y - 45 * Time.deltaTime, 0);
        }
    }

    void setCameraPos(Vector3 cameraPos)
    {
        _fixedCameraPos = cameraPos;
    }
}
