using System;

namespace GameEvents
{
    public static class ObjectEvents
    {
        public static event Action OnGraphMadeBlinkChange;
        public static void GraphMadeBlinkChange()
        {
            OnGraphMadeBlinkChange?.Invoke();
        }

        public static event Action OnPlayAudio;
        public static void PlayAudio()
        {
            OnPlayAudio?.Invoke();
        }
    }
}
