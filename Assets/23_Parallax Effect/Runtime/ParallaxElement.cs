using UnityEngine;

[System.Serializable]
public class ParallaxElement
{
    [SerializeField] private GameObject _object;
    [SerializeField] private float _objectScale;
    [SerializeField] private float _depth;
    [SerializeField] private float _speedModifier;

    public GameObject Object => _object;
    public float SpeedModifier => _speedModifier;
    public float ObjectLength => _objectScale;
    public float Depth => _depth;
}
