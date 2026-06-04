using UnityEngine;
using XCharts.Runtime;
using System.Collections.Generic;
using TMPro;
using VRKeys;

public class ScatterChartFiller : MonoBehaviour
{
    [SerializeField] private ScatterChart _chart;
    [SerializeField] private Keyboard keyboard;
    [SerializeField] private TextMeshProUGUI inputY;


    //Vremenno
    [ContextMenu("🔄 Render Graph Now")]
    public void AddData() => AddData(double.Parse(keyboard.text), double.Parse(inputY.text));
    //Vremenno

    private string seriaName = "Ia by Ic";

    public void AddData(double x, double y)
    {
        _chart.AddData(seriaName, x, y);
    }
}