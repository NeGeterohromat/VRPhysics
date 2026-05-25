using UnityEngine;
using UnityEngine.UI;
using CSharpMath.SkiaSharp; // Основной рендерер
using SkiaSharp;
using System;

public class LatexRenderer : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("Компонент RawImage для вывода")]
    public RawImage targetRawImage;

    [Header("LaTeX Settings")]
    public string latexInput = @"\frac{-b \pm \sqrt{b^2 - 4ac}}{2a}";
    [Range(10f, 120f)] public float fontSize = 36f;
    public Color fontColor = Color.white;
    public Color backgroundColor = new Color(0,0,0,0);
    public int padding = 15;

    void Start() => RenderLatex();

    /// <summary>
    /// Генерация текстуры из LaTeX
    /// </summary>
    [ContextMenu("🔄 Render LaTeX Now")]
    public void RenderLatex()
    {
        try
        {
            // 1. Создаем painter через пустой конструктор
            var painter = new MathPainter
            {
                LaTeX = latexInput, // ✅ Парсинг происходит здесь автоматически
                FontSize = fontSize,
                //DisplayStyle = true, // Дроби и корни будут крупнее и красивее
                TextColor = ColorToSKColor(fontColor)
            };

            // 2. Измеряем итоговый размер формулы
            var size = painter.Measure();
            int width = (int)Math.Ceiling(size.Width) + padding * 2;
            int height = (int)Math.Ceiling(size.Height) + padding * 2;

            // Защита от нулевых размеров (бывает при пустой строке)
            if (width <= padding || height <= padding)
            {
                Debug.LogWarning("Формула не отрендерилась. Проверьте синтаксис LaTeX.");
                return;
            }

            // 3. Рендер в SkiaBitmap
            using (var bitmap = new SKBitmap(width, height))
            using (var canvas = new SKCanvas(bitmap))
            {
                // Фон
                canvas.Clear(ColorToSKColor(backgroundColor));

                painter.Draw(canvas);

                // 4. Конвертация SKBitmap -> Unity Texture2D
                Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, false);

                using (var image = SKImage.FromBitmap(bitmap))
                using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
                {
                    texture.LoadImage(data.ToArray());
                }
                texture.Apply();

                // 5. Назначаем на UI
                if (targetRawImage != null)
                {
                    if (targetRawImage.texture != null && targetRawImage.texture != texture)
                        Destroy(targetRawImage.texture); // Чистим память

                    targetRawImage.texture = texture;
                    targetRawImage.SetNativeSize(); // Подстраиваем размер Image под текстуру
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"❌ Ошибка рендера LaTeX: {ex.Message}\n{ex.StackTrace}");
        }
    }

    // Вспомогательный метод конвертации цветов
    private SKColor ColorToSKColor(Color c) =>
        new SKColor((byte)(c.r * 255), (byte)(c.g * 255), (byte)(c.b * 255), (byte)(c.a * 255));
}