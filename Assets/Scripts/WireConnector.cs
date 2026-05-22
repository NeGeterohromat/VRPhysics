using UnityEngine;

public class WireConnector : MonoBehaviour
{
    public string socketId;          // Уникальный идентификатор гнезда
    public bool isConnected = false;
    private GameObject wire;

    void OnMouseDown()
    {
        MagnetronExperiment experiment = FindObjectOfType<MagnetronExperiment>();
        if (experiment != null)
            experiment.HandleSocketClick(this);
    }

    public void CreateWire(GameObject targetSocket)
    {
        // Создать визуальный проводник между текущим гнездом и targetSocket
        // Можно использовать LineRenderer или готовую префабу провода
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