namespace DevelopersHub.RaidingThrones
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Camera _camera = null;
        [SerializeField] private float _moveSpeed = 50;
        [SerializeField] private float _moveSmooth = 5;

        [SerializeField] private float _zoomSpeed = 5f;
        [SerializeField] private float _zoomSmooth = 5;

  

        private Controls _inputs = null;

        private bool _zooming = false;
        private bool _moving = false;
        private Vector3 _center = Vector3.zero;
        private float _right = 10;
        private float _left = 10;
        private float _up = 10;
        private float _down = 10;
        private float _angle = 45;
        private float _zoom = 5;
        private float _zoomMax = 10;
        private float _zoomMin = 1;



        private Transform _root = null;
        private Transform _pivot = null;
        private Transform _target = null;
        private void Awake()
        {
            _inputs = new Controls();
            _root = new GameObject("CameraHelper").transform;
            _pivot = new GameObject("CameraPivot").transform;
            _target = new GameObject("CameraTarget").transform;
            _camera.orthographic = true;
            _camera.nearClipPlane = 0;
        }

        private void Start()
          {
            Initialize(center:Vector3.zero, right:10, left:10, up:10, down:10, angle:45, zoom:5, zoomMin:3, zoomMax:10);
          }

        public void Initialize(Vector3 center, float right, float left, float up, float down, float angle, float zoom, float zoomMin, float zoomMax)
        {
            _center = center;
            _right = right;
            _left = left;
            _up = up;
            _down = down;
            _angle = angle;
            _zoom = zoom;
            _zoomMin = zoomMin;
            _zoomMax = zoomMax;
            
            _camera.orthographicSize = _zoom;
            _moving = false;
            _pivot.SetParent(_root);
            _target.SetParent(_pivot);

            _root.position = center;
            _root.localEulerAngles = Vector3.zero;

            _pivot.localPosition = Vector3.zero;
            _pivot.localEulerAngles = new Vector3(_angle, 0, 0);

            _target.localPosition = new Vector3(0, 0, -10);
            _target.localEulerAngles = Vector3.zero;
        }


        private void OnEnable()
        {
            _inputs.Enable();
            _inputs.Main.Move.started += _ => MoveStarted();
            _inputs.Main.Move.canceled += _ => MoveCanceled();
        }

        private void OnDisable()
        {
            _inputs.Disable();
        }

        private void MoveStarted()
        {
            _moving = true;

        }

        private void MoveCanceled()
        {
            _moving = false;
        }

        private void Update()
        {
            if (Input.touchSupported == false)
            {
                float mouseScroll = _inputs.Main.MouseScroll.ReadValue<float>();
                if(mouseScroll > 0)
                {
                    _zoom -= 3f * Time.deltaTime;
                }
                else if (mouseScroll < 0)
                {
                    _zoom += 3f * Time.deltaTime;
                }
            }


            if (_moving)
            {
                Vector2 move = _inputs.Main.MoveDelta.ReadValue<Vector2>();
                if (move != Vector2.zero)
                {
                    move.x /= Screen.width;
                    move.y /= Screen.height;
                    _root.position -= _root.right.normalized * move.x * _moveSpeed;
                    _root.position -= _root.forward.normalized * move.y * _moveSpeed;
                }
            }
            if(_camera.orthographicSize != _zoom)
            {
                _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, _zoom, _moveSmooth * Time.deltaTime);
            }

            if (_camera.transform.position != _target.position)
            {
                _camera.transform.position = Vector3.Lerp(_camera.transform.position, _target.position, Time.deltaTime * _moveSmooth);
            }
            if (_camera.transform.rotation != _target.rotation)
            {
                _camera.transform.rotation = _target.rotation;
            }
        }
    }
}