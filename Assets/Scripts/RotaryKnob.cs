using UnityEngine;

public class RotaryKnob : MonoBehaviour
{
    public float currentIc = 0.4f;
    public float step = 0.02f;
    public float minIc = 0f;
    public float maxIc = 0.8f;
    public MagnetronExperiment experiment;
    public float rotationRange = 270f; // диапазон поворота ручки

    void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
            currentIc += step;
        else if (Input.GetMouseButtonDown(1))
            currentIc -= step;

        currentIc = Mathf.Clamp(currentIc, minIc, maxIc);
        float angle = Mathf.Lerp(0, rotationRange, (currentIc - minIc) / (maxIc - minIc));
        transform.localEulerAngles = new Vector3(0, 0, -angle);

        if (experiment != null)
            experiment.SetSolenoidCurrent(currentIc);
    }
}