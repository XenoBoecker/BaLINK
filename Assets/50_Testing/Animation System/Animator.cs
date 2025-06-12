using System;
using UnityEngine;

namespace Custom
{
    [AddComponentMenu("Custom/Animator")]
    public class Animator : MonoBehaviour
    {
        [SerializeField] private int _startingFrame;
        [SerializeField] private bool _loopAnimation;
        [SerializeField] private Animation _animation;
        [SerializeField] private bool _animateOnlyIfInView = false;

        private int _currentFrame;
        private MeshRenderer _renderer;
        [SerializeField] LayerMask _visibilityMask;

        private void Awake()
        {
            Physics.queriesHitBackfaces = false;
            _renderer = GetComponent<MeshRenderer>();

            SetAnimationStateToFrame(_startingFrame);
        }

        public Animation GetAnimation()
        {
            return _animation;
        }

        public bool InitializeAnimation()
        {
            _animation = new Animation(4);

            return true;
        }

        public void NextFrame()
        {
            if (!IsVisible(_renderer) && _animateOnlyIfInView) { return; }

            _currentFrame++;
            if (_currentFrame >= _animation.AnimationLength)
            {
                if (!_loopAnimation) { return; }
                _currentFrame = 0;
            }
            SetAnimationStateToFrame(_currentFrame);
        }

        public void SetAnimationStateToFrame(int currentFrame)
        {
            _animation.SetAnimationToIndex(gameObject, currentFrame);
        }

        private bool IsVisible(MeshRenderer renderer)
        {
            if (!renderer.isVisible) { return false; }

            Transform cameraTransform = Camera.main.transform;

            Vector3 direction = cameraTransform.position - transform.position;
            float distance = direction.magnitude;
            direction.Normalize();

            bool hit = Physics.Raycast(new Ray(transform.position, direction), distance, _visibilityMask);
            return !hit;
        }
    }
}

