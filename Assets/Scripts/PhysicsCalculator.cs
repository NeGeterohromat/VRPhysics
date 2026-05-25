using System;
using UnityEngine;

public class PhysicsCalculator : MonoBehaviour
{
    [Header("Параметры установки (из методички)")]
    public double Ua = 5.0;        // В
    public double Ra_mm = 2.50;    // мм
    public double L_mm = 50.0;     // мм
    public double D_mm = 40.0;     // мм
    public int N = 512;            // витков

    [Header("Эмпирические коэффициенты")]
    public double realisticCoef = 100.0;   // Уменьшение макс. тока до реальных ~150 мкА
    public double steepness = 0.04;        // Крутизна падения

    // Физические константы
    private const double eps0 = 8.854e-12;
    private const double e = 1.602e-19;
    private const double m = 9.109e-31;
    private const double mu0 = 4 * Math.PI * 1e-7;

    private double Ia_max;   // мкА
    private double I_crit;   // А

    void Start()
    {
        double Ra = Ra_mm * 1e-3;
        double L = L_mm * 1e-3;
        double D = D_mm * 1e-3;

        double sqrt2em = Mathf.Sqrt((float)(2 * e / m));
        double Ia_max_amps = (4 * Mathf.PI * eps0 / 9.0) *
                             sqrt2em *
                             L *
                             Mathf.Pow((float)Ua, 1.5f) /
                             Ra;
        Ia_max = Ia_max_amps / realisticCoef;

        double e_over_m = e / m;
        double numerator = Mathf.Sqrt((float)(8 * Ua * (L * L + D * D) / e_over_m));
        double denominator = Ra * mu0 * N;
        I_crit = numerator / denominator;

        Debug.Log($"PhysicsCalculator: Ia_max = {Ia_max} µA, I_crit = {I_crit} A");
    }

    public double CalculateIa(double Ic)
    {
        double Ia = Ia_max / (1.0 + Mathf.Exp((float)((Ic - I_crit) / steepness)));

        Debug.Log($"CalculateIa: Ic={Ic}, Ia={Ia} µA");

        return Ia;

    }
}