using System;
using UnityEngine;

namespace GameEvents
{
    public static class InputEvent
    {
        public static event Action onPlayerBlinked;
        public static void PlayerBlinked()
        {
            onPlayerBlinked?.Invoke();
        }
    }
}
