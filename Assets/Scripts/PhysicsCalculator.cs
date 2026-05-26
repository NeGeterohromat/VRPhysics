using TMPro;
using UnityEngine;
using System;
using MathNet.Numerics.Distributions;
using VRKeys;

public class PhysicsCalculator : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI output;
    [SerializeField] private Keyboard keyboard;

    //параметры самой установки.
    public double UaVolts = 6;
    public double RaMilliMetres = 2.5;
    public double LMilliMetres = 50;
    public double DMilliMetres = 40;
    public double N = 490;

    //физические константы. _n в имени означает умножение на 10^(-n)
    private const double e_19 = 1.602;
    private const double me_31 = 9.109;
    private const double eps0_12 = 8.854;
    private const double mu0_7 = Math.PI * 4;

    //с точки зрения формул всё верно, но, видимо, из-за конструкции установки реальный анодный ток Ia сильно ниже рассчётного. 
    private const double realisticCoef = 15;

    //параметры плавного падения Ia для установки
    private double Ia_maksimalnoe_mikroAmpery;
    private double mu_Ikr;
    private const double sigma = 0.04;

    private void Start()
    {
        keyboard.OnSubmit.AddListener((s) => CalculateIaByInput());
    }

    [ContextMenu("🔄 Render Physics Now")]
    public void CalculateIaByInput()
    {
        try
        {
            double inp = double.Parse(keyboard.text);
            var Ia = CalculateIa(inp);
            output.text = Ia.ToString();// + " " + Ia_maksimalnoe_mikroAmpery + " " + mu_Ikr;
        }
        catch (Exception e)
        {
            output.text = $"Error in input: {e}";
        }
    }

    public double CalculateIa(double inp) =>
        CalculateIa(inp, UaVolts, RaMilliMetres, LMilliMetres, DMilliMetres, N);

    public double CalculateIa(double inp, double UaVolts, double RaMilliMetres, double LMilliMetres,
        double DMilliMetres, double N)
    {
        //закон Ленгмюра-Чайлда
        Ia_maksimalnoe_mikroAmpery = 4 * Math.PI * eps0_12 / 9
            * Math.Sqrt(2 * e_19 / me_31)
            * LMilliMetres * Math.Pow(UaVolts, 3d / 2) / RaMilliMetres
            / realisticCoef;

        mu_Ikr = GetIkr();

        return Ia_maksimalnoe_mikroAmpery * (1 - Laplace.CDF(mu_Ikr, sigma, inp));
    }

    public double GetIkr() => GetIkr(UaVolts, RaMilliMetres, LMilliMetres, DMilliMetres, N);

    //I критическое по формуле (9) (-31-3-3+19+3+3+7+7)/2 = 1 => остаётся домножить на 10^1;
    public double GetIkr(double UaVolts, double RaMilliMetres, double LMilliMetres,
        double DMilliMetres, double N) =>
        Math.Sqrt(me_31 * 8 * UaVolts * (LMilliMetres * LMilliMetres + DMilliMetres * DMilliMetres)
            / e_19 / RaMilliMetres / RaMilliMetres / mu0_7 / mu0_7 / N / N) * 10;

}