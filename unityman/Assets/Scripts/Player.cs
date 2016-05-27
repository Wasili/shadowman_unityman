using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    public float _jumpForce;

    // health bar
    int _xBar;
    int _yBar;
    float _maxHealthValue;
    float _currentHealthValue;

    // delta time
    float _deltaTime;

    public AudioSource bridgeSound;
    public AudioSource tutorialBackgroundSound1;
    public AudioSource tutorialBackgroundSound2;
    public AudioSource tutorialLevel2Sound1;
    public AudioSource tutorialLevel2Sound2;


    Image healthBar;
    public GameObject gameOverScreen;

    void Awake()
    {
        _startPosition = transform.position;
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
        //_jumpForce = 1750f;
        //_jumpGravity = -0.5f;
        _speed = 5;
        _fallcounter = 10;
        _canSpawnSnowBall = false;
        _throwTimer = 0.0f;
        _snowBall = null;
        _deltaTime = 0;

    }

    void Start()
    {
        healthBar = GameObject.FindWithTag("HealthBar").GetComponent<Image>();
        Time.timeScale = 1;
    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.L))
        {
            Instantiate(snowball, transform.position, new Quaternion());
        }


        _deltaTime = Time.deltaTime;

        _fallcounter += _deltaTime;

        // make the model a little bit smaller and normalize its normals
        GetComponent<MeshFilter>().mesh = _playerMeshes[(int)((_currentHealthValue / _maxHealthValue) * (_numberOfMeshes - 1))];
        GetComponent<CapsuleCollider>().height = GetComponent<MeshFilter>().mesh.bounds.size.y;
        GetComponent<CapsuleCollider>().radius = 0.8f * (GetComponent<CapsuleCollider>().height / 2.2f);

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
            Time.timeScale = 0;
            gameOverScreen.SetActive(true);
            //TODO: set player dead, game over screen or menu
        }

        // reset player if he falls out of the level
        if (transform.position.y < -1)
        {
            spawnPlayer(0);
        }

        healthBar.color = new Color((255 - (_currentHealthValue * 2.55f)) / 100, 0, (0 + (_currentHealthValue * 2.55f)) / 100);
        healthBar.fillAmount = _currentHealthValue / _maxHealthValue;
    }

    void _move(float deltaTime)
    {
        nodePosition = transform.position;
        _rotation = transform.rotation;

        float roty_rad = _rotation.y * Mathf.PI / 180;

        if ((_inputHandler.up))
        {
            gameObject.transform.Translate(new Vector3(_movementSpeed * deltaTime * -Mathf.Sin(roty_rad), 0, _movementSpeed * deltaTime * -Mathf.Cos(roty_rad)));
        }
        else if ((_inputHandler.down))
        {
            gameObject.transform.Translate(new Vector3(-_movementSpeed * deltaTime * -Mathf.Sin(roty_rad), 0, -_movementSpeed * deltaTime * -Mathf.Cos(roty_rad)));
        }

        if ((_inputHandler.left))
        {
            gameObject.transform.Rotate(new Vector3(0, -_speedRotY * deltaTime, 0));
        }
        else if ((_inputHandler.right))
        {
            gameObject.transform.Rotate(new Vector3(0, _speedRotY * deltaTime, 0));
        }

        if ((_inputHandler.left) && (_inputHandler.right) ||
            (_inputHandler.up) && (_inputHandler.down))
        {
            nodePosition = transform.position;
            _rotation = transform.rotation;

            GetComponent<Rigidbody>().AddForce((nodePosition - transform.position).normalized);
            //transform.position = nodePosition;
            transform.rotation = (_rotation);
        }
        if ((_inputHandler.jump) && isGrounded())
        {
            GetComponent<Rigidbody>().AddForce(0, _jumpForce, 0);
            GetComponent<AudioSource>().Play();
            _inputHandler.jump = false;
        }

        if(transform.position.z > 113.5 && transform.position.z < 123 && transform.position.x < 21 && 
            transform.position.x > 18 && SceneManager.GetActiveScene().ToString() == "Level1")
        {
            if(!bridgeSound.isPlaying) bridgeSound.Play();
        }
        else
        {
            bridgeSound.Pause();
        }

        if (transform.position.z > 113.5 && SceneManager.GetActiveScene().ToString() == "Level1")
        {
            if (!tutorialBackgroundSound2.isPlaying) tutorialBackgroundSound2.Play();
        }
        else
        {
            tutorialBackgroundSound2.Pause();
        }

        if (transform.position.z < 113.5 && SceneManager.GetActiveScene().ToString() == "Level1")
        {
            if (!tutorialBackgroundSound1.isPlaying) tutorialBackgroundSound1.Play();
        }
        else
        {
            tutorialBackgroundSound1.Pause();
        }

        if (transform.position.z < -137 && SceneManager.GetActiveScene().ToString() == "Level2")
        {
            if (!tutorialBackgroundSound1.isPlaying) tutorialBackgroundSound1.Play();
        }
        else
        {
            tutorialBackgroundSound1.Pause();
        }

        if (transform.position.z > -137 && SceneManager.GetActiveScene().ToString() == "Level2")
        {
            if (!tutorialBackgroundSound1.isPlaying) tutorialBackgroundSound1.Play();
        }
        else
        {
            tutorialBackgroundSound1.Pause();
        }
    }

    bool isGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, (GetComponent<MeshRenderer>().bounds.size.y / 2) + 0.1f);
    }

    // player can throw snowballs when the mouse is pressed
    void _throwSnowBall()
    {
        //create a ray with its starting point being the mouse cursor. This method automatically converts screen coordindates to world coordinates if the correct camera is provided
        Ray ray = Camera.main.ScreenPointToRay(_inputHandler.mobileVersion ? new Vector3(_inputHandler.shootTouch.position.x, _inputHandler.shootTouch.position.y, 0) : Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            Vector3 throwDestination = hit.point;
            //make sure we always throw at the same height as the player
            throwDestination.y = transform.position.y;
            //add a new snowball to our snowball list
            //_snowBallList.push_back(new SnowBall("SnowBall", _triangleSelector, getSceneNode().getPosition(), throwDestination));
            SnowBall b = ((GameObject)Instantiate(snowball, transform.position, new Quaternion())).GetComponent<SnowBall>();
            b.setTargetPosition(throwDestination);
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
