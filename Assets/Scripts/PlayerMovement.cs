using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private Transform _rotationCenter;

    [SerializeField]
    private TMPro.TMP_Text _scoreText;

    [SerializeField]
    private AudioClip _coinClip, _explosionClip;

    private Rigidbody2D _rb;

    private bool _canMove;
    private bool _canJump;
    private int _score;

    [SerializeField]
    private float _gravityForce, _moveForce, _jumpForce;

    private void Awake()
    {
        _canJump = false;
        _canMove = false;
        _rb = GetComponent<Rigidbody2D>();
        _score = 0;
        _scoreText.text = _score.ToString();
    }

    private void OnEnable()
    {
        EventManager.StartListening(Constants.EventNames.GAME_START,GameStart);
        EventManager.StartListening(Constants.EventNames.GAME_OVER, GameOver);
    }

    private void OnDisable()
    {
        EventManager.StopListening(Constants.EventNames.GAME_START, GameStart);
        EventManager.StopListening(Constants.EventNames.GAME_OVER, GameOver);
    }

    private void GameStart(Dictionary<string,object> message)
    {
        _canMove = true;
    }

    private void GameOver(Dictionary<string, object> message)
    {
        _canMove = false;
        _rb.velocity = Vector2.zero;
    }

    private void Update()
    {
        if (!_canMove) return;
        if(Input.GetMouseButton(0))
        {
            _canJump = true;
        }
        else
        {
            _canJump = false;
        }
    }

    private void FixedUpdate()
    {
        if (!_canMove) return;
        _rb.velocity = _rb.velocity * 0.9f;
        Vector3 gravityDirection = _rotationCenter.position - transform.position;
        _rb.AddForce(gravityDirection.normalized * _gravityForce, ForceMode2D.Force);
        Vector3 forwardDirection = Vector3.Cross(Vector3.forward,gravityDirection.normalized).normalized;
        _rb.AddForce(forwardDirection.normalized * _moveForce);
        transform.rotation = Quaternion.LookRotation(Vector3.forward, -gravityDirection.normalized);
        if(_canJump)
        {
            _rb.AddForce(- gravityDirection.normalized * _jumpForce * Time.deltaTime, ForceMode2D.Impulse);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag(Constants.Tags.OBSTACLE))
        {
            EventManager.TriggerEvent(Constants.EventNames.GAME_OVER, null);
            EventManager.TriggerEvent(Constants.EventNames.UPDATE_SCORE, new Dictionary<string, object>()
            {
                { Constants.EventParams.SCORE, _score }
            });
            AudioManager.instance.PlaySound(_explosionClip);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag(Constants.Tags.GOAL))
        {
            _score++;
            _scoreText.text = _score.ToString();
            EventManager.TriggerEvent(Constants.EventNames.SPAWN_SPOKE,null);
            AudioManager.instance.PlaySound(_coinClip);
        }
    }
}
