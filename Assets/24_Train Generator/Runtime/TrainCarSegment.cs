using UnityEngine;

public class TrainCarSegment : MonoBehaviour
{
    [SerializeField] private int _index;
    [SerializeField] private GameObject _flipableObjects;
    
    public int Index => _index;
    
    private int _flipCounter = 0;

    public void SetIndex(int index)
    {
        _index = index;
    }

    public void FlipSegmentContents()
    {
        if (_flipableObjects == null) { return; }

        if (_flipCounter % 2 == 0)
        {
            _flipableObjects.transform.localScale = Vector3.Scale(_flipableObjects.transform.localScale, new Vector3(-1, 1, 1));
        } 
        else
        {
            _flipableObjects.transform.localScale = Vector3.Scale(_flipableObjects.transform.localScale, new Vector3(1, 1, -1));
        }
        _flipCounter++;
    }
}
