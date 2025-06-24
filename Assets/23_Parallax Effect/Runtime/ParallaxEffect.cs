using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Custom/Paralax Effect")]
public class ParallaxEffect : MonoBehaviour
{
    [SerializeField] ParallaxElement[] _paralaxElements;
    [SerializeField] ParallaxGroup[] _paralaxGroups;

    [SerializeField] private float _speed;
    [SerializeField] private float _length;

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        _paralaxGroups = new ParallaxGroup[_paralaxElements.Length];

        for (int i = 0; i < _paralaxElements.Length; i++)
        {
            GameObject holder = new GameObject(_paralaxElements[i].Object.name + " Holder");
            holder.transform.SetParent(transform);

            int numberOfObjects = Mathf.RoundToInt(_length/_paralaxElements[i].ObjectLength);
            List<GameObject> objects = new List<GameObject>();
            for (int j = 0; j < numberOfObjects; j++)
            {
                objects.Add(Instantiate(_paralaxElements[i].Object, holder.transform));
                objects[j].transform.localPosition = new Vector3(_paralaxElements[i].ObjectLength * j, 0, _paralaxElements[i].Depth);
            }

            _paralaxGroups[i] = new ParallaxGroup(objects.ToArray(), _paralaxElements[i]);
        }
    }

    private void Update()
    {
        foreach (ParallaxGroup group in _paralaxGroups)
        {
            group.MoveObjects(_speed * Time.deltaTime);
        }
    }

    public void Preview()
    {
        foreach (ParallaxGroup group in _paralaxGroups)
        {
            group.MoveObjects(_speed * Time.deltaTime * 0.3f);
        }
    }

    public void Cleanup()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }
}
