using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Renderer))]
public class PrefabCube : MonoBehaviour
{
    private Renderer _materialColor;
    private Coroutine _coroutine;

    private bool _haveCollided;
    private bool _isTimerExpired;

    private Action<PrefabCube> _destroyAction;

    public void InitializeAction(Action<PrefabCube> destroyAction)
    {
        _destroyAction = destroyAction;
    }

    private void Awake()
    {
        _materialColor = GetComponent<Renderer>();
        ResetChanges();
    }

    private void Update()
    {
        if (_isTimerExpired)
        {
            _destroyAction(this);
            ResetChanges();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(_haveCollided == false)
        {
            _haveCollided = true;

            ChangeColor();
            _coroutine = StartCoroutine(LifetimeTimer());
        }
    }

    private void ResetChanges()
    {
        Color defaultColor = Color.gray;

        _materialColor.material.color = defaultColor;
        _haveCollided = false;
        _isTimerExpired = false;

        if(_coroutine != null)
            StopCoroutine(_coroutine);
    }

    private void ChangeColor()
    {
        _materialColor.material.color = Random.ColorHSV();
    }

    private IEnumerator LifetimeTimer()
    {
        float minLifetime = 2f;
        float maxLifetime = 5f;

        float lifeTime = Random.Range(minLifetime, maxLifetime);

        yield return new WaitForSecondsRealtime(lifeTime);

        _isTimerExpired = true;
    }
}