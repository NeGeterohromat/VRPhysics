using UnityEngine;

public class RotaryKnob : MonoBehaviour
{
    public float currentIc = 0.4f;
    public float step = 0.02f;
    public float minIc = 0f;
    public float maxIc = 0.8f;
    public MagnetronExperiment experiment;

    void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
            currentIc += step;
        else if (Input.GetMouseButtonDown(1))
            currentIc -= step;

        currentIc = Mathf.Clamp(currentIc, minIc, maxIc);

        if (experiment != null)
            experiment.SetSolenoidCurrent(currentIc);
    }
}