using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Custom
{
    [CustomEditor(typeof(Animator))]
    public class AnimatorEditor : Editor
    {
        AnimationWindow _window;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            Animator animator = target as Animator;
            GameObject obj = animator.gameObject;

            if (GUILayout.Button("Edit Animation"))
            {
                if (_window == null)
                {
                    _window = CreateInstance<AnimationWindow>();
                }

                _window.CreateWindow(animator);
            }
        }
    }
}

