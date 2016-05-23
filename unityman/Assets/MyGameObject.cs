using UnityEngine;
using System.Collections;

public class MyGameObject : MonoBehaviour {
	private string _name;
    private bool _isDeleted;

    void Awake()
    {
        _name = name;
	    _isDeleted = false;
    }

    void Start()
    {
        initialize();
    }

    void Update()
    {
        update(Time.deltaTime);
    }
    
    void initialize()
    {
        //Start-up logic is specified in the inheriting object
    }

    void update(float deltaTime)
    {
        //Per-frame logic is specified in the inheriting object
    }

    void renderGUI()
    {
        //Per-frame GUI behaviour is specified in the inheriting object
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

    bool isMarkedForDeletion()
    {
        return _isDeleted;
    }
}
