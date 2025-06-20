using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class TrainCarGenerator : MonoBehaviour
{
    [HideInInspector] public TrainCarSegmentSetting[] SegmentLayout;
    public float MinimumSegementLength;
    public TrainCarSegmentSetup[] DefinedSegments;
    public float TrainCarLength = 3;

    [SerializeField] GameObject _carEndSegment;

    public int RequiredSegmentCount { get { return Mathf.CeilToInt(TrainCarLength / MinimumSegementLength); } }
    
    public void RegenerateCar()
    {
        if (SegmentLayout == null)
        {
            SegmentLayout = new TrainCarSegmentSetting[0];
        }

        int difference = (RequiredSegmentCount - 1) - SegmentLayout.Length;
        if (difference != 0)
        {
            List<TrainCarSegmentSetting> tempSegments = new List<TrainCarSegmentSetting>(SegmentLayout);
            for (int i = 0; i < Mathf.Abs(difference); i++)
            {
                if (difference > 0)
                {
                    tempSegments.Add(new TrainCarSegmentSetting(0));
                } 
                else
                {
                    if (tempSegments[tempSegments.Count - 1].IsLocked)
                    {
                        TrainCarLength += Mathf.Abs(difference) - i;
                        break;
                    }

                    tempSegments.RemoveAt(tempSegments.Count - 1);
                }
            }
            SegmentLayout = tempSegments.ToArray();
        }

        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            if (transform.GetChild(i).TryGetComponent(out TrainCarSegment segment))
            {
                if (SegmentLayout.Length > segment.Index && SegmentLayout[segment.Index].IsLocked)
                {
                    continue;
                }
            }

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
            
            if (SegmentLayout[i-1].IsLocked) { continue; }

            if (!SegmentLayout[i - 1].SegmentIndex.Equals(int.MinValue))
            {
                position += Vector3.forward * MinimumSegementLength * (0.5f * (DefinedSegments[SegmentLayout[i - 1].SegmentIndex].Length - 1));
                GameObject obj = Instantiate(DefinedSegments[SegmentLayout[i - 1].SegmentIndex].SegmentPrefab, position, Quaternion.identity, transform);
                if (obj.TryGetComponent(out TrainCarSegment segment))
                {
                    segment.SetIndex(i - 1);
                }
                continue;
            }
        }
    }
}
