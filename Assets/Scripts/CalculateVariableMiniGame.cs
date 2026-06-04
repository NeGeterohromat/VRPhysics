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
    [SerializeField] private double UaVolts;
    [SerializeField] private double RaMilliMetres;
    [SerializeField] private double LMilliMetres;
    [SerializeField] private double DMilliMetres;
    [SerializeField] private double N;
    [SerializeField] private double Ikr;

    private void Start()
    {
        //Стартовые параметры
        UaVolts = Random.Range(40, 70) / 10f;
        RaMilliMetres = Random.Range(10,50) / 10f;
        LMilliMetres = Random.Range(40, 60);
        DMilliMetres = Random.Range(30, 50);
        N = Random.Range(430, 580);
        Ikr = calculator.GetIkr(UaVolts, RaMilliMetres, LMilliMetres, DMilliMetres, N);

        //Выбираем один случайный, который загадаем
        switch (Random.Range(0, 7))
        {
            case 0:
                correctAnswer = 1.76;
                varText.text = $"Ua = {UaVolts}В\nRa = {RaMilliMetres}мм\nL = {LMilliMetres}мм\nD = {DMilliMetres}мм\nN = {N}\nIкр = {Ikr:F3}А\nμ0 = 4π*10^(-7)\nНайдите e/m*10^(-11) (ответ с точностью 2 знака после запятой)";
                break;
            case 1:
                correctAnswer = UaVolts;
                varText.text = $"e/m = 1.76*10^(11)\nRa = {RaMilliMetres}мм\nL = {LMilliMetres}мм\nD = {DMilliMetres}мм\nN = {N}\nIкр = {Ikr:F3}А\nμ0 = 4π*10^(-7)\nНайдите Ua в Вольтах (ответ с точностью 2 знака после запятой)";
                break;
            case 2:
                correctAnswer = RaMilliMetres;
                varText.text = $"e/m = 1.76*10^(11)\nUa = {UaVolts}В\nL = {LMilliMetres}мм\nD = {DMilliMetres}мм\nN = {N}\nIкр = {Ikr:F3}А\nμ0 = 4π*10^(-7)\nНайдите Ra в миллиметрах (ответ с точностью 2 знака после запятой)";
                break;
            case 3:
                correctAnswer = LMilliMetres;
                varText.text = $"e/m = 1.76*10^(11)\nUa = {UaVolts}В\nRa = {RaMilliMetres}мм\nD = {DMilliMetres}мм\nN = {N}\nIкр = {Ikr:F3}А\nμ0 = 4π*10^(-7)\nНайдите L в миллиметрах (ответ с точностью 2 знака после запятой)";
                break;
            case 4:
                correctAnswer = DMilliMetres;
                varText.text = $"e/m = 1.76*10^(11)\nUa = {UaVolts}В\nRa = {RaMilliMetres}мм\nL = {LMilliMetres}мм\nN = {N}\nIкр = {Ikr:F3}А\nμ0 = 4π*10^(-7)\nНайдите D в миллиметрах (ответ с точностью 2 знака после запятой)";
                break;
            case 5:
                correctAnswer = N;
                varText.text = $"e/m = 1.76*10^(11)\nUa = {UaVolts}В\nRa = {RaMilliMetres}мм\nL = {LMilliMetres}мм\nD = {DMilliMetres}мм\nIкр = {Ikr:F3}А\nμ0 = 4π*10^(-7)\nНайдите N (ответ с точностью 2 знака после запятой)";
                break;
            case 6:
                correctAnswer = Ikr;
                varText.text = $"e/m = 1.76*10^(11)\nUa = {UaVolts}В\nRa = {RaMilliMetres}мм\nL = {LMilliMetres}мм\nD = {DMilliMetres}мм\nN = {N}\nμ0 = 4π*10^(-7)\nНайдите Iкр в Амперах (ответ с точностью 2 знака после запятой)";
                break;
        }
    }

    public void CheckAnswer()
    {
        if (Math.Abs(double.Parse(keyboard.text) - correctAnswer) < 1e-2)
        {
            correctText.text = "Правильно!";
        }
        else
        {
            correctText.text = "Неверно, попробуйте ещё раз";
        }
    }
}
