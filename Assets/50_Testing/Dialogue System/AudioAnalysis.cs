using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class AudioAnalysis : MonoBehaviour
{
    [SerializeField] private AudioClip _audio;
    [SerializeField] private List<float> _data;

    [SerializeField] private List<float> _timing;
    int _timingIndex;

    [SerializeField] private float _threshold;

    private AudioSource _source;

    private void Awake()
    {
        _source = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _data = GetData(100);
        _timing = AnalizeData(100, _data);

        _source.clip = _audio;
        _source.Play();
    }

    private void Update()
    {
        if (_timingIndex == _timing.Count) { return; }

        if (_source.time > _timing[_timingIndex])
        {
            _timingIndex++;
            Debug.Log("Now");
        }
    }

    private List<float> AnalizeData(int samplesPerSecond, List<float> data)
    {
        List<float> retData = new List<float>();

        for (int i = 0; i < data.Count - 1; i++)
        {
            float change = data[i + 1] - data[i];
            if (change > _threshold)
            {
                retData.Add(i / (float)samplesPerSecond);
            }
        }

        return retData;
    }

    private List<float> GetData(int samplesPerSecond)
    {
        List<float> retData = new List<float>();
        int dataSize = (int)(_audio.frequency * (1.0f / samplesPerSecond));

        for (int i = 0; i < samplesPerSecond * _audio.length; i++)
        {
            float[] data = new float[dataSize];
            _audio.GetData(data, dataSize * i);

            float sum = 0;
            for (int j = 0; j < data.Length; j++)
            {
                sum += data[j];
            }
            retData.Add(sum);
        }

        return retData;
    }
}
