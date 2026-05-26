using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ExcelExporter : MonoBehaviour
{
    private string filePath;

    void Start()
    {
        // Папка persistentDataPath гарантированно доступна для записи
        // (на Windows это обычно: C:\Users\<user>\AppData\LocalLow\<company>\<product>)
        filePath = Path.Combine(Application.persistentDataPath, "magnetron_data.csv");
        Debug.Log("Файл будет сохранён: " + filePath);
    }

    // Вызывается для записи всех измерений
    public void ExportToCSV(List<ExperimentData.MeasurementPoint> measurements)
    {
        if (measurements == null || measurements.Count == 0)
        {
            Debug.LogWarning("Нет данных для экспорта");
            return;
        }

        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine("Ic (A);Ia (мкА)");
            foreach (var point in measurements)
            {
                double iaMicro = point.Ia * 1e6;
                string icStr = point.Ic.ToString("0.######", System.Globalization.CultureInfo.InvariantCulture);
                string iaStr = iaMicro.ToString("0.###", System.Globalization.CultureInfo.InvariantCulture);
                writer.WriteLine($"{icStr};{iaStr}");
            }
        }
        Debug.Log($"Данные сохранены: {filePath}");
#if UNITY_EDITOR
        UnityEditor.EditorUtility.RevealInFinder(filePath);
#endif
    }

    // Альтернатива: записывать точки по одной (добавлять в конец файла)
    public void AppendPoint(double ic, double ia)
    {
        bool fileExists = File.Exists(filePath);
        using (StreamWriter writer = new StreamWriter(filePath, true))
        {
            if (!fileExists)
                writer.WriteLine("Ic (A);Ia (мкА)");
            double iaMicro = ia * 1e6;
            string icStr = ic.ToString("0.######", System.Globalization.CultureInfo.InvariantCulture);
            string iaStr = iaMicro.ToString("0.###", System.Globalization.CultureInfo.InvariantCulture);
            writer.WriteLine($"{icStr};{iaStr}");
        }
    }
}