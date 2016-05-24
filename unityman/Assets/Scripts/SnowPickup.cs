using UnityEngine;
using System.Collections;

public class SnowPickup : MyGameObject
{
    Player _player;
    float _gainHealth;

    void Awake()
    {
        _gainHealth = 10;
    }

    void Start()
    {
        _player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    //Check if player grabs a snow pile
    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            //if true --> Make player gain health
            _player.updateHealth(_gainHealth);
            Destroy(gameObject);
        }
    }
}
