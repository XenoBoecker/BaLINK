using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class TrainCarGenerator : MonoBehaviour
{
    public float MinimumSegementLength;
    public TrainCarSegement[] DefinedSegments;
    [HideInInspector] public int[] SegmentLayout;
    public float TrainCarLength = 3;

    [SerializeField] GameObject _carEndSegment;

    public int RequiredSegmentCount { get { return Mathf.CeilToInt(TrainCarLength / MinimumSegementLength); } }
    
    public void RegenerateCar()
    {
        int difference = (RequiredSegmentCount - 1) - SegmentLayout.Length;
        if (difference != 0)
        {
            List<int> tempSegments = new List<int>(SegmentLayout);
            for (int i = 0; i < Mathf.Abs(difference); i++)
            {
                if (difference > 0)
                {
                    tempSegments.Add(0);
                } 
                else
                {
                    tempSegments.RemoveAt(tempSegments.Count - 1);
                }
            }
            SegmentLayout = tempSegments.ToArray();
        }

        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < RequiredSegmentCount + 1; i++)
        {
            Vector3 position = transform.position + Vector3.forward * i * MinimumSegementLength;

            if (i == 0 || i >= RequiredSegmentCount)
            {
                GameObject endSegment = Instantiate(_carEndSegment, position, Quaternion.identity, transform);
                if (i >= RequiredSegmentCount)
                {
                    endSegment.transform.localScale = Vector3.Scale(endSegment.transform.localScale, new Vector3(1, 1, -1));
                }
                continue;
            } 

            if (!SegmentLayout[i - 1].Equals(int.MinValue))
            {
                Instantiate(DefinedSegments[SegmentLayout[i - 1]].SegmentPrefab, position, Quaternion.identity, transform);
                continue;
            }
        }
    }
}
