using UnityEngine;
using TMPro;

public class Multimeter : MonoBehaviour
{
    public bool isOn = false;          // Включён ли прибор (селектор в нужном положении)
    public Transform needle;           // Стрелка прибора
    public float maxAngle = 90f;       // Угол при максимальном токе
    public float maxCurrent = 1.2f;    // Для амперметра – 1.2 А, для микроамперметра – 200 мкА
    public TextMeshPro digitalDisplay; // Цифровой дисплей (опционально)
    public bool isMicroAmmeter = false;

    public void TurnOn() { isOn = true; }
    public void TurnOff() { isOn = false; needle.localEulerAngles = Vector3.zero; if (digitalDisplay) digitalDisplay.text = "0"; }

    public void SetCurrent(double currentAmps)
    {
        if (!isOn) return;
        float displayValue;
        if (isMicroAmmeter)
        {
            displayValue = (float)(currentAmps * 1e6); // в микроамперах
            float t = displayValue / 200f;             // шкала до 200 мкА
            needle.localEulerAngles = new Vector3(0, 0, -t * maxAngle);
            if (digitalDisplay != null)
                digitalDisplay.text = $"{displayValue:F1} µA";
        }
        else
        {
            displayValue = (float)currentAmps;
            float t = displayValue / maxCurrent;
            needle.localEulerAngles = new Vector3(0, 0, -t * maxAngle);
            if (digitalDisplay != null)
                digitalDisplay.text = $"{displayValue:F3} A";
        }

        Debug.Log($"Multimeter {name}: SetCurrent called, isOn={isOn}, currentAmps={currentAmps}");
    }
}