using UnityEngine;
using System.Collections;

public class Player : MyGameObject
{
    public GameObject snowball;
    bool inShadow;
    Vector3 resetLocation;
    Vector3 startLocation;

    Vector3 nodePosition;

    private const int _numberOfMeshes = 10;
    public Mesh[] _playerMeshes = new Mesh[_numberOfMeshes];
    const float _snowballHealthCost = 0.5f;
    
    public InputHandler _inputHandler;

	// snowball variables
	float _speed;
    const float _throwCooldown = 0.5f;
    float _throwTimer;
    Vector3 _startPosition;

    bool _canSpawnSnowBall;
    SnowBall _snowBall;
    Vector3 _force;

    // rotate player variables
    Quaternion _rotation;
    float _speedRotY;
    float _scale;
    float _movementSpeed;
    float _gravity;
    float _jumpGravity;
    float _fallcounter;
    bool _grounded;

    // health bar variables

    float _jumpForce;

    // health bar
    int _xBar;
    int _yBar;
    float _maxHealthValue;
    float _currentHealthValue;

    // delta time
    float _deltaTime;

    void Awake()
    {
        _name = "Player";
        _grounded = false;
        
        _gravity = -5.5f;
        
        _speedRotY = 100;
        inShadow = false;
        _scale = 1;
        _xBar = 10;
        _yBar = 10;
        _maxHealthValue = 100;

        _movementSpeed = 5;
        _currentHealthValue = _maxHealthValue;
        _jumpForce = 0.2f;
        _jumpGravity = -0.5f;
        _speed = 5;
        _fallcounter = 10;
        _canSpawnSnowBall = false;
        _throwTimer = 0.0f;
        _snowBall = null;
        _deltaTime = 0;

    }

    void Start()
    {
    }

    void Update()
    {
        _deltaTime = Time.deltaTime;

        _fallcounter += _deltaTime;

        // make the model a little bit smaller and normalize its normals
        GetComponent<MeshFilter>().mesh = _playerMeshes[(int)((_currentHealthValue / _maxHealthValue) * (_numberOfMeshes - 1))];

        _move(_deltaTime);

        _throwTimer -= _deltaTime;
        if (_inputHandler.fire)
        {
            if (_throwTimer <= 0)
            {
                _throwSnowBall();
                _throwTimer = _throwCooldown;
            }
        }

        if (_currentHealthValue <= 0)
        {
            _currentHealthValue = 0;
            //TODO: set player dead, game over screen or menu
        }

        // reset player if he falls out of the level
        if (transform.position.y < -1)
        {
            spawnPlayer(0);
        }

    }

    void _move(float deltaTime)
    {
        nodePosition = transform.position;
        _rotation = transform.rotation;

        float roty_rad = _rotation.y * Mathf.PI / 180;

        if ((_inputHandler.up))
        {
            nodePosition.z += _movementSpeed * deltaTime * -Mathf.Cos(roty_rad);
            nodePosition.x += _movementSpeed * deltaTime * -Mathf.Sin(roty_rad);
        }
        else if ((_inputHandler.down))
        {
            nodePosition.z -= _movementSpeed * deltaTime * -Mathf.Cos(roty_rad);
            nodePosition.x -= _movementSpeed * deltaTime * -Mathf.Sin(roty_rad);
        }

        if ((_inputHandler.left))
        {
            _rotation.y -= _speedRotY * deltaTime;
        }
        else if ((_inputHandler.right))
        {
            _rotation.y += _speedRotY * deltaTime;
        }

        if ((_inputHandler.left) && (_inputHandler.right) ||
            (_inputHandler.up) && (_inputHandler.down))
        {
            nodePosition = transform.position;
            _rotation = transform.rotation;

            if ((_inputHandler.jump) && !isGrounded())
            {
                GetComponent<Rigidbody>().AddForce(0, _jumpForce, 0);
            }

            GetComponent<Rigidbody>().AddForce((nodePosition - transform.position).normalized);
            //transform.position = nodePosition;
            transform.rotation = (_rotation);
        }
    }

    bool isGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, GetComponent<MeshRenderer>().bounds.center.y + 0.1f);
    }

    // player can throw snowballs when the mouse is pressed
    void _throwSnowBall()
    {
        //create a ray with its starting point being the mouse cursor. This method automatically converts screen coordindates to world coordinates if the correct camera is provided
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100.0f))
        {
            Vector3 throwDestination = hit.point;
            //make sure we always throw at the same height as the player
            throwDestination.y = transform.position.y;
            //add a new snowball to our snowball list
            //_snowBallList.push_back(new SnowBall("SnowBall", _triangleSelector, getSceneNode().getPosition(), throwDestination));
            SnowBall b = ((GameObject)Instantiate(snowball, transform.position, new Quaternion())).GetComponent<SnowBall>();
            b.targetPosition = throwDestination;
        }
        updateHealth(-_snowballHealthCost);
    }

    // Update snowman health and scale
    public void updateHealth(float value)
    {
        _currentHealthValue += value;
        if (_currentHealthValue > _maxHealthValue)
        {
            _currentHealthValue = _maxHealthValue;
        }
    }

    public float getHealthPercentage()
    {
        return (_currentHealthValue / _maxHealthValue) * 100;
    }

   /*void renderGUI()
    {
        //Create health bar and change the color from blue to red when it's decreased
        _videoDriver.draw2DRectangle(core::rect<int>(_xBar + 3, _yBar + 3, static_cast<int>(_currentHealthValue) * 2 + _xBar - 3, (_yBar + 40) - 3),
        video::SColor(255, 255 - static_cast<int>(_currentHealthValue * 2.55f), 0, 0 + static_cast<int>(_currentHealthValue * 2.55f)),
        video::SColor(255, 255 - static_cast<int>(_currentHealthValue * 2.55f), 0, 0 + static_cast<int>(_currentHealthValue * 2.55f)),
        video::SColor(255, 255 - static_cast<int>(_currentHealthValue * 2.55f), 0, 0 + static_cast<int>(_currentHealthValue * 2.55f)),
        video::SColor(255, 255 - static_cast<int>(_currentHealthValue * 2.55f), 0, 0 + static_cast<int>(_currentHealthValue * 2.55f)));
    }*/

    void startPosition(Vector3 pos)
    {
        //gets the new position for the player to restart on
        startLocation = pos;
    }

    void spawnPlayer(int levelNumber)
    {
        transform.position = (_startPosition);
    }
}
