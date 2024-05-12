using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace BulletSmile.Ball.View
{
    public class BallAimView : MonoBehaviour
    {
        public BallController ballController;
        public RectTransform aimRectTransform, parentRectTransform;
        public List<Graphic> notches;
        public Color minColor, maxColor;

        private bool _aiming;
        
        private void Start()
        {
            var distance = ballController.maxDelta / notches.Count;
            for (var i = 0; i < notches.Count; i++)
            {
                notches[i].rectTransform.anchoredPosition =
                    new Vector2(notches[i].rectTransform.anchoredPosition.x, -distance * i);
            }

            ballController.onAimingStatusChanged += (position, aiming) =>
            {
                aimRectTransform.gameObject.SetActive(aiming);
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTransform, position, 
                        null, out var aimPosition))
                {
                    aimRectTransform.anchoredPosition = aimPosition;
                }
            };
            
            aimRectTransform.gameObject.SetActive(false);
        }

        private void Update()
        {
            var length = ballController.currentDirection.magnitude * notches.Count;
            for (var i = 0; i < notches.Count; i++)
            {
                notches[i].color = Color.Lerp(minColor, maxColor, length - i);
            }

            if (length > Mathf.Epsilon)
            {
                aimRectTransform.up = ballController.currentDirection.normalized;
            }
        }
    }
}
