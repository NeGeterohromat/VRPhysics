using TMPro;
using UnityEngine;
using VRKeys;
using System;
using Random = UnityEngine.Random;

public class CalculateVariableMiniGame : MonoBehaviour
{
    [SerializeField] private Keyboard keyboard;
    [SerializeField] private TextMeshProUGUI varText;
    [SerializeField] private TextMeshProUGUI correctText;
    [SerializeField] private PhysicsCalculator calculator;

    private double correctAnswer;

    //параметры самой установки.
    private double UaVolts;
    private double RaMilliMetres;
    private double LMilliMetres;
    private double DMilliMetres;
    private double N;
    private double Ikr;

    private void Start()
    {
        UaVolts = Random.Range(40, 70) / 10f;
        RaMilliMetres = Random.Range(10,50) / 10f;
        LMilliMetres = Random.Range(40, 60);
        DMilliMetres = Random.Range(30, 50);
        N = Random.Range(430, 580);
        Ikr = calculator.GetIkr(UaVolts, RaMilliMetres, LMilliMetres, DMilliMetres, N);
        switch (Random.Range(0, 7))
        {
            case 0:
                correctAnswer = 1.76;
                varText.text = $"UaVolts = {UaVolts}В\nRa = {RaMilliMetres}мм\nL = {LMilliMetres}мм\nD = {DMilliMetres}мм\nN = {N}\nIкр = {Ikr}А\nμ0 = 4π*10^(-7)\nНайдите e/m*10^(-11) (ответ с точностью 2 знака после запятой)";
                break;
        }
    }
}
