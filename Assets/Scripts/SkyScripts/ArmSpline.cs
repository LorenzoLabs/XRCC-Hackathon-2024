using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.Serialization;
using UnityEngine.Splines;

namespace LlamAcademy.ChickenDefense.Units.Enemies.Snake.Behaviors
{
    [RequireComponent(typeof(SplineContainer))]
    public class ArmSplineAnimator : MonoBehaviour
    {
        public enum HandleOrientation
        {
            Element = 0,
            Global = 1,
            Local = 2,
            Parent = 3
        }
        
        [SerializeField] private float _speed = 1f;
        [SerializeField] private float _maxWidthOffset = 1f;
        [SerializeField] private float _maxDistance = 2f; // Maximum distance the hand can stretch out
        [SerializeField] private float _moveSpeed = 1f; // Speed at which the hand moves
        [SerializeField] private float _inputThreshold = 0.5f; // Threshold for thumbstick input
        [SerializeField] private Transform _hand;
        [SerializeField] private Transform _forearm;
        //[SerializeField] private InputActionProperty thumbstickInput;

        private Transform _root;
        private SplineContainer _container;
        private Quaternion _defaultHeadRotation;
        private Quaternion _defaultTailRotation;
        private Vector3 _initialHandPosition;
        private Vector3 _initialForearmPosition;
        private bool _isMoving = false;
        private Vector3 _targetHandPosition;
        private Vector3 _targetForearmPosition;

        private void Awake()
        {
            //_root = GetComponentInParent<Snake>().transform;
            _container = GetComponent<SplineContainer>();
            _defaultHeadRotation = _hand.localRotation;
            _defaultTailRotation = _forearm.localRotation;
            Spline.Changed += HandleSplineChanged;

            _initialHandPosition = _hand.localPosition;
            _initialForearmPosition = _forearm.localPosition;
        }

        private void LateUpdate()
        {
            BezierKnot knot = _container.Spline[_container.Spline.Count - 1];
            Quaternion knotRotation = new(
                knot.Rotation.value.x,
                knot.Rotation.value.y,
                knot.Rotation.value.z,
                knot.Rotation.value.w
            );

            _hand.transform.localRotation = Quaternion.Euler(
                knotRotation.eulerAngles - _defaultHeadRotation.eulerAngles
            );

            if (_isMoving)
            {
                _hand.transform.localPosition = Vector3.MoveTowards(_hand.transform.localPosition, _targetHandPosition, _moveSpeed * Time.deltaTime);
                knot.Position = _hand.localPosition; // Update the spline's last knot position
                _container.Spline[_container.Spline.Count - 1] = knot;

                if (Vector3.Distance(_hand.transform.localPosition, _targetHandPosition) < 0.01f)
                {
                    _isMoving = false;
                }
            }
            else
            {
                _hand.transform.localPosition = new Vector3(knot.Position.x, knot.Position.y, knot.Position.z);
            }
        }

        private void HandleSplineChanged(Spline spline, int index, SplineModification change)
        {
            if (spline != _container.Spline || index != spline.Count - 1)
            {
                return;
            }

            BezierKnot knot = spline[index];
            Quaternion knotRotation = new(
                knot.Rotation.value.x,
                knot.Rotation.value.y,
                knot.Rotation.value.z,
                knot.Rotation.value.w
            );

            _hand.transform.localRotation = Quaternion.Euler(
                knotRotation.eulerAngles - _defaultHeadRotation.eulerAngles
            );

            if (!_isMoving)
            {
                _hand.transform.localPosition = new Vector3(knot.Position.x, knot.Position.y, knot.Position.z);
            }
        }

        private void Update()
        {
            bool handTriggerPressed = OVRInput.Get(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch);
            
            Vector2 thumbstickInput = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch);

            if (thumbstickInput.y > _inputThreshold)
            {
                float distance = Mathf.Clamp((thumbstickInput.y - _inputThreshold) / (1 - _inputThreshold) * _maxDistance, 0, _maxDistance);
                _targetHandPosition = _initialHandPosition + _hand.forward * distance;
                _isMoving = true;
            }
            else if (thumbstickInput.y < -_inputThreshold)
            {
                float distance = Mathf.Clamp((-thumbstickInput.y - _inputThreshold) / (1 - _inputThreshold) * _maxDistance, 0, _maxDistance);
                _targetHandPosition = _initialHandPosition - _hand.forward * distance;
                _isMoving = true;
            }
            else
            {
                _targetHandPosition = _initialHandPosition;
                _isMoving = true;
            }

            Spline spline = _container.Spline;
            int knotCount = spline.Count;
            BezierKnot knot;

            for (int i = knotCount - 1; i > 0; i--)
            {
                float distanceDamping = (float)i / knotCount;
                knot = spline[i];
                knot.Position.x = Mathf.Sin(i * (Mathf.PI / 2) + Time.time * _speed) * distanceDamping * _maxWidthOffset;
                spline[i] = knot;
            }

            knot = spline[0];
            Quaternion knotRotation = new(
                knot.Rotation.value.x,
                knot.Rotation.value.y,
                knot.Rotation.value.z,
                knot.Rotation.value.w
            );

            _forearm.transform.localRotation = Quaternion.Euler(
                knotRotation.eulerAngles + _defaultTailRotation.eulerAngles
            );

            _forearm.transform.localPosition =
                new Vector3(knot.Position.x, knot.Position.y, knot.Position.z);
        }
    }
}