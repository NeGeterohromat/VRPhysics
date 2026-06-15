using System.Collections.Generic;
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
        measurements.Add((Ic,Ia));
        var row = table[measurements.Count - 1];
        row[0].text = $"{Ic:F3}";
        row[1].text = $"{Ia:F3}";
    }
}
