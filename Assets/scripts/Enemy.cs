using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("__________Settings__________")]
    [Min(0f)][SerializeField] float _speed = 1.0f;
    [SerializeField] bool _enemyIdle = true;
    [Min(0f)][SerializeField] float _idleTime = 1f;
    [Min(0.01f)][SerializeField] float minDystans = 0.1f;
    [SerializeField] bool _enemyDefultStartPos = true;

    [Header("__________Edit__________")]
    [SerializeField] bool ADD_POS = false;
    [SerializeField] bool CLEAR_POS = false;

    [Header("__________Debug__________")]
    [SerializeField] bool _showDebug = false;
    [Range(0.1f, 2f)][SerializeField] float _debugSize = .5f;
    [SerializeField] Color _debugColor = Color.white;
    [SerializeField] Color _debugFollowPathColor = Color.green;
    int _followPointIndex;

    [Header("__________Path__________")]
    [SerializeField] List<Vector3> _path = new List<Vector3>();


    float _timer;
    bool _ideling = false;

    void _MoveToPoint()
    {
        // Call the _Idle function
        _Idle();

        // Check if the object is ideling
        if (_ideling)
            return;

        // Calculate the direction from the current position to the target position
        Vector3 direction = _path[_followPointIndex] - transform.position;
        // Normalize the direction to get a unit vector (a vector of length 1)
        direction = direction.normalized;
        // Calculate the velocity by multiplying the direction by the speed
        Vector3 velocity = direction * _speed;
        // Move the transform by the velocity
        transform.position += velocity * Time.deltaTime;

        // Check if the minimum distance to the next point has been reached
        if (minDystans >= (_path[_followPointIndex] - transform.position).magnitude)
        {
            // Call the _NextPoint function
            _NextPoint();
            // Set the ideling flag to true
            _ideling = true;
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
        if (_enemyIdle && _ideling)
        {
            _timer += Time.deltaTime;
            if (_timer >= _idleTime)
            {
                _timer = 0;
                _ideling = false;
            }
        }
    }


    private void OnDrawGizmos()
    {
        // Check if debugging is enabled
        if (!_showDebug)
            return;

        // Set the color for the gizmos
        Gizmos.color = _debugColor;

        // Loop through each point in the path
        for (int i = 0; i < _path.Count; i++)
        {
            // Check if the current point is the follow point index
            if (_followPointIndex == i)
                Gizmos.color = _debugFollowPathColor;

            // Draw a sphere at the current point
            Gizmos.DrawSphere(_path[i], _debugSize);

            // Change the color back to the original color
            if (_followPointIndex == i)
                Gizmos.color = _debugColor;
        }

        // Loop through each point in the path (excluding the last point)
        for (int i = 0; i < _path.Count - 1; i++)
        {
            // Check if the current point is the follow point index
            if (_followPointIndex == i)
                Gizmos.color = _debugFollowPathColor;

            // Draw a line between the current point and the next point
            Gizmos.DrawLine(_path[i], _path[i + 1]);

            // Change the color back to the original color
            if (_followPointIndex == i)
                Gizmos.color = _debugColor;
        }

        // Draw a line between the last point and the first point
        Gizmos.DrawLine(_path[_path.Count - 1], _path[0]);
    }

    private void OnValidate()
    {
        if (ADD_POS)
        {
            ADD_POS = false;
            _path.Add(transform.position);
        }
        if (CLEAR_POS)
        {
            CLEAR_POS = false;
            _path.Clear();
            if (_enemyDefultStartPos)
            {
                _path.Add(transform.position);
            }
        }

    }
    void Start()
    {
        if (_enemyDefultStartPos)
        {
            transform.position = _path[0];
        }
    }

    void Update()
    {
        _MoveToPoint();
    }
}