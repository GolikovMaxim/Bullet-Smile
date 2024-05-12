using DG.Tweening;
using UnityEngine;

namespace BulletSmile.Ball
{
    public class AimSlowMotion : MonoBehaviour
    {
        public BallController ballController;
        public float slowMotionTimeScale, slowMotionTransitionDuration;

        private float _startFixedDeltaTime;
    
        private void Start()
        {
            _startFixedDeltaTime = Time.fixedDeltaTime;

            ballController.onAimingStatusChanged += (position, aiming) =>
            {
                DOTween.To(() => Time.timeScale, value =>
                    {
                        Time.timeScale = value;
                        Time.fixedDeltaTime = _startFixedDeltaTime * value;
                    }, 
                    aiming ? slowMotionTimeScale : 1, slowMotionTransitionDuration);
            };
        }
    }
}
