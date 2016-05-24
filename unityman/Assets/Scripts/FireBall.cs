using UnityEngine;
using System.Collections;

public class FireBall : MyGameObject
{
    public float damage;

    public float _speed;

    private Vector3 _oldPlayerPos;
    private Vector3 _playerPos;
    private float _timLeft;
    private Player _player;

    void Awake()
    {
        _timLeft = 1.5f;
        //_fireBallSound = _soundEngine->play3D("../media/fireBallSound.ogg", _sceneNode->getPosition(), false, true, false);
    }

    void Start()
    {
        _player = GameObject.FindWithTag("Player").GetComponent<Player>();
        _playerPos = _player.transform.position;
        _oldPlayerPos = _player.transform.position;
    }

    public void attack(float deltaTime)
    {
        //_fireBallSound->setIsPaused(false);
        _timLeft -= deltaTime;
        Vector3 firePos = transform.position;
        Vector3 direction = firePos - _oldPlayerPos;
        direction.Normalize();

        firePos.x += _speed * -direction.x * deltaTime;
        firePos.z += _speed * -direction.z * deltaTime;
        //_fireBallSound->setPosition(firePos);
        transform.position = (firePos);

        if (_timLeft <= 0)
        {
            Destroy(this.gameObject);
        }

        if (GetComponent<MeshFilter>().mesh.bounds.Contains(_player.transform.position))
        {
            doDamage();
            Destroy(this.gameObject);
        }
    }

    Vector3 getOldPlayerPos()
    {
        return _oldPlayerPos;
    }

    void doDamage()
    {
        _player.updateHealth(damage);
    }
}
