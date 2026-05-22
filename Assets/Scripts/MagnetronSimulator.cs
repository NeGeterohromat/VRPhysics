using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

/// <summary>
/// Симулятор лабораторной работы «Измерение удельного заряда электрона методом магнетрона».
/// Реализует зависимость анодного тока Iа от тока соленоида Iс по сбросовой характеристике.
/// </summary>
public class MagnetronSimulator : MonoBehaviour
{
    [Header("Параметры установки (из методички)")]
    [Tooltip("Анодное напряжение, В")]
    public double Ua = 5.0;                // 5.0 ± 0.1 В
    [Tooltip("Радиус анода, мм")]
    public double Ra_mm = 2.50;            // 2.50 ± 0.01 мм
    [Tooltip("Длина соленоида, мм")]
    public double L_mm = 50.00;            // 50.00 ± 0.01 мм
    [Tooltip("Диаметр соленоида, мм")]
    public double D_mm = 40.00;            // 40.00 ± 0.01 мм
    [Tooltip("Число витков соленоида")]
    public int N = 512;                    // 512 ± 10

    [Header("Эмпирическая подгонка")]
    [Tooltip("Коэффициент уменьшения максимального анодного тока (приводит расчётное значение к реальному порядку 10–200 мкА)")]
    public double currentReductionFactor = 100.0;
    [Tooltip("Крутизна сбросовой характеристики (чем меньше, тем резче падение)")]
    public double steepness = 0.04;

    [Header("UI элементы управления")]
    public Slider icSlider;                // Ползунок для тока соленоида (0 … 1.2 А)
    public TMP_Text icValueText;           // Отображение Iс (А)
    public TMP_Text iaValueText;           // Отображение Iа (мкА)
    public TMP_Text constantsText;         // Текст с константами установки
    public Transform ammeterNeedle;        // Стрелка микроамперметра (опционально)
    public float maxNeedleAngle = 90f;     // Максимальный угол отклонения стрелки

    // Физические константы (СИ)
    private const double eps0 = 8.854187817e-12;   // Ф/м
    private const double e = 1.60217662e-19;       // Кл
    private const double m = 9.1093837e-31;        // кг
    private const double mu0 = 4 * Math.PI * 1e-7; // Гн/м

    // Рассчитываемые параметры
    private double Ra;          // радиус анода (м)
    private double L;           // длина соленоида (м)
    private double D;           // диаметр соленоида (м)
    private double Ia_max;      // максимальный анодный ток при Iс = 0 (мкА)
    private double I_crit;      // критический ток соленоида (А)

    private void Start()
    {
        // Перевод размеров в метры
        Ra = Ra_mm * 1e-3;
        L = L_mm * 1e-3;
        D = D_mm * 1e-3;

        // 1. Максимальный анодный ток по закону Чайлда-Ленгмюра (переводим в микроамперы)
        double sqrt2em = Math.Sqrt(2 * e / m);                     // sqrt(2e/m)
        double Imax_amps = (4 * Math.PI * eps0 / 9.0) *
                           sqrt2em *
                           L *
                           Math.Pow(Ua, 1.5) /
                           Ra;
        Ia_max = Imax_amps * 1e6 / currentReductionFactor;

        // 2. Критический ток соленоида из формулы (9) методички
        //    (используем табличное значение удельного заряда e/m)
        double e_over_m = e / m;                                   // ~1.7588e11 Кл/кг
        double numerator = Math.Sqrt(8 * Ua * (L * L + D * D) / e_over_m);
        double denominator = Ra * mu0 * N;
        I_crit = numerator / denominator;

        // Отображаем параметры установки на UI
        if (constantsText != null)
        {
            constantsText.text = $"Параметры установки:\n" +
                                 $"Ua = {Ua:F1} ± 0.1 В\n" +
                                 $"Ra = {Ra_mm:F2} ± 0.01 мм\n" +
                                 $"L = {L_mm:F2} ± 0.01 мм\n" +
                                 $"D = {D_mm:F2} ± 0.01 мм\n" +
                                 $"N = {N} ± 10";
        }

        // Настройка слайдера тока соленоида
        if (icSlider != null)
        {
            icSlider.minValue = 0f;
            icSlider.maxValue = 1.2f;      // Максимальный ток 1.2 А (диапазон лабораторной работы)
            icSlider.value = 0.4f;         // Стартовое значение 0.4 А, как указано в инструкции
            icSlider.onValueChanged.AddListener(OnIcChanged);
        }

        // Первичный расчёт
        OnIcChanged(icSlider != null ? icSlider.value : 0.4f);
    }

    /// <summary>
    /// Вызывается при изменении тока соленоида (повороте ручки потенциометра).
    /// </summary>
    /// <param name="ic">Ток соленоида в амперах</param>
    public void OnIcChanged(float ic)
    {
        double Ic = ic;
        // Расчёт анодного тока по логистической (сигмоидальной) кривой.
        // При Ic = I_crit значение функции равно 0.5, то есть ток падает вдвое.
        double Ia = Ia_max * Logistic(I_crit, Ic, steepness);
        if (iaValueText != null)
            iaValueText.text = $"{Ia:F1} мкА";
        if (icValueText != null)
            icValueText.text = $"{Ic:F3} А";

        // Анимация стрелки микроамперметра (если назначена)
        if (ammeterNeedle != null)
        {
            float t = Mathf.Clamp01((float)(Ia / Ia_max));
            float angle = maxNeedleAngle * t;
            ammeterNeedle.localEulerAngles = new Vector3(0, 0, -angle);
        }
    }

    /// <summary>
    /// Логистическая функция, моделирующая сбросовую характеристику магнетрона.
    /// </summary>
    private double Logistic(double Icrit, double Ic, double steepness)
    {
        // 1 / (1 + exp((Ic - Icrit)/steepness))
        return 1.0 / (1.0 + Math.Exp((Ic - Icrit) / steepness));
    }
}