using Mediapipe.Tasks.Components.Containers;
using Mediapipe.Unity.Sample.FaceLandmarkDetection;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FaceLandmarkerRunner))]
public class EyeDataCollector : MonoBehaviour
{
    private FaceLandmarkerRunner _faceLandmarkerRunner;

    private static int[] _eyeIndicesRight = new int[6] { 133, 33, 158, 153, 160, 144 };
    private static int[] _eyeIndicesLeft = new int[6] { 362, 263, 385, 380, 387, 373 };

    private void Awake()
    {
        _faceLandmarkerRunner = GetComponent<FaceLandmarkerRunner>();
    }

    public EyeData GetEyeData()
    {
        List<NormalizedLandmarks> faces = _faceLandmarkerRunner.GetResult().faceLandmarks;

        if (faces.Count == 0) { return default; }

        List<NormalizedLandmark> landmarks = faces[0].landmarks;

        float eyeLeft = CalculateEyeAspectRatio(landmarks, _eyeIndicesLeft);
        float eyeRight = CalculateEyeAspectRatio(landmarks, _eyeIndicesRight);

        return new EyeData(eyeLeft, eyeRight);
    }

    private static float CalculateEyeAspectRatio(List<NormalizedLandmark> landmarks, int[] lookupTable)
    {
        if (landmarks.Count < 468) return float.NaN;

        Vector2 insideCorner = NormalizedLandmarkToVector2(landmarks[lookupTable[0]]);
        Vector2 outsideCorner = NormalizedLandmarkToVector2(landmarks[lookupTable[1]]);
        Vector2 insideUp = NormalizedLandmarkToVector2(landmarks[lookupTable[2]]);
        Vector2 insideDown = NormalizedLandmarkToVector2(landmarks[lookupTable[3]]);
        Vector2 outsideUp = NormalizedLandmarkToVector2(landmarks[lookupTable[4]]);
        Vector2 outsideDown = NormalizedLandmarkToVector2(landmarks[lookupTable[5]]);

        float innerHeight = (insideUp - insideDown).magnitude;
        float outerHeight = (outsideUp - outsideDown).magnitude;
        float length = (insideCorner - outsideCorner).magnitude;

        return (innerHeight + outerHeight) / (2 * length);
    }

    private static Vector2 NormalizedLandmarkToVector2(NormalizedLandmark inLandmark)
    {
        return new Vector2(inLandmark.x, inLandmark.y);
    }
}
