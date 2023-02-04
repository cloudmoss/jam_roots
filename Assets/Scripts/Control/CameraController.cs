using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float speed = 20;
    public Vector2 zoomBounds = new Vector2(5, 25);
    public float zoomSpeed = 5f;
    public float zoomSmoothing = 20f;
    public float movementSmoothing = 50f;

    private Camera _camera;
    private Vector2 _velocity = Vector2.zero;
    private Vector2 _targetPos;
    private float _zoom;

    private void Awake()
    {
        _targetPos = transform.position;
        _camera = GetComponent<Camera>();
        _zoom = _camera.orthographicSize;
    }

    void Update()
    {
        _velocity = Vector2.zero;

        if (Input.GetKey(KeyCode.W))
        {
            if (_velocity.y < 0)
                _velocity.y = 0;

            _velocity += Vector2.up;
        }

        if (Input.GetKey(KeyCode.S))
        {
            if (_velocity.y > 0)
                _velocity.y = 0;

            _velocity -= Vector2.up;
        }

        if (Input.GetKey(KeyCode.D))
        {
            if (_velocity.x < 0)
                _velocity.x = 0;

            _velocity += Vector2.right;
        }

        if (Input.GetKey(KeyCode.A))
        {
            if (_velocity.x > 0)
                _velocity.x = 0;

            _velocity -= Vector2.right;
        }

        _velocity = Vector2.ClampMagnitude(_velocity, speed * (_zoom / zoomBounds.x)) * speed;

        _targetPos += _velocity * Time.unscaledDeltaTime;

        _zoom -= Input.mouseScrollDelta.y * zoomSpeed;
        _zoom = Mathf.Clamp(_zoom, zoomBounds.x, zoomBounds.y);

        _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, _zoom, Time.unscaledDeltaTime * zoomSmoothing);

        transform.position = Vector3.Lerp(transform.position, _targetPos, Time.unscaledDeltaTime * movementSmoothing);
        transform.position = new Vector3(transform.position.x, transform.position.y, -200);
    }
}
