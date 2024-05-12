using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BulletSmile.Ball.View
{
    public class BallTrajectoryView : MonoBehaviour
    {
        public BallMover ballMover;
        public BallController ballController;
        public SphereCollider ballCollider;
        public LineRenderer lineRenderer;
    
        [Header("Rendering settings")]
        public LayerMask rayCastMask;
        public float rayCastDeltaTime;
        public int maxRayCastCount;
        public Transform finalPositionView;

        [Header("Debug settings")]
        public bool alwaysShowTrajectory;

        private bool _predicting;

        private void Start()
        {
            ballController.onAimingStatusChanged += (position, aiming) =>
            {
                _predicting = aiming;
                var show = aiming || alwaysShowTrajectory;
                lineRenderer.enabled = show;
                finalPositionView.gameObject.SetActive(show);
            };
        }

        private void FixedUpdate()
        {
            if (!_predicting)
            {
                return;
            }

            var origin = ballMover.transform.position;
            var velocity = ballMover.GetThrowVelocity(ballController.currentDirection) + 
                           ballMover.ballRigidbody.velocity;

            var positions = new List<Vector3>{ origin };
            for (var i = 0; i < maxRayCastCount; i++)
            {
                var nextVelocity = velocity + Physics.gravity * rayCastDeltaTime;
                var averageVelocity = (velocity + nextVelocity) * 0.5f;
                var positionDelta = averageVelocity * rayCastDeltaTime;
                var ray = new Ray(origin, averageVelocity);
            
                if (Physics.SphereCast(ray, ballCollider.radius, out var hitInfo, 
                        positionDelta.magnitude, rayCastMask))
                {
                    positions.Add(hitInfo.point + hitInfo.normal * ballCollider.radius);
                    break;
                } 
                // SphereCast does not register collisions when you try to cast it
                // to the surface in distance ~equal to the sphere radius,
                // so that is some sort of solution for this problem
                if (Physics.Raycast(ray, out hitInfo, positionDelta.magnitude, rayCastMask))
                {
                    positions.Add(hitInfo.point + hitInfo.normal * ballCollider.radius);
                    break;
                }
            
                origin += positionDelta;
                velocity = nextVelocity;
                positions.Add(origin);
            }

            lineRenderer.positionCount = positions.Count;
            lineRenderer.SetPositions(positions.ToArray());
            finalPositionView.position = positions.Last();
        }
    }
}
