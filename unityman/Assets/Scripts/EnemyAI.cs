using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyAI : MyGameObject {
    List<FireBall> fireBallList = new List<FireBall>();
    
    float _speed;
    float _rotateSpeed;
    Vector3 _rotattion;
    float _shootCoolDown = 1;

    enum State
    {
        root,
        idle,
        patrol,
        combat,
        move,
        attack
    };
    State _state;

    public FireBall fireBall;
    float _counter;
    Player _player;

    List<Vector3> wayPointPositions = new List<Vector3>();
    bool calculateColsestWayPoint;

    void Awake()
    {
        _speed = .1f;
        _rotateSpeed = 10;
        _state = State.root;
        _counter = 0;
        _wayPoints();
        calculateColsestWayPoint = true;
    }

    void Start()
    {
        _player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    void Update()
    {
        _behaviorTree(Time.deltaTime);
    }

    void _movement(float deltaTime)
    {
        if (_state == State.patrol)
        {
            if (_calculateWayPoint())
            {
                for (int i = 0; i < wayPointPositions.Count; i++)
                {
                    transform.LookAt(wayPointPositions[i]);
                    Vector3 enemyPos = transform.position;
                    Vector3 direction = enemyPos - ((Vector3)wayPointPositions[i]);
                    direction.Normalize();

                    enemyPos.z += _speed * -direction.z * deltaTime;
                    enemyPos.x += _speed * -direction.x * deltaTime;

                    transform.position = (enemyPos);
                }
            }
        }
        else if (_state == State.move)
        {
            transform.LookAt(_player.transform.position);

            Vector3 enemyPos = transform.position;
            Vector3 direction = enemyPos - _player.transform.position;
            direction.Normalize();

            enemyPos.z += _speed * -direction.z * deltaTime;
            enemyPos.x += _speed * -direction.x * deltaTime;

            transform.position = (enemyPos);
        }
    }

// Check in which state the enemy by conditions that has to do with the player
void _behaviorTree(float deltaTime)
{
        Vector3 minionPos = transform.position;
    if (Vector3.Distance(minionPos, transform.position) > 15)
    {
        _state = State.patrol;
    }
    else
    {
        _state = State.combat;
    }

    if (_state == State.combat && Vector3.Distance(minionPos, transform.position) < 15 && Vector3.Distance(minionPos, transform.position) > 7)
    {
        _state = State.move;
    }
    if (_state == State.move || _state == State.patrol)
    {
        _movement(deltaTime);
    }
    if (Vector3.Distance(minionPos, transform.position) <= 7)
    {
        _state = State.attack;
    }
    if (_state == State.attack)
    {
            transform.LookAt(_player.transform.position);
            _attack(deltaTime);
    }
}

void _attack(float deltaTime)
{
    if (_counter <= 0)
    {
            FireBall tempFireBall = Instantiate(fireBall);
        fireBallList.Add(fireBall);
        _counter = _shootCoolDown;
    }

    for (int i = 0; i < fireBallList.Count; i++)
    {
        if (fireBallList[i] != null)
        {
            fireBallList[i].attack(deltaTime);
            if (fireBallList[i].isMarkedForDeletion())
            {
                    FireBall deletionObject = fireBallList[i];
                    fireBallList.RemoveAt(i);
                    Destroy(deletionObject);
            }
        }
    }
    _counter -= deltaTime;
}

void _wayPoints()
{
    wayPointPositions.Add(new Vector3(-25, 1.2f, 20));
    wayPointPositions.Add(new Vector3(25, 1.2f, 20));
    wayPointPositions.Add(new Vector3(-25, 1.2f, -20));
    wayPointPositions.Add(new Vector3(25, 1.2f, -20));
}

bool _calculateWayPoint()
{
        Vector3 enemyPos = transform.position;
    List<float> closest = new List<float>();
    for (int i = 0; i < wayPointPositions.Count; i++)
    {
        for (int j = 0; j < wayPointPositions.Count; j++)
        {
            closest.Add(Vector3.Distance(wayPointPositions[i], enemyPos));

            if (Vector3.Distance(enemyPos, wayPointPositions[i]) == closest[j])
            {
                if (i == j)
                {
                    continue;
                }
                else
                {
                    if (Vector3.Distance(enemyPos, wayPointPositions[i]) <= closest[j])
                    {
                        return true;
                    }
                }
            }
        }
    }
    return false;
}
}
