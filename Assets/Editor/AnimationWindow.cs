using System;
using UnityEditor;
using UnityEngine;

namespace Custom
{
    public class AnimationWindow : EditorWindow
    {
        private AnimationWindow _window;
        private Animator _animator;
        private int _currentFrame = 0;

        private Vector3 _positionPrevious = Vector3.negativeInfinity;
        private Vector3 _rotationPrevious = Vector3.negativeInfinity;
        private Vector3 _scalePrevious = Vector3.negativeInfinity;

        private const float SLIDER_PADDING_PERCENTAGE = 0.05f;
        private const float SLIDER_HANDLE_RECT_SIZE = 15;

        private bool isDraggingSlider = false;

        public void CreateWindow(Animator animator)
        {
            _animator = animator;

            //get a window and set the title of the window
            _window = GetWindow<AnimationWindow>();
            _window.titleContent = new GUIContent("Custom Animator");
        }

        private void OnGUI()
        {
            Animation animation = _animator.GetAnimation();
            GameObject obj = _animator.gameObject;

            if (animation == null || animation.AnimationLength == 0)
            {
                if (GUILayout.Button("Create Animation"))
                {
                    if (!_animator.InitializeAnimation()) { return; }
                } 
                else
                {
                    return;
                }
            }

            Rect frameSelectionRect = GUILayoutUtility.GetRect(position.width, 50);

            Handles.color = new Color(0.125f, 0.125f, 0.125f, 1f);
            Handles.DrawAAPolyLine
                (2, 
                frameSelectionRect.position + new Vector2(GetXPosFromIndex(animation, ref frameSelectionRect, 0), frameSelectionRect.height * 0.5f), 
                frameSelectionRect.position + new Vector2(GetXPosFromIndex(animation, ref frameSelectionRect, animation.AnimationLength - 1), frameSelectionRect.height * 0.5f)
                );

            for (int i = 0; i < animation.AnimationLength; i++)
            {
                float positionX = GetXPosFromIndex(animation, ref frameSelectionRect, i);

                Handles.DrawAAPolyLine
                (2,
                frameSelectionRect.position + new Vector2(positionX, frameSelectionRect.height * 0.25f),
                frameSelectionRect.position + new Vector2(positionX, frameSelectionRect.height * 0.75f));

                Handles.color = new Color(0.15f, 0.15f, 0.15f, 1f);
                Handles.DrawSolidDisc((frameSelectionRect.position + new Vector2(positionX, frameSelectionRect.height * 0.5f)), Vector3.forward, 3);
            }

            Handles.color = new Color(0.8f, 0.8f, 0.8f, 1f);
            Handles.DrawSolidDisc((frameSelectionRect.position + new Vector2(GetXPosFromIndex(animation, ref frameSelectionRect, _currentFrame), frameSelectionRect.height * 0.5f)), Vector3.forward, 4);

            Rect handleRect = new Rect(
                new Vector2(
                    GetXPosFromIndex(animation, ref frameSelectionRect, _currentFrame) - SLIDER_HANDLE_RECT_SIZE * 0.5f, 
                    frameSelectionRect.y + frameSelectionRect.height * 0.5f - SLIDER_HANDLE_RECT_SIZE * 0.5f), 
                new Vector2(SLIDER_HANDLE_RECT_SIZE, SLIDER_HANDLE_RECT_SIZE));

            if (Event.current.type == EventType.MouseDown)
            {
                isDraggingSlider = handleRect.Contains(Event.current.mousePosition);
            } 

            if (Event.current.type == EventType.MouseDrag && isDraggingSlider)
            {
                float xSpacing = GetXPosFromIndex(animation, ref frameSelectionRect, 0) - GetXPosFromIndex(animation, ref frameSelectionRect, 1);
                float xCurrent = GetXPosFromIndex(animation, ref frameSelectionRect, _currentFrame);
                float xDifference = Event.current.mousePosition.x - xCurrent;
                if (Mathf.Abs(xDifference) > Mathf.Abs(xSpacing) * 0.5f)
                {
                    int newSelectedFrameIndex = _currentFrame + (xDifference < 0 ? -1 : 1);
                    if (newSelectedFrameIndex >= 0 && newSelectedFrameIndex <= animation.AnimationLength - 1)
                    {
                        _currentFrame = newSelectedFrameIndex;

                        _animator.SetAnimationStateToFrame(_currentFrame);
                    }
                }
            }

            if (Event.current.type == EventType.MouseUp && isDraggingSlider)
            {
                isDraggingSlider = false;
            }

            AnimationFrame frame = animation.GetAnimationFrameFromIndex(_currentFrame);

            GUILayoutUtility.GetRect(position.width, 10);
            
            Rect frameSettingsRect = EditorGUILayout.GetControlRect();
            int newNumberOfFrames = EditorGUI.IntField(frameSettingsRect, "Number Of Frames", animation.AnimationLength);
            if (newNumberOfFrames > 1)
            {
                animation.SetNumberOfFrames(newNumberOfFrames);
                if (_currentFrame > animation.AnimationLength - 1) { _currentFrame = animation.AnimationLength - 1; }
            }

            GUILayoutUtility.GetRect(position.width, 10);

            if (ShouldBeDrawn(frame.Position))
            {
                frame.Position.Value = (Vector3)DrawToggleableField(new GUIContent("Position"), frame.Position.Value, frame.Position.IsActive, out bool activeSate);
                frame.Position.IsActive = activeSate;
            }

            if (ShouldBeDrawn(frame.Rotation))
            {
                frame.Rotation.Value = (Quaternion)DrawToggleableField(new GUIContent("Rotation"), frame.Rotation.Value, frame.Rotation.IsActive, out bool activeSate);
                frame.Rotation.IsActive = activeSate;
            }

            if (ShouldBeDrawn(frame.Scale))
            {
                frame.Scale.Value = (Vector3)DrawToggleableField(new GUIContent("Scale"), frame.Scale.Value, frame.Scale.IsActive, out bool activeSate);
                frame.Scale.IsActive = activeSate;
            }

            if (Event.current.type == EventType.Repaint)
            {
                if(!_positionPrevious.Equals(Vector3.negativeInfinity) && _positionPrevious != obj.transform.position)
                {
                    frame.Position.IsActive = true;
                    frame.Position.Value = obj.transform.position;
                }

                if (!_rotationPrevious.Equals(Vector3.negativeInfinity) && _rotationPrevious != obj.transform.eulerAngles)
                {
                    frame.Rotation.IsActive = true;
                    frame.Rotation.Value = obj.transform.rotation;
                }

                if (!_scalePrevious.Equals(Vector3.negativeInfinity) && _scalePrevious != obj.transform.localScale)
                {
                    frame.Scale.IsActive = true;
                    frame.Scale.Value = obj.transform.localScale;
                }

                _positionPrevious = obj.transform.position;
                _rotationPrevious = obj.transform.eulerAngles;
                _scalePrevious = obj.transform.localScale;
            }

            Rect windowRect = new Rect(position);
            windowRect.position = Vector2.zero;

            if (Event.current.type == EventType.Used && windowRect.Contains(Event.current.mousePosition))
            {
                _animator.SetAnimationStateToFrame(_currentFrame);
                EditorUtility.SetDirty(obj);
            }

            HandleUtility.Repaint();
        }

