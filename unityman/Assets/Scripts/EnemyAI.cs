using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyAI : MyGameObject
{
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

    int _randomcorner;
    Vector3 _corner1;
    Vector3 _corner2;
    Vector3 _corner3;
    Vector3 _corner4;

    void Awake()
    {
        _speed = .1f;
        _rotateSpeed = 10;
        _state = State.root;
        _counter = 0;
        _corner1 = new  Vector3(-20, 1, 24);
        _corner2 = new Vector3(30, 1, 30);
        _corner3 = new Vector3(25, 1, -30);
        _corner4 = new Vector3(-26, 1, -34);

        _getRandomcorner();
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
            _findNewTarget();
            /*if (_calculateWayPoint())
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
            }*/
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
        if (Vector3.Distance(minionPos, _player.transform.position) > 15)
        {
            _state = State.patrol;
        }
        else
        {
            _state = State.combat;
        }

        if (_state == State.combat && Vector3.Distance(minionPos, _player.transform.position) < 15 && Vector3.Distance(minionPos, _player.transform.position) > 7)
        {
            _state = State.move;
        }
        if (_state == State.move || _state == State.patrol)
        {
            _movement(deltaTime);
        }
        if (Vector3.Distance(minionPos, _player.transform.position) <= 7)
        {
            _state = State.attack;
        }
        if (_state == State.attack)
        {
            transform.LookAt(_player.transform.position);
            _createFireBall(deltaTime);
        }
        if (_state == State.attack || _state == State.move || _state == State.patrol || _state == State.combat)
        {
            _attack(deltaTime);
        }
    }
    void _createFireBall(float deltaTime)
    {
        if (_counter <= 0)
        {
            FireBall tempFireBall = (FireBall)Instantiate(fireBall, transform.position, new Quaternion());
            fireBallList.Add(tempFireBall);
            _counter = _shootCoolDown;
        }
        _counter -= deltaTime;

    }
    void _attack(float deltaTime)
    {
        if (fireBallList.Count > 0)
        {
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
        }
    }

    Vector3 _corner()
    {
        switch (_randomcorner)
        {
            case 1:
                return _corner1;
                break;
            case 2:
                return _corner2;
                break;
            case 3:
                return _corner3;
                break;
            case 4:
                return _corner4;
                break;
        }

        return new Vector3(0, 0, 0);
    }
    int _getRandomcorner()
    {

        return _randomcorner = Random.Range(1,5);
    }
    void _findNewTarget()
    {
        //	_sceneNode->setRotation(_faceTarget(_gridPos[randomTarget]));
        Vector3 enemyPos = transform.position;
        Vector3 direction = (_corner() - enemyPos).normalized;

        //enemyPos.z += _speed * direction.z * Time.deltaTime;
        //enemyPos.x += _speed * direction.x * Time.deltaTime;

        //transform.position = (enemyPos);

        if (Vector3.Distance(_corner1, transform.position) < 5.0f ||
            Vector3.Distance(_corner2, transform.position) < 5.0f ||
            Vector3.Distance(_corner3, transform.position) < 5.0f ||
            Vector3.Distance(_corner4, transform.position) < 5.0f)

            /* (vector2df(_sceneNode->getPosition().X, _sceneNode->getPosition().Z)) < 5.0f*/
            /* || vector2df(_corner2.X, _corner2.Z).getDistanceFrom(vector2df(_sceneNode->getPosition().X, _sceneNode->getPosition().Z)) < 5.0f
             || vector2df(_corner3.X, _corner3.Z).getDistanceFrom(vector2df(_sceneNode->getPosition().X, _sceneNode->getPosition().Z)) < 5.0f
             || vector2df(_corner4.X, _corner4.Z).getDistanceFrom(vector2df(_sceneNode->getPosition().X, _sceneNode->getPosition().Z)) < 5.0f)*/
        {
            _getRandomcorner();
        }
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
