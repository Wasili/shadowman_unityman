using UnityEngine;
using System.Collections;

public class MyGameObject : MonoBehaviour {
	protected string _name;
    private bool _isDeleted;

    void Awake()
    {
        _name = name;
	    _isDeleted = false;
    }

    void Start()
    {

    }

    void Destroy()
    {
        _isDeleted = true;
    }

    string getName()
    {
        return _name;
    }

    GameObject getSceneNode()
    {
        return gameObject;
    }

    public bool isMarkedForDeletion()
    {
        return _isDeleted;
    }
}
