using UnityEngine;

namespace BulletSmile.Ball
{
    public class BallMover : MonoBehaviour
    {
        public float maxThrowImpulse;
        public Rigidbody ballRigidbody;

        public void Throw(Vector3 direction)
        {
            ballRigidbody.AddForce(GetThrowVelocity(direction), ForceMode.VelocityChange);
        }

        public Vector3 GetThrowVelocity(Vector3 direction)
        {
            return Vector3.ClampMagnitude(direction, 1) * maxThrowImpulse;
        }
    }
}
