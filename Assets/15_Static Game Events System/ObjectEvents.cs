using System;
using UnityEngine;

namespace GameEvents
{
    public static class ObjectEvents
    {
        public static event Action OnGraphMadeBlinkChange;
        public static void GraphMadeBlinkChange()
        {
            OnGraphMadeBlinkChange?.Invoke();
        }

        public static event Action<AudioSystemClip, Vector3> OnPlayAudio;
        public static void PlayAudio(AudioSystemClip clip, Vector3 position)
        {
            OnPlayAudio?.Invoke(clip, position);
        }

        public static event Action OnWrongButtonPressed;
        public static void WrongButtonPressed()
        {
            OnWrongButtonPressed?.Invoke();
        }
    }
}
