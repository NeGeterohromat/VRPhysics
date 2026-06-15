using System.Collections.Generic;
using System;
using TMPro;
using UnityEditor;
using UnityEngine;

public class ModelMeasurementsData : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI UaText;
    [SerializeField] private TextMeshProUGUI RaText;
    [SerializeField] private TextMeshProUGUI LText;
    [SerializeField] private TextMeshProUGUI DText;
    [SerializeField] private TextMeshProUGUI NText;
    [SerializeField] private Transform tableParent;
    [SerializeField] private ScatterChartFiller IaChart;
    [SerializeField] private ScatterChartFiller DerivativeChart;

    private TextMeshProUGUI[][] table;

    private List<(double, double)> measurements;

    private void Start()
    {
        measurements = new();
        table = new TextMeshProUGUI[20][];
        for (int i = 0; i < 20 * 6; i++)
        {
            int row = i / 6;
            int col = i % 6;
            if (col == 0)
                table[row] = new TextMeshProUGUI[6];

            table[row][col] = tableParent.GetChild(i).GetComponent<TextMeshProUGUI>();
        }

    }

    public void ViewModelParameters(double Ua, double Ra, double L, double D, double N)
    {
        UaText.text = $"{Ua} В";
        RaText.text = $"{Ra} мм";
        LText.text = $"{L} мм";
        DText.text = $"{D} мм";
        NText.text = $"{N}";
    }

    public void AddMeasurement(double Ic, double Ia)
    {
        if (measurements.Count == 20) return;

        measurements.Add((Ic,Ia));
        var row = table[measurements.Count - 1];
        row[0].text = $"{Ic:F3}";
        row[1].text = $"{Ia:F3}";
        IaChart.AddData(Ic,Ia);
        if (measurements.Count > 1) 
        {
            var deltaIc = Math.Abs(measurements[measurements.Count - 2].Item1 - Ic);
            var deltaIa = Math.Abs(measurements[measurements.Count - 2].Item2 - Ia);
            row[2].text = $"{deltaIa:F3}";
            row[3].text = $"{deltaIc:F3}";
            row[4].text = $"{deltaIa / deltaIc:F3}";
            row[5].text = $"{(measurements[measurements.Count - 2].Item1 + Ic) / 2:F3}";
            DerivativeChart.AddData(Ic, Math.Round(deltaIa / deltaIc, 4));
        }
    }
}
