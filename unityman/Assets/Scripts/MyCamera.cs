using UnityEngine;
using System.Collections;

public class MyCamera : MyGameObject {
    Camera _cameraSceneNode;

    Player _player;
    Vector3 _cameraPos;
    public Vector3 _fixedCameraPos;

    void Awake()
    {
        _cameraSceneNode = GetComponent<Camera>();
        _player = null;
    }

    void Start()
    {
        _player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    Camera getSceneNode()
    {
        return _cameraSceneNode;
    }

    // Camera follows player around
    void Update()
    {
        // get current position
        _cameraPos = transform.position;

        //look at the player
        _cameraSceneNode.transform.LookAt(_player.transform.position);

        // update the camera position
        _cameraPos = _player.transform.position + _fixedCameraPos;

        // set new position of the camera
        _cameraSceneNode.transform.position = (_cameraPos);
    }

    void setCameraPos(Vector3 cameraPos)
    {
        _fixedCameraPos = cameraPos;
    }
}
