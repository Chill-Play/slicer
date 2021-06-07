using UnityEngine;
using System.Collections;

public class SignalSmootherVector3 : SignalSmoother<Vector3>
{
    #region Constructors

    public SignalSmootherVector3(int samplesCount) : base(samplesCount)
    {
    }

    #endregion



    #region Public methods

    public override Vector3 GetAverage()
    {
        Vector3 average = Vector3.zero;
        if (inputSamples.Count > 0)
        {
            for (int i = 0; i < inputSamples.Count; i++)
            {
                average += inputSamples[i];
            }
            average /= inputSamples.Count;
        }

        return average;

    }

    #endregion
}
