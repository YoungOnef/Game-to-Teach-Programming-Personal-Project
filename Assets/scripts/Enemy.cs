using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // This section defines the settings for the enemy behavior
    [Header("__________Settings__________")]
    [Min(0f)][SerializeField] float _speed = 1.0f; // The speed at which the enemy moves
    [SerializeField] bool _enemyIdle = true; // Whether the enemy should idle in place
    [Min(0f)][SerializeField] float _idleTime = 1f; // The amount of time the enemy should idle for
    [Min(0.01f)][SerializeField] float minDystans = 0.1f; // The minimum distance the enemy needs to be from the player
    [SerializeField] bool _enemyDefultStartPos = true; // Whether the enemy should start at its default position

    // This section allows for editing of the enemy behavior
    [Header("__________Edit__________")]
    [SerializeField] bool ADD_POS = false; // Whether to add a new position to the enemy's path
    [SerializeField] bool CLEAR_POS = false; // Whether to clear the enemy's path

    // This section enables debugging options for the enemy behavior
    [Header("__________Debug__________")]
    [SerializeField] bool _showDebug = false; // Whether to show debugging information
    [Range(0.1f, 2f)][SerializeField] float _debugSize = .5f; // The size of the debug points
    [SerializeField] Color _debugColor = Color.white; // The color of the debug points
    [SerializeField] Color _debugFollowPathColor = Color.green; // The color of the debug points that follow the enemy's path
    int _followPointIndex; // The index of the point the enemy is currently following

    // This section defines the enemy's path
    [Header("__________Path__________")]
    [SerializeField] List<Vector3> _path = new List<Vector3>(); // The list of points that make up the enemy's path



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

    // This method updates the index of the point that the enemy is following on its path
    void _NextPoint()
    {
        _followPointIndex++; // Increment the index by 1
        if (_followPointIndex >= _path.Count) // If the index exceeds the number of points in the path
        {
            _followPointIndex = 0; // Reset the index to 0 to start over at the beginning of the path
        }
    }

    // This method controls the idle behavior of the enemy
    void _Idle()
    {
        if (_enemyIdle && _ideling) // If the enemy is set to idle and is currently idling
        {
            _timer += Time.deltaTime; // Increment the timer by the amount of time that has passed since the last frame
            if (_timer >= _idleTime) // If the timer has reached the idle time
            {
                _timer = 0; // Reset the timer to 0
                _ideling = false; // Set the idling flag to false to indicate that the enemy is no longer idling
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

    // This method is called in the editor whenever the inspector values for this component are changed
    private void OnValidate()
    {
        if (ADD_POS) // If the ADD_POS flag is set to true in the inspector
        {
            ADD_POS = false; // Reset the flag to false
            _path.Add(transform.position); // Add the current position of the enemy to the path
        }
        if (CLEAR_POS) // If the CLEAR_POS flag is set to true in the inspector
        {
            CLEAR_POS = false; // Reset the flag to false
            _path.Clear(); // Clear the path list
            if (_enemyDefultStartPos) // If the enemy is set to start at its default position
            {
                _path.Add(transform.position); // Add the current position of the enemy to the path
            }
        }
    }


    // This method runs at the start of the script
    void Start()
    {
        if (_enemyDefultStartPos) // If the enemy is set to start at its default position
        {
            transform.position = _path[0]; // Set the position of the enemy to the first point in its path
        }
    }

    //update the points of the path
    void Update()
    {
        _MoveToPoint();
    }
}