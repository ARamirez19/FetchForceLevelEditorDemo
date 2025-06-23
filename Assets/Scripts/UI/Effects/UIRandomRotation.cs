using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRandomRotation : MonoBehaviour
{
    private Quaternion _targetRotation = Quaternion.identity;
    private float _timer = 0f;
    private float _speed = 0f;

    void Update()
    {
        if (_timer <= 0f)
        {
            PickNewRandom();
        }
        else
        {
            RotateTowards();
            _timer -= Time.deltaTime;
        }
    }

    private void RotateTowards()
    {
        transform.rotation = Quaternion.Lerp(this.transform.rotation, _targetRotation, _speed * Time.deltaTime);
    }

    private void PickNewRandom()
    {
        _targetRotation = Quaternion.Euler(Random.Range(-15f, 15f), Random.Range(-15f, 15f), Random.Range(-15f, 15f));
        _timer = Random.Range(0.2f, 0.5f);
        _speed = Random.Range(0.2f, 1.5f);
    }
}
