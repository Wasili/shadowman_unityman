using UnityEngine;
using System.Collections;

public class SnowBall : MyGameObject
{
    private float _speed;
    private float _snowballLifetime;
    private Vector3 _velocity;
    private Vector3 targetPosition;
    private EnemyAI[] _enemies;

    void Awake()
    {
        Physics.IgnoreCollision(gameObject.GetComponent<SphereCollider>(), GameObject.Find("Player").GetComponent<CapsuleCollider>());
        _name = "SnowBall";
        _speed = 1000.0f;
        _snowballLifetime = 1.3f;
        _velocity = (targetPosition - transform.position).normalized;

        _enemies = FindObjectsOfType<EnemyAI>();
    }

    void Update()
    {
        Physics.IgnoreCollision(gameObject.GetComponent<SphereCollider>(), GameObject.Find("Player").GetComponent<CapsuleCollider>());
        float _deltaTime = Time.deltaTime;
        _snowballLifetime -= _deltaTime;

        //transform.position = transform.position + (_velocity * _deltaTime * _speed);

        for (int i = 0; i < _enemies.Length; i++)
        {
            if (_enemies[i] != null)
            {
                if (!_enemies[i].isMarkedForDeletion())
                {
                    if (Vector3.Distance(transform.position, _enemies[i].transform.position) <= 2.0f)
                    {
                        Destroy(_enemies[i].gameObject);
                        Destroy(gameObject);
                    }
                }
            }
        }

        if (_snowballLifetime <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void setTargetPosition(Vector3 targetPos)
    {
        targetPosition = targetPos;
        _velocity = (targetPosition - transform.position).normalized;
        gameObject.GetComponent<Rigidbody>().AddForce(_velocity * _speed);
    }

    void OnCollisionEnter(Collision coll)
    {
        Destroy(this.gameObject);
    }
}
