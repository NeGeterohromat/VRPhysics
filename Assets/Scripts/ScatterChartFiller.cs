using UnityEngine;
using XCharts.Runtime;
using System.Collections.Generic;
using TMPro;
using VRKeys;

public class ScatterChartFiller : MonoBehaviour
{
    [SerializeField] private ScatterChart _chart;

    private string seriaName = "Ia by Ic";

    public void AddData(double x, double y)
    {
        _chart.AddData(seriaName, x, y);
    }
}