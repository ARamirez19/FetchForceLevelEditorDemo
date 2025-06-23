using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRandomPosition : MonoBehaviour
{
    private Vector3 _rootPosition = Vector3.zero;
    private Vector3 _targetPosition = Vector3.zero;
    private float _timer = 0f;
    private float _speed = 0f;
    public float _moveValue = 80.0f;

    void Start()
    {
        _rootPosition = transform.localPosition;
    }

    void Update()
    {
        if (_timer <= 0f)
        {
            PickNewRandom();
        }
        else
        {
            MoveTowards();
            _timer -= Time.deltaTime;
        }
    }

    private void MoveTowards()
    {
        transform.localPosition = Vector3.Lerp(this.transform.localPosition, _targetPosition, _speed * Time.deltaTime);
    }

    private void PickNewRandom()
    {
        _targetPosition = _rootPosition + new Vector3(Random.Range(-_moveValue, _moveValue), Random.Range(-_moveValue, _moveValue), 0f);
        _timer = Random.Range(0.6f, 0.9f);
        _speed = Random.Range(0.5f, 1f);
    }
}
