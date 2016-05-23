using UnityEngine;
using System.Collections;

public class LightSource : MyGameObject
{
    public GameObject _parentNode;
    protected Player _player;
    protected float _damage;
    protected float _lightRadius;

    void Awake()
    {
        _damage = -5;
    }

    void Start()
    {
        _player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    public virtual void Update()
    {
        _damagePlayer();
    }
    
    protected virtual void _damagePlayer()
    {
        //create a ray with its starting point being the mouse cursor. This method automatically converts screen coordindates to world coordinates if the correct camera is provided
        Ray ray = new Ray(transform.position, _player.transform.position);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100.0f))
        {
            if (hit.transform.gameObject.tag == "Player")
            {
                _player.updateHealth(_damage * Time.deltaTime);
            }
        }
    }


    Vector3 getPosition()
    {
        return transform.position;
    }

    void setLightPosition(Vector3 newPosition)
    {
        _parentNode.transform.position = (newPosition);
    }
}
