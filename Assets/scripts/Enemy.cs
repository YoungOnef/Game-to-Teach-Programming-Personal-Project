using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float _speed = 1.0f;
    [SerializeField] float _waitTime = 1f;
    [SerializeField] float minDystans = 0.1f;
    [SerializeField] bool _enemyDefultStartPos = true;
    [SerializeField] bool _enemyIdle = true;

    [SerializeField] bool _showDebug = false;
    [SerializeField] float _debugSize = .5f;
    [SerializeField] Color _debugColor = Color.red;
    int _followPointIndex;

    [SerializeField] List<Vector3> _path = new List<Vector3>();

    float _timer;
    bool _waiting = false;

    void _MoveToPoint()
    {
        _Idle();
        if (_waiting)
            return;

        // Calculate the direction from the current position to the target position
        Vector3 direction = _path[_followPointIndex] - transform.position;
        // Normalize the direction to get a unit vector (a vector of length 1)
        direction = direction.normalized;
        // Calculate the velocity by multiplying the direction by the speed
        Vector3 velocity = direction * _speed;
        // Move the transform by the velocity
        transform.position += velocity * Time.deltaTime;

        if (minDystans >= (_path[_followPointIndex] - transform.position).magnitude)
        {
            _NextPoint();
            _waiting = true;
        }
    }
    void _NextPoint()
    {
        _followPointIndex++;
        if (_followPointIndex >= _path.Count)
        {
            _followPointIndex = 0;
        }
    }
    void _Idle()
    {
        if (_enemyIdle && _waiting)
        {
            _timer += Time.deltaTime;
            if (_timer >= _waitTime)
            {
                _timer = 0;
                _waiting = false;
            }
        }
    }


    private void OnDrawGizmos()
    {
        if (!_showDebug)
            return;
        Gizmos.color = _debugColor;
        for (int i = 0; i < _path.Count; i++)
        {
            if (_followPointIndex == i)
                Gizmos.color = Color.green;

            Gizmos.DrawLine(_path[i] + Vector3.forward * _debugSize, _path[i] + -Vector3.forward * _debugSize);
            Gizmos.DrawLine(_path[i] + Vector3.right * _debugSize, _path[i] + -Vector3.right * _debugSize);

            Gizmos.color = _debugColor;
        }
        for (int i = 0; i < _path.Count - 1; i++)
        {
            Gizmos.DrawLine(_path[i], _path[i + 1]);
        }
        Gizmos.DrawLine(_path[_path.Count - 1], _path[0]);
    }

    void Start()
    {
        if (_enemyDefultStartPos)
        {
            _path.Insert(0, transform.position);
        }
    }

    void Update()
    {
        _MoveToPoint();
    }
}
