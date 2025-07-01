using UnityEngine;

public class ParallaxGroup
{
    private GameObject[] _objectsInGroup;
    public ParallaxElement _element;
    public float _totalLength => _objectsInGroup.Length * _element.ObjectLength;

    public GameObject[] objectsInGroup => _objectsInGroup;

    public ParallaxGroup(GameObject[] objectsInGroup, ParallaxElement element)
    {
        _objectsInGroup = objectsInGroup;
        _element = element;
    }

    public void MoveObjects(float speed)
    {
        for (int i = 0; i < _objectsInGroup.Length; i++)
        {
            _objectsInGroup[i].transform.localPosition += new Vector3(1, 0, 0) * speed * _element.SpeedModifier;

            if (_objectsInGroup[i].transform.localPosition.x >= _totalLength)
            {
                _objectsInGroup[i].transform.localPosition = Vector3.Scale(_objectsInGroup[i].transform.localPosition, new Vector3(0, 1, 1));
            }

            if (_objectsInGroup[i].transform.localPosition.x < 0)
            {
                _objectsInGroup[i].transform.localPosition = new Vector3(_totalLength, _objectsInGroup[i].transform.localPosition.y, _objectsInGroup[i].transform.localPosition.z);
            }
        }
    }
}
