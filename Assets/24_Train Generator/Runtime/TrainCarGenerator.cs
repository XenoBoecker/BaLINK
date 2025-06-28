using BetterAttributes;
using System;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
[AddComponentMenu("Custom/Train Generator")]
public class TrainCarGenerator : MonoBehaviour
{
    [SerializeField, HideInInspector] private TrainCarSegmentSetting[] _segmentLayout;
    [SerializeField] private float _trainCarLength = 1;
    [SerializeField] private float _minimumSegementLength = 1;

    [Header("Setgment Settings")]
    [SerializeField] private GameObject _carEndSegment;
    [SerializeField] private TrainCarSegmentSetup[] _definedSegments;

    [Header("Reflection Probe Settings")]
    [SerializeField] private int _numberOfReflectionProbes = 1;
    [SerializeField] private float _probeWidth = 5;
    [SerializeField] private float _probeHeight = 5;
    [SerializeField] private GameObject _reflectionProbePrefab;
    [SerializeField, HideInInspector] private ReflectionProbeSetup[] _reflectionProbes;

    public TrainCarSegmentSetting[] SegmentLayout => _segmentLayout;
    public float MinimumSegementLength => _minimumSegementLength;
    public float ProbeWidth => _probeWidth;
    public float ProbeHeight => _probeHeight;
    public TrainCarSegmentSetup[] DefinedSegments => _definedSegments;
    public float TrainCarLength => _trainCarLength;
    public int NumberOfReflectionProbes => _numberOfReflectionProbes;


    public int RequiredSegmentCount { get { return Mathf.CeilToInt(TrainCarLength / MinimumSegementLength); } }

    [Button("Regenerate Car")]
    public void RegenerateCar()
    {
        if (SegmentLayout == null)
        {
            _segmentLayout = new TrainCarSegmentSetting[0];
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
                        _trainCarLength += Mathf.Abs(difference) - i;
                        break;
                    }
                    tempSegments.RemoveAt(tempSegments.Count - 1);
                }
            }
            _segmentLayout = tempSegments.ToArray();
        }

        if (NumberOfReflectionProbes != _reflectionProbes.Length)
        {
            int differenceInRelfectionProbeCount = NumberOfReflectionProbes - _reflectionProbes.Length;
            List<ReflectionProbeSetup> tempProbes = new List<ReflectionProbeSetup>(_reflectionProbes);

            for (int i = 0; i < Mathf.Abs(differenceInRelfectionProbeCount); i++)
            {
                if (differenceInRelfectionProbeCount > 0)
                {
                    tempProbes.Add(Instantiate(_reflectionProbePrefab, transform).GetComponent<ReflectionProbeSetup>());
                } 
                else
                {
                    DestroyImmediate(tempProbes[tempProbes.Count - 1].gameObject);
                    tempProbes.RemoveAt(tempProbes.Count - 1);
                }
            }

            _reflectionProbes = tempProbes.ToArray();
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
            if (transform.GetChild(i).TryGetComponent(out ReflectionProbeSetup probe))
            {
                continue;
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

        for (int i = 0; i < _reflectionProbes.Length; i++)
        {
            if (_reflectionProbes[i] == null) { continue; }
            _reflectionProbes[i].SetProbeBounds(this, i);
        }
    }

    public void SetTrainCarLength(float newLength)
    {
        if (newLength < 1)
        {
            _trainCarLength = 1;
        } 
        else
        {
            _trainCarLength = newLength;
        }
    }
}
