using UnityEngine;
using System.Collections.Generic;

public abstract class SignalSmoother<T>
{
    #region Variables

    protected List<T> inputSamples = new List<T>();

    #endregion



    #region Constructors

    protected SignalSmoother(int samplesCount)
    {
        inputSamples.Capacity = samplesCount;
        for(int i = 0; i < samplesCount; i++)
        {
            inputSamples.Add(default(T));
        }
    }

    #endregion


    #region Public methods

    public void Clear()
    {
        for (int i = 0; i < inputSamples.Count; i++)
        {
            inputSamples[i] = default(T);
        }
    }

    public void AddInputSample(T sample)
    {
        if (inputSamples.Count == inputSamples.Capacity)
        {
            inputSamples.RemoveAt(inputSamples.Count - 1);
        }
        inputSamples.Insert(0, sample);
    }


    public abstract T GetAverage();


    #endregion
}
