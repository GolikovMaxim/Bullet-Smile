using UnityEngine;
using UnityEngine.InputSystem;

namespace BulletSmile.Ball
{
    public class BallController : MonoBehaviour
    {
        public delegate void AimingStatusCallback(Vector3 position, bool aiming);
    
        public BallMover ballMover;
        public Camera viewCamera;
        public float maxDelta, referenceResolution;

        private BallControls _ballControls;
        private InputAction _moveAction, _pressAction;

        private Vector2 _startPosition;

        public Vector3 currentDirection => viewCamera.transform.TransformDirection(
            (_startPosition - _moveAction.ReadValue<Vector2>()) / maxDelta);

        public event AimingStatusCallback onAimingStatusChanged;
    
        private void Awake()
        {
            var minScreenSide = Mathf.Min(Screen.width, Screen.height);
            var scaledMaxDelta = minScreenSide * maxDelta / referenceResolution;
            maxDelta = scaledMaxDelta;
        
            _ballControls = new BallControls();
        
            _moveAction = _ballControls.FindAction("Move");
            _pressAction = _ballControls.FindAction("Press");

            _pressAction.started += context =>
            {
                _startPosition = _moveAction.ReadValue<Vector2>();
                onAimingStatusChanged?.Invoke(_startPosition, true);
            };
            _pressAction.canceled += context =>
            {
                onAimingStatusChanged?.Invoke(_moveAction.ReadValue<Vector2>(), false);
                ballMover.Throw(currentDirection);
            };
        
            SetActive(true);
        }

        public void SetActive(bool active)
        {
            if (active)
            {
                _ballControls.Enable();
            }
            else
            {
                _ballControls.Disable();
            }
        }
    }
}
