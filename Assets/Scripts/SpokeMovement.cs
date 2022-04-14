using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpokeMovement : MonoBehaviour
{

    [SerializeField]
    private GameObject _centerBox, _wallB, _wallT;

    [SerializeField]
    private float _minHeight, _maxHeight, _centerOffset;

    [SerializeField]
    private float _startSizeSpeedIncrease, _rotationSpeed, _oscillationSpeed;

    private bool _hasGameFinished;
    private SpriteRenderer _wallBRenderer, _wallTRenderer;
    private BoxCollider2D _wallBCollider, _wallTCollider, _centerBoxCollider;

    private void Awake()
    {
        _hasGameFinished = false;
        _wallBRenderer = _wallB.GetComponent<SpriteRenderer>();
        _wallTRenderer= _wallT.GetComponent<SpriteRenderer>();
        _wallBCollider= _wallB.GetComponent<BoxCollider2D>();
        _wallTCollider = _wallT.GetComponent<BoxCollider2D>();
        _centerBoxCollider = _centerBox.GetComponent<BoxCollider2D>();
    }

    private void OnEnable()
    {
        EventManager.StartListening(Constants.EventNames.GAME_OVER, GameOver);
    }

    private void OnDisable()
    {
        EventManager.StopListening(Constants.EventNames.GAME_OVER, GameOver);
    }

    private void GameOver(Dictionary<string, object> message)
    {
        _hasGameFinished = true;
    }

    public void SetFirstSpoke()
    {
        _centerBox.tag = Constants.Tags.GOAL;
    }

    public void SetSpokeParams(float delay)
    {
        StartCoroutine(StartResize(delay));
    }

    IEnumerator StartResize(float delay)
    {
        _centerBox.SetActive(false);
        Vector2 tempSize = _wallBCollider.size;
        tempSize.y = 0f;
        _wallBCollider.size = tempSize;
        _wallBRenderer.size = tempSize;
        tempSize = _wallTCollider.size;
        tempSize.y = 0f;
        _wallTCollider.size = tempSize;
        _wallTRenderer.size = tempSize;

        yield return new WaitForSeconds(delay > 2f ? 0f : delay);

        float spokeHeight = (_minHeight + _maxHeight - _centerOffset) / 2f;

        while(_wallBCollider.size.y < spokeHeight || _wallTCollider.size.y < spokeHeight)
        {
            tempSize = _wallBCollider.size;
            tempSize.y += Time.deltaTime * _startSizeSpeedIncrease;
            _wallBCollider.size = tempSize;
            _wallBRenderer.size = tempSize;

            tempSize = _wallTCollider.size;
            tempSize.y += Time.deltaTime * _startSizeSpeedIncrease;
            _wallTCollider.size = tempSize;
            _wallTRenderer.size = tempSize;

            yield return null;
        }

        tempSize = _wallBCollider.size;
        tempSize.y = spokeHeight;
        _wallBCollider.size = tempSize;
        _wallBRenderer.size = tempSize;

        tempSize = _wallTCollider.size;
        tempSize.y = spokeHeight;
        _wallTCollider.size = tempSize;
        _wallTRenderer.size = tempSize;

        _centerBox.SetActive(true);

        StartCoroutine(SpokeRotate(delay > 5f ? 0f : 1 - delay));
        StartCoroutine(SpokeOscillate(delay > 5f ? 0f : delay));
    }

    IEnumerator SpokeRotate(float delay)
    {
        yield return new WaitForSeconds(delay);

        while(!_hasGameFinished)
        {
            transform.Rotate(0,0,_rotationSpeed * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator SpokeOscillate(float delay)
    {
        yield return new WaitForSeconds(delay);

        float center = (_minHeight + _maxHeight - _centerOffset) / 2f;
        float startCenter = center;
        Vector2 tempSize;
        float currentSpeed = _oscillationSpeed;

        while (!_hasGameFinished)
        {

            center += Time.deltaTime * currentSpeed;

            tempSize = _wallBCollider.size;
            tempSize.y += Time.deltaTime * currentSpeed;
            _wallBCollider.size = tempSize;
            _wallBRenderer.size = tempSize;
            _wallBCollider.offset = new Vector2(0, tempSize.y / 2f);

            tempSize = _wallTCollider.size;
            tempSize.y -= Time.deltaTime * currentSpeed;
            _wallTCollider.size = tempSize;
            _wallTRenderer.size = tempSize;
            _wallTCollider.offset = new Vector2(0,-tempSize.y / 2f);

            _centerBoxCollider.offset = new Vector2(0, center - startCenter);

            if (center < _minHeight || center > _maxHeight) currentSpeed *= -1f;

            yield return null;
        }
    }
}
