using System.Collections.Generic;
using UnityEngine;

public class ExperimentData : MonoBehaviour
{
    public List<MeasurementPoint> measurements = new List<MeasurementPoint>();

    [System.Serializable]
    public struct MeasurementPoint
    {
        public double Ic;   // ��� ���������, �
        public double Ia;   // ������� ���, ���
    }

    public void AddMeasurement(double ic, double ia)
    {
        measurements.Add(new MeasurementPoint { Ic = ic, Ia = ia });
        Debug.Log($"��������� �����: Ic={ic:F3} A, Ia={ia:F1} ���");
    }

    public void ClearMeasurements()
    {
        measurements.Clear();
    }
}