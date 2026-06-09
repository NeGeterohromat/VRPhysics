using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ElectronChargeCalculator : MonoBehaviour
{
    [Header("Слайдеры")]
    public Slider sliderUa;      // Вольты
    public Slider sliderL;       // метры
    public Slider sliderD;       // метры
    public Slider sliderRa;      // метры
    public Slider sliderN;       // количество витков
    public Slider sliderIkr;     // амперы

    [Header("Текстовые поля для отображения значений")]
    public TMP_Text valueUa;
    public TMP_Text valueL;
    public TMP_Text valueD;
    public TMP_Text valueRa;
    public TMP_Text valueN;
    public TMP_Text valueIkr;

    [Header("Поле для результата")]
    public TMP_Text resultText;

    [Header("Подсказка")]
    public TMP_Text hintText;    

    // Табличное значение e/m (Кл/кг)
    private const double E_TABLE = 1.7588e11;

    // Физические константы
    private const double MU0 = 4 * Mathf.PI * 1e-7;   

    void Start()
    {
        sliderUa.onValueChanged.AddListener(delegate { UpdateUI(); });
        sliderL.onValueChanged.AddListener(delegate { UpdateUI(); });
        sliderD.onValueChanged.AddListener(delegate { UpdateUI(); });
        sliderRa.onValueChanged.AddListener(delegate { UpdateUI(); });
        sliderN.onValueChanged.AddListener(delegate { UpdateUI(); });
        sliderIkr.onValueChanged.AddListener(delegate { UpdateUI(); });

        UpdateUI();
    }

    public void UpdateUI()
    {
        valueUa.text = sliderUa.value.ToString("F2");
        valueL.text = sliderL.value.ToString("F3");
        valueD.text = sliderD.value.ToString("F3");
        valueRa.text = sliderRa.value.ToString("F4");
        valueN.text = sliderN.value.ToString("F0");
        valueIkr.text = sliderIkr.value.ToString("F2");

        if (sliderRa.value < 0.002f)
        {
            hintText.text = "Радиус анода слишком мал — электроны не долетают!";
        }
        else
        {
            hintText.text = "";
        }

        // Расчёт e/m
        double e_over_m = CalculateEtoM();
        double deviation = (e_over_m - E_TABLE) / E_TABLE * 100.0;

        // Форматирование результата
        string resultString = $"e/m = {e_over_m.ToString("E3")} Кл/кг\n";
        resultString += $"Отклонение: {deviation.ToString("F2")}%";
        if (Mathf.Abs((float)deviation) < 1.0f)
            resultString += " ✓ (в пределах 1%)";
        else
            resultString += " ✗ (высокая погрешность)";

        resultText.text = resultString;
    }

    private double CalculateEtoM()
    {
        double Ua = sliderUa.value;
        double L = sliderL.value;
        double D = sliderD.value;
        double Ra = sliderRa.value;
        double N = sliderN.value;
        double Ikr = sliderIkr.value;

        double numerator = 8.0 * Ua * (L * L + D * D);
        double denominator = Ra * Ra * MU0 * MU0 * Ikr * Ikr * N * N;
        double result = numerator / denominator;

        return result;
    }

    public void ResetToDefaults()
    {
        sliderUa.value = 5.0f;
        sliderL.value = 0.05f;
        sliderD.value = 0.04f;
        sliderRa.value = 0.0025f;
        sliderN.value = 512f;
        sliderIkr.value = 0.57f;
        UpdateUI();
    }

    public void ShowGeneralHint()
    {
        hintText.text = "Поизменяй значения в формуле и посмотри на новый результат. Чувствительность e/m к Ra и Iкр особенно высока!";
        Invoke("ClearHint", 4.0f);
    }

    private void ClearHint()
    {
        if (sliderRa.value >= 0.002f)
            hintText.text = "";
    }
}