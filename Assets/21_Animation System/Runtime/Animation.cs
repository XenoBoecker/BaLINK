using System;
using System.Collections.Generic;
using UnityEngine;

namespace Custom
{
    [System.Serializable]
    public class Animation
    {
        [SerializeField] private AnimationFrame[] _animationFrames;
        public int AnimationLength { get { return _animationFrames.Length; } }

        public Animation(int initialFrameCount)
        {
            _animationFrames = new AnimationFrame[initialFrameCount];
            for (int i = 0; i < _animationFrames.Length; i++)
            {
                _animationFrames[i] = new AnimationFrame();
            }
        }

        public AnimationFrame GetAnimationFrameFromIndex(int index)
        {
            if (index < 0 || index > AnimationLength - 1)
            {
                throw new Exception("Tried to get a frame that dosen't exist");
            }

            return _animationFrames[index];
        }

        internal void SetAnimationToIndex(GameObject gameObject, int index)
        {
            _animationFrames[index].ApplyAnimationFrame(gameObject);
        }

        public void SetNumberOfFrames(int numberOfRemaingFrames)
        {
            List<AnimationFrame> framesTemp = new List<AnimationFrame>(_animationFrames);
            int differenceInFrameCount = numberOfRemaingFrames - _animationFrames.Length;
            
            if (differenceInFrameCount < 0)
            {
                for (int i = 0; i < Mathf.Abs(differenceInFrameCount); i++)
                {
                    framesTemp.RemoveAt(framesTemp.Count - 1);
                }
            } 
            else if (differenceInFrameCount > 0)
            {
                for (int i = 0; i < Mathf.Abs(differenceInFrameCount); i++)
                {
                    framesTemp.Add(new AnimationFrame());
                }
            }

            _animationFrames = framesTemp.ToArray();
        }
    }
}
