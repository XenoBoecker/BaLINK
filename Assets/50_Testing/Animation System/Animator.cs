using System;
using UnityEngine;

namespace Custom
{
    [AddComponentMenu("Custom/Animator")]
    public class Animator : MonoBehaviour
    {
        [SerializeField] int _currentFrame;
        [SerializeField] bool _loopAnimation;
        [SerializeField] private Animation _animation;

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
    }
}

