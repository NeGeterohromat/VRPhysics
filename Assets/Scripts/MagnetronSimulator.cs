using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

/// <summary>
/// ��������� ������������ ������ ���������� ��������� ������ ��������� ������� ����������.
/// ��������� ����������� �������� ���� I� �� ���� ��������� I� �� ��������� ��������������.
/// </summary>
public class MagnetronSimulator : MonoBehaviour
{
    [Header("��������� ��������� (�� ���������)")]
    [Tooltip("������� ����������, �")]
    public double Ua = 5.0;                // 5.0 � 0.1 �
    [Tooltip("������ �����, ��")]
    public double Ra_mm = 2.50;            // 2.50 � 0.01 ��
    [Tooltip("����� ���������, ��")]
    public double L_mm = 50.00;            // 50.00 � 0.01 ��
    [Tooltip("������� ���������, ��")]
    public double D_mm = 40.00;            // 40.00 � 0.01 ��
    [Tooltip("����� ������ ���������")]
    public int N = 512;                    // 512 � 10

    [Header("������������ ��������")]
    [Tooltip("����������� ���������� ������������� �������� ���� (�������� ��������� �������� � ��������� ������� 10�200 ���)")]
    public double currentReductionFactor = 100.0;
    [Tooltip("�������� ��������� �������������� (��� ������, ��� ����� �������)")]
    public double steepness = 0.04;

    [Header("UI �������� ����������")]
    public Slider icSlider;                // �������� ��� ���� ��������� (0 � 1.2 �)
    public TMP_Text icValueText;           // ����������� I� (�)
    public TMP_Text iaValueText;           // ����������� I� (���)
    public TMP_Text constantsText;         // ����� � ����������� ���������
    public Transform ammeterNeedle;        // ������� ��������������� (�����������)
    public float maxNeedleAngle = 90f;     // ������������ ���� ���������� �������

    // ���������� ��������� (��)
    private const double eps0 = 8.854187817e-12;   // �/�
    private const double e = 1.60217662e-19;       // ��
    private const double m = 9.1093837e-31;        // ��
    private const double mu0 = 4 * Math.PI * 1e-7; // ��/�

    // �������������� ���������
    private double Ra;          // ������ ����� (�)
    private double L;           // ����� ��������� (�)
    private double D;           // ������� ��������� (�)
    private double Ia_max;      // ������������ ������� ��� ��� I� = 0 (���)
    private double I_crit;      // ����������� ��� ��������� (�)

    private void Start()
    {
        // ������� �������� � �����
        Ra = Ra_mm * 1e-3;
        L = L_mm * 1e-3;
        D = D_mm * 1e-3;

        // 1. ������������ ������� ��� �� ������ ������-�������� (��������� � �����������)
        double sqrt2em = Math.Sqrt(2 * e / m);                     // sqrt(2e/m)
        double Imax_amps = (4 * Math.PI * eps0 / 9.0) *
                           sqrt2em *
                           L *
                           Math.Pow(Ua, 1.5) /
                           Ra;
        Ia_max = Imax_amps * 1e6 / currentReductionFactor;

        // 2. ����������� ��� ��������� �� ������� (9) ���������
        //    (���������� ��������� �������� ��������� ������ e/m)
        double e_over_m = e / m;                                   // ~1.7588e11 ��/��
        double numerator = Math.Sqrt(8 * Ua * (L * L + D * D) / e_over_m);
        double denominator = Ra * mu0 * N;
        I_crit = numerator / denominator;

        // ���������� ��������� ��������� �� UI
        if (constantsText != null)
        {
            constantsText.text = $"��������� ���������:\n" +
                                 $"Ua = {Ua:F1} � 0.1 �\n" +
                                 $"Ra = {Ra_mm:F2} � 0.01 ��\n" +
                                 $"L = {L_mm:F2} � 0.01 ��\n" +
                                 $"D = {D_mm:F2} � 0.01 ��\n" +
                                 $"N = {N} � 10";
        }

        // ��������� �������� ���� ���������
        if (icSlider != null)
        {
            icSlider.minValue = 0f;
            icSlider.maxValue = 1.2f;      // ������������ ��� 1.2 � (�������� ������������ ������)
            icSlider.value = 0.4f;         // ��������� �������� 0.4 �, ��� ������� � ����������
            icSlider.onValueChanged.AddListener(OnIcChanged);
        }

        // ��������� ������
        OnIcChanged(icSlider != null ? icSlider.value : 0.4f);
    }

    /// <summary>
    /// ���������� ��� ��������� ���� ��������� (�������� ����� �������������).
    /// </summary>
    /// <param name="ic">��� ��������� � �������</param>
    public void OnIcChanged(float ic)
    {
        double Ic = ic;
        // ������ �������� ���� �� ������������� (�������������) ������.
        // ��� Ic = I_crit �������� ������� ����� 0.5, �� ���� ��� ������ �����.
        double Ia = Ia_max * Logistic(I_crit, Ic, steepness);
        if (iaValueText != null)
            iaValueText.text = $"{Ia:F1} ���";
        if (icValueText != null)
            icValueText.text = $"{Ic:F3} �";

        // �������� ������� ��������������� (���� ���������)
        if (ammeterNeedle != null)
        {
            float t = Mathf.Clamp01((float)(Ia / Ia_max));
            float angle = maxNeedleAngle * t;
            ammeterNeedle.localEulerAngles = new Vector3(0, 0, -angle);
        }
    }

    /// <summary>
    /// ������������� �������, ������������ ��������� �������������� ����������.
    /// </summary>
    private double Logistic(double Icrit, double Ic, double steepness)
    {
        // 1 / (1 + exp((Ic - Icrit)/steepness))
        return 1.0 / (1.0 + Math.Exp((Ic - Icrit) / steepness));
    }
}