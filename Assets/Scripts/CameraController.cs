using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : SimpleSingleton<CameraController>
{
    [SerializeField]
    Camera _cam;

    [SerializeField]
    Transform _focusTarget;

    [SerializeField]
    float _baseXAxisEuler = 30;

    [SerializeField]
    float _maxXAxisEuler = 80;

    [SerializeField]
    float _minXAxisEuler = 0;

    [SerializeField]
    float _distanceFromTarget;

    [SerializeField]
    float _cameraMoveSpeed = 60f;

    [SerializeField]
    float _rotateSpeed = 60f;

    [SerializeField]
    float _scrollSpeed = 10f;

    Vector3 _targetPosition;
    float _currentHorizontalAngle = 0;
    float _targetHorizontalAngle = 0;
    float _currentXAxisEuler;

    Vector3 _currentMousePos;
    Vector3 _lastMousePos;
    private void Update()
    {
        if (_cam == null || _focusTarget == null) return;
        RotateCameraOnInput();
        ZoomCameraOnInput();
        UpdatePosition();
        SelectInput();
    }

    public void FocusTarget(Transform focusTarget)
    {
        _focusTarget = focusTarget;
        _targetHorizontalAngle = 0;
        _targetPosition = GetPosition(_targetHorizontalAngle);
        _currentXAxisEuler = _baseXAxisEuler;
    }

    public Vector3 GetPosition(float horizontalAngle)
    {
        Quaternion rotation = Quaternion.Euler(_currentXAxisEuler, 0, 0);
        Vector3 dir = rotation * Vector3.back;
        Vector3 positionAtZero = _focusTarget.position + dir * _distanceFromTarget;
        Vector3 targetDir = Quaternion.AngleAxis(horizontalAngle, Vector3.up) * Vector3.back;
        Vector3 targetPosition = _focusTarget.position;
        targetPosition.y = positionAtZero.y;
        float distanceToCenterLine = (positionAtZero - targetPosition).magnitude;
        targetPosition += distanceToCenterLine * targetDir;
        _currentHorizontalAngle = horizontalAngle;
        return targetPosition;
    }

    void UpdatePosition()
    {
        if (Vector3.Distance(_targetPosition, _cam.transform.position) < 0.5f) return;
        Vector3 position = Vector3.SlerpUnclamped(_cam.transform.position, _targetPosition, _cameraMoveSpeed * Time.deltaTime);
        _cam.transform.position = position;
        _cam.transform.forward = (_focusTarget.transform.position - position).normalized;
    }

    void RotateCameraOnInput()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            _currentMousePos = _lastMousePos = Input.mousePosition;
        }
        if (Input.GetButton("Fire2"))
        {
            _currentMousePos = Input.mousePosition;
            float x = _currentMousePos.x - _lastMousePos.x;
            float y = _currentMousePos.y - _lastMousePos.y;
            _lastMousePos = Input.mousePosition;
            if (x != 0) _targetHorizontalAngle += x * _rotateSpeed * Time.deltaTime;
            if (y != 0) 
            { 
                _currentXAxisEuler -= y * _rotateSpeed * Time.deltaTime;
                if (_currentXAxisEuler > _maxXAxisEuler) _currentXAxisEuler = _maxXAxisEuler;
                else if (_currentXAxisEuler < _minXAxisEuler) _currentXAxisEuler = _minXAxisEuler;
            }
            _targetPosition = GetPosition(_targetHorizontalAngle);
            _currentHorizontalAngle = _targetHorizontalAngle;
        }

        if (Input.GetButtonUp("Fire2"))
        {
            _currentMousePos = _lastMousePos = Vector3.zero;
        }
    }

    void ZoomCameraOnInput()
    {
        if(Input.mouseScrollDelta != Vector2.zero)
        {
            _distanceFromTarget -= Input.mouseScrollDelta.y * Time.deltaTime * _scrollSpeed;
            _targetPosition = GetPosition(_targetHorizontalAngle);
        }
    }

    void SelectInput()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            RaycastHit hit;
            Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit, _distanceFromTarget * 2, 1 << 6))
            {
                Block block = hit.collider.GetComponent<Block>();
                if (block != null)
                {
                    OnBlockSelectMessage msg = new OnBlockSelectMessage();
                    msg.SelectedBlock = block;
                    GlobalPubSub.PublishEvent(msg);
                }
            }
        }
    }
}
