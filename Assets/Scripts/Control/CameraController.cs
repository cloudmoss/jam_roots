using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float acceleration = 5f;
    public float decelerationRate = 0.5f;
    public float maxSpeed = 20;
    public Vector2 zoomBounds = new Vector2(5, 25);
    public float zoomSpeed = 5f;
    public float zoomSmoothing = 20f;

    private Camera _camera;
    private Vector2 _velocity = Vector2.zero;
    private float _zoom;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        _zoom = _camera.orthographicSize;
    }

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

        _velocity = Vector2.ClampMagnitude(_velocity, maxSpeed) * (_zoom / zoomBounds.x);

        transform.position += (Vector3)_velocity * Time.unscaledDeltaTime;

        _velocity *= new Vector2(inputX ? 1 : decelerationRate, inputY ? 1 : decelerationRate);
        
        _zoom -= Input.mouseScrollDelta.y * zoomSpeed;
        _zoom = Mathf.Clamp(_zoom, zoomBounds.x, zoomBounds.y);

        _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, _zoom, Time.unscaledDeltaTime * zoomSmoothing);
    }
}
