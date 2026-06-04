using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ToggleSwitch : MonoBehaviour
{
    public bool isOn = false;
    public UnityEvent onToggleOn;
    public UnityEvent onToggleOff;
    public GameObject switchHandle;   // ������ ����� (��� ��������)
    private Button button;

    private void Start()
    {
        button = GetComponentInChildren<Button>();
        if (!(button is null))
        {
            button.onClick.AddListener(() =>
            {
                Toggle();
            });
        }
    }

    void OnMouseDown()
    {
        Toggle();
    }

    private void Toggle()
    {
        isOn = !isOn;
        // �������� ��������� �����
        if (switchHandle != null)
            switchHandle.transform.localEulerAngles = isOn ? new Vector3(0, 0, -30) : Vector3.zero;

        if (isOn) onToggleOn.Invoke();
        else onToggleOff.Invoke();
    }
}