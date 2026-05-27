using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class WireConnector : MonoBehaviour
{
    public string socketId;          // ���������� ������������� ������
    public bool isConnected = false;
    private GameObject wire;
    private Button button;
    private MagnetronExperiment experiment;

    private void Start()
    {
        experiment = FindObjectOfType<MagnetronExperiment>();
        button = GetComponentInChildren<Button>();
        if (!(button is null))
        {
            button.onClick.AddListener(() => 
            {
                if (experiment != null)
                    experiment.HandleSocketClick(this);
            });
        }
    }

    void OnMouseDown()
    {
        Debug.Log("sss");
        if (experiment != null)
            experiment.HandleSocketClick(this);
    }

    public void CreateWire(GameObject targetSocket)
    {
        // ������� ���������� ��������� ����� ������� ������� � targetSocket
        // ����� ������������ LineRenderer ��� ������� ������� �������
        wire = new GameObject("Wire");
        LineRenderer lr = wire.AddComponent<LineRenderer>();
        lr.positionCount = 2;
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, targetSocket.transform.position);
        lr.startWidth = 0.01f;
        lr.endWidth = 0.01f;
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startColor = Color.yellow;
        lr.endColor = Color.yellow;
    }

    public void RemoveWire()
    {
        if (wire != null) Destroy(wire);
    }
}