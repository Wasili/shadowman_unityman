using UnityEngine;
using System.Collections;

public class SnowBall : MyGameObject {
    private float _speed;
    private float _snowballLifetime;
    private Vector3 _velocity;
    private Vector3 _targetPosition;
    private EnemyAI[] _enemies;

    void Awake()
    {
        _speed = 10.0f;
        _snowballLifetime = 1.3f;
        _velocity = (_targetPosition - transform.position).normalized;

        _enemies = FindObjectsOfType<EnemyAI>();
    }

    void update(float deltaTime)
    {
        float _deltaTime = deltaTime;
        _snowballLifetime -= _deltaTime;
        transform.position = transform.position + (_velocity * _deltaTime * _speed);

        for (int i = 0; i < _enemies.Length; i++)
        {
            /*if (_enemies[i] != null)
            {
                if (!_enemies[i].isMarkedForDeletion())
                {
                    if (Vector3.Distance(transform.position, _enemies[i].transform.position) <= 2.0f)
                    {
                        Destroy(_enemies[i]);
                        Destroy(this);
                    }
                }
            }*/
        }

        if (_snowballLifetime <= 0)
        {
            Destroy(this);
        }
    }
}
