using UnityEngine;
using System.Collections;

public class FinalBossProjectile : MonoBehaviour {
	public Vector3 velocity;
    float _speed;
    Player _player;
    float _snowballLifetime;
    string globalMediaPrefix;

    void Awake()
    {
        _player = GameObject.FindObjectOfType<Player>();
        _speed = 10.0f;
        _snowballLifetime = 10.0f;
    }

    void Update()
    {
        _snowballLifetime -= Time.deltaTime;


        if (_snowballLifetime <= 0)
        {
            Destroy(gameObject);
        }

        transform.position += (velocity * _speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, _player.transform.position) <= 1.0f)
        {
            _player.updateHealth(-15.0f);
            Destroy(gameObject);
        }
    }
}
