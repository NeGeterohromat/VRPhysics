using UnityEngine;
using TMPro;

public class Multimeter : MonoBehaviour
{
    public bool isOn = false;          // ������� �� ������ (�������� � ������ ���������)
    public Transform needle;           // ������� �������
    public float maxAngle = 90f;       // ���� ��� ������������ ����
    public float maxCurrent = 1.2f;    // ��� ���������� � 1.2 �, ��� ��������������� � 200 ���
    public TextMeshPro digitalDisplay; // �������� ������� (�����������)
    public bool isMicroAmmeter = false;

    public void TurnOn() { isOn = true; }
    public void TurnOff() { isOn = false; needle.localEulerAngles = Vector3.zero; if (digitalDisplay) digitalDisplay.text = "0"; }

    public void SetCurrent(double currentAmps)
    {
        if (!isOn) return;
        float displayValue;
        if (isMicroAmmeter)
        {
            displayValue = (float)(currentAmps); // � ������������
            float t = displayValue / 200f;             // ����� �� 200 ���
            needle.localEulerAngles = new Vector3(0, 0, -t * maxAngle);
            if (digitalDisplay != null)
                digitalDisplay.text = $"{displayValue:F3} μA";
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