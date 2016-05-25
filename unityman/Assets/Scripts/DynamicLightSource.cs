using UnityEngine;
using System.Collections;

public class DynamicLightSource : MyGameObject
{
    public GameObject _parentNode;
    private Player _player;
    private float _damage = -5;
    private float _lightRadius;
    Vector3 _targetRotation;
    private float newRotationY;
    //   Vector3 _targetPosition; 
    Vector3 _lastKnownPlayerPos;
    const float _searchCooldown = 2.0f;
    float _searchTimer;
    const float _baseRotationSpeed = 1.0f;
    float _rotationSpeed;
    float _movementSpeed;
    
    private bool isDamaging;
    void Awake()
    {
        isDamaging = false;
        _targetRotation = new Vector3(0, 270, 0);
        newRotationY = 90;
        // _parentNode.transform.eulerAngles = (_targetRotation);
        _rotationSpeed = _baseRotationSpeed;
        _movementSpeed = 5.0f;
        _searchTimer = _searchCooldown;
        //    _parentNode.transform.position = (_targetPosition);
    }

    void Start()
    {
        _player = GameObject.FindWithTag("Player").GetComponent<Player>();
        //  _targetPosition = _player.transform.position;
        _lastKnownPlayerPos = _player.transform.position;
    }
    
    void Update()
    {
        _damagePlayer();
        //float angle = Quaternion.Angle(Quaternion.Euler(new Vector3(0, 0, 0)), transform.rotation);
        //_parentNode->setRotation(vector3df(_xRotation, _yRotation, _zRotation));
        if (isDamaging)
        {
            /*Vector3 targetDir = target.position - transform.position;

            float step = _rotationSpeed * Time.deltaTime;

            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);

            transform.rotation = Quaternion.LookRotation(newDir);
            newRotationY = transform.rotation.y;*/
            //_parentNode.transform.position += (_player.transform.position - _parentNode.transform.position).normalized * Time.deltaTime * 5.0f;
            /*if ((target.position - _parentNode.transform.position).magnitude > 1)
            {
                _parentNode.transform.position = (_parentNode.transform.position + ((target.position - _parentNode.transform.position).normalized * _movementSpeed * Time.deltaTime));
            }*/
            _searchTimer = _searchCooldown;
            //Vector3 target = new Vector3(_player.transform.position.x, _parentNode.transform.position.y, _player.transform.position.z);
            //transform.LookAt(target);
        }
        //search
        else
        {
            _searchTimer -= Time.deltaTime;
            if (_searchTimer <= 0)
            {
                if ((_targetRotation - _parentNode.transform.eulerAngles).magnitude > 1)
                {
                    _parentNode.transform.eulerAngles = (_parentNode.transform.eulerAngles + ((_targetRotation - _parentNode.transform.eulerAngles).normalized * _rotationSpeed * Time.deltaTime));

                }
            }
            // _parentNode.transform.Rotate(new Vector3(0, Time.deltaTime * _rotationSpeed, 0));
            //newRotationY += 5.0f * Time.deltaTime;
            //transform.rotation = new Quaternion(transform.rotation.x + 30, newRotationY, transform.rotation.z, transform.rotation.w);
        }
        /*if ((_targetRotation - _parentNode.transform.eulerAngles).magnitude > 1)
         {
             _parentNode.transform.eulerAngles = (_parentNode.transform.eulerAngles + ((_targetRotation - _parentNode.transform.eulerAngles).normalized * _rotationSpeed * Time.deltaTime));
         }
         if ((target.position - _parentNode.transform.position).magnitude > 1)
         {
             _parentNode.transform.position = (_parentNode.transform.position + ((target.position - _parentNode.transform.position).normalized * _movementSpeed * Time.deltaTime));
         }*/
    }

    void _damagePlayer()
    {
        //create a ray with its starting point being the mouse cursor. This method automatically converts screen coordindates to world coordinates if the correct camera is provided
        Ray ray = new Ray(_player.transform.position, transform.position);
        //Debug.DrawRay(ray.origin, ray.direction * 1000);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100.0f))
        {
            isDamaging = false;
            _searchTimer -= Time.deltaTime;
            if (_searchTimer <= 0)
            {
                //   _targetRotation = _parentNode.transform.eulerAngles;
                _targetRotation.y += 5;
            }
                //  _targetPosition = _parentNode.transform.position;
        }
        else {
            isDamaging = true;
            _player.updateHealth(_damage * Time.deltaTime);
            //_targetRotation = _parentNode.transform.eulerAngles;
            //    _targetPosition = _player.transform.position;
            _searchTimer = _searchCooldown;
            _rotationSpeed = (_baseRotationSpeed * 2.0f) - ((_baseRotationSpeed * _player.getHealthPercentage() / 100.0f));
        }

    }
}
