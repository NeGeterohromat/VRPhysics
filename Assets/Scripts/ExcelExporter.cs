using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ExcelExporter : MonoBehaviour
{
    private string filePath;

    void Start()
    {
        // ����� persistentDataPath �������������� �������� ��� ������
        // (�� Windows ��� ������: C:\Users\<user>\AppData\LocalLow\<company>\<product>)
        filePath = Path.Combine(Application.persistentDataPath, "magnetron_data.csv");
        Debug.Log("���� ����� �������: " + filePath);
    }

    // ���������� ��� ������ ���� ���������
    public void ExportToCSV(List<ExperimentData.MeasurementPoint> measurements)
    {
        if (measurements == null || measurements.Count == 0)
        {
            Debug.LogWarning("��� ������ ��� ��������");
            return;
        }

        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine("Ic (A);Ia (���)");
            foreach (var point in measurements)
            {
                double iaMicro = point.Ia * 1e6;
                string icStr = point.Ic.ToString("0.######", System.Globalization.CultureInfo.InvariantCulture);
                string iaStr = iaMicro.ToString("0.###", System.Globalization.CultureInfo.InvariantCulture);
                writer.WriteLine($"{icStr};{iaStr}");
            }
        }
        Debug.Log($"������ ���������: {filePath}");
#if UNITY_EDITOR
        UnityEditor.EditorUtility.RevealInFinder(filePath);
#endif
    }

    // ������������: ���������� ����� �� ����� (��������� � ����� �����)
    public void AppendPoint(double ic, double ia)
    {
        bool fileExists = File.Exists(filePath);
        using (StreamWriter writer = new StreamWriter(filePath, true))
        {
            if (!fileExists)
                writer.WriteLine("Ic (A);Ia (���)");
            double iaMicro = ia * 1e6;
            string icStr = ic.ToString("0.######", System.Globalization.CultureInfo.InvariantCulture);
            string iaStr = iaMicro.ToString("0.###", System.Globalization.CultureInfo.InvariantCulture);
            writer.WriteLine($"{icStr};{iaStr}");
        }
    }
}