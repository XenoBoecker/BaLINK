using UnityEngine;

[System.Serializable]
public class TrainCarSegmentSetup
{
    [SerializeField] private int _length = 1;
    public int Length => _length;

    [SerializeField] private GameObject _segmentPrefab;
    public GameObject SegmentPrefab => _segmentPrefab;
}
