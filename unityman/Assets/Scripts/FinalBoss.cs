using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FinalBoss : MyGameObject {
	public Vector3 fightPosition;
    
	Player _player;
    EnemyAI[] _enemies;
    enum state { idle, move, fight };
    state curState;
    float _shootCD;
    float _shootTimer;
    float _health;
    Vector3 _startPosition;
    public FinalBossProjectile projectile;

    void Awake()
    {
        curState = state.idle;
        _shootCD = 0.5f;
        _shootTimer = _shootCD;
        _health = 100;
        _startPosition = transform.position;
    }

    void Start()
    {
        _player = GameObject.FindWithTag("Player").GetComponent<Player>();
        _findEnemiesAlive();
    }

    void Update()
    {
        if (curState == state.idle)
        {
            _idle();
        }
        else if (curState == state.fight)
        {
            _fight();
        }
        else if (curState == state.move)
        {
            _move();
        }
    }

    void _fight()
    {
        _shootTimer -= Time.deltaTime;

        if (_shootTimer <= 0)
        {
            Vector3 shootVelocity = (_player.transform.position - transform.position).normalized;

            FinalBossProjectile myProjectile = (FinalBossProjectile)Instantiate(projectile, transform.position, new Quaternion());
            myProjectile.velocity = shootVelocity;
            _shootTimer = _shootCD;
        }

        SnowBall[] snowballs = GameObject.FindObjectsOfType<SnowBall>();
        for (int i = 0; i < snowballs.Length; i++)
        {
            if (Vector3.Distance(snowballs[i].transform.position, transform.position) <= 2.0f)
            {
                Destroy(snowballs[i].gameObject);
                _health -= 15;
            }
        }

        if (_health <= 0)
        {
            _player.Win(_startPosition);
            Destroy(gameObject);
        }
    }

    void _idle()
    {
        _findEnemiesAlive();
        if (_enemies.Length <= 0)
        {
            curState = state.move;
        }
    }

    void _move()
    {
        DynamicLightSource light = GameObject.FindObjectOfType<DynamicLightSource>();
        if (light != null)
        {
            Destroy(light.gameObject);
        }

        transform.position += (fightPosition - transform.position).normalized * Time.deltaTime * 5.0f;
        _player.updateHealth(Time.deltaTime * 15.0f);

        if (Vector3.Distance(transform.position, fightPosition) <= 0.2f)
        {
            curState = state.fight;
        }
    }

    void _findEnemiesAlive()
    {
        _enemies = GameObject.FindObjectsOfType<EnemyAI>();
    }
}
