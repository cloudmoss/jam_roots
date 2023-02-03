using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float acceleration = 5f;
    public float decelerationRate = 0.5f;
    public float maxSpeed = 20;

    private Vector2 _velocity = Vector2.zero;

    void Update()
    {
        var inputX = false;
        var inputY = false;

        if (Input.GetKey(KeyCode.W))
        {
            if (_velocity.y < 0)
                _velocity.y = 0;

            inputY = true;
            _velocity += Vector2.up * acceleration * Time.unscaledDeltaTime;
        }

        if (Input.GetKey(KeyCode.S))
        {
            if (_velocity.y > 0)
                _velocity.y = 0;

            inputY = true;
            _velocity -= Vector2.up * acceleration * Time.unscaledDeltaTime;
        }

        if (Input.GetKey(KeyCode.D))
        {
            if (_velocity.x < 0)
                _velocity.x = 0;

            inputX = true;
            _velocity += Vector2.right * acceleration * Time.unscaledDeltaTime;
        }

        if (Input.GetKey(KeyCode.A))
        {
            if (_velocity.x > 0)
                _velocity.x = 0;

            inputX = true;
            _velocity -= Vector2.right * acceleration * Time.unscaledDeltaTime;
        }

        _velocity = Vector2.ClampMagnitude(_velocity, maxSpeed);

        transform.position += (Vector3)_velocity * Time.unscaledDeltaTime;

        _velocity *= new Vector2(inputX ? 1 : decelerationRate, inputY ? 1 : decelerationRate);
    }
}
