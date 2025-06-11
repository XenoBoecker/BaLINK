using System;
using UnityEngine;

namespace GameEvents
{
    public static class InputEvents
    {
        public static event Action onPlayerBlinked;
        public static void PlayerBlinked()
        {
            onPlayerBlinked?.Invoke();
        }
    }
}
