using UnityEngine;
using System.Collections;

public class SnowBall : MyGameObject {
    private float _speed;
    private float _snowballLifetime;
    private Vector3 _velocity;
    public Vector3 targetPosition;
    private EnemyAI[] _enemies;

    void Awake()
    {
        _name = "SnowBall";
        _speed = 10.0f;
        _snowballLifetime = 1.3f;
        _velocity = (targetPosition - transform.position).normalized;

        _enemies = FindObjectsOfType<EnemyAI>();
    }

    void Update()
    {
        float _deltaTime = Time.deltaTime;
        _snowballLifetime -= _deltaTime;
        transform.position = transform.position + (_velocity * _deltaTime * _speed);

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
}