        private static float GetXPosFromIndex(Animation animation, ref Rect frameSelectionRect, int i)
        {
            float offsetFractionX = i * (1.0f / (animation.AnimationLength - 1));
            float positionX = frameSelectionRect.width * offsetFractionX + (frameSelectionRect.width * SLIDER_PADDING_PERCENTAGE * 2) * (0.5f - offsetFractionX);
            return positionX;
        }

        private object DrawToggleableField(GUIContent guiContent, object value, bool isActive, out bool activeState)
        {
            object returnValue = value;
            activeState = isActive;

            Rect controlRect = EditorGUILayout.GetControlRect();

            GUILayout.BeginHorizontal();
            Rect toggleRect = new Rect(controlRect);
            toggleRect.width = 80;

            activeState = EditorGUI.ToggleLeft(toggleRect, guiContent, isActive);
            controlRect.width -= toggleRect.width;
            controlRect.x += toggleRect.width;

            GUI.enabled = isActive;

            switch (value)
            {
                case Vector3:
                    returnValue = EditorGUI.Vector3Field(controlRect, GUIContent.none, (Vector3)value);
                    break;

                case Quaternion:
                    returnValue = Quaternion.Euler(EditorGUI.Vector3Field(controlRect, GUIContent.none, ((Quaternion)value).eulerAngles));
                    break;

                default:
                    throw new Exception($"Value type not defined for type ({value.GetType()})");
            }

            GUI.enabled = true;
            GUILayout.EndHorizontal();
            return returnValue;
        }

        private bool ShouldBeDrawn<T>(ToggleableField<T> field)
        {
            return field != null;
        }
    }
}
