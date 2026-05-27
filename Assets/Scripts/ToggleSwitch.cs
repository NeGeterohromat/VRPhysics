using UnityEngine;
using UnityEngine.Events;

public class ToggleSwitch : MonoBehaviour
{
    public bool isOn = false;
    public UnityEvent onToggleOn;
    public UnityEvent onToggleOff;
    public GameObject switchHandle;   // ������ ����� (��� ��������)

    void OnMouseDown()
    {
        isOn = !isOn;
        // �������� ��������� �����
        if (switchHandle != null)
            switchHandle.transform.localEulerAngles = isOn ? new Vector3(0, 0, -30) : Vector3.zero;

        if (isOn) onToggleOn.Invoke();
        else onToggleOff.Invoke();
    }
}