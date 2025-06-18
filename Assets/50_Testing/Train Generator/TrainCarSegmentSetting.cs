using UnityEngine;

[System.Serializable]
public class TrainCarSegmentSetting
{
    private int _segmentIndex;
    private bool _isFlipped;
    private bool _isLocked;

    public int SegmentIndex => _segmentIndex;
    public bool IsFlipped => _isFlipped;
    public bool IsLocked => _isLocked;

    public void SetIndex(int index) { _segmentIndex = index; }
    public void SetIsFlipped(bool value) { _isFlipped = value; }
    public void SetIsLocked(bool value) { _isLocked = value; }

    public TrainCarSegmentSetting(int index)
    {
        _segmentIndex = index;
    }
}