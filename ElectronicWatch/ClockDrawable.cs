using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicWatch;

public class ClockDrawable : IDrawable
{
    // Размеры одного "пикселя" и отступы
    private const float PixelSize = 12f;
    private const float PixelSpacing = 4f;
    private const float DigitSpacing = 20f;

    // === ЦВЕТА (Вариант 2: Неоновый красный) ===
    // Включенный пиксель (Яркий неоновый красный)
    private readonly Color _onColor = Color.FromArgb("#FF3333");
    // Выключенный пиксель (Темно-бордовый, чтобы матрица смотрелась красиво)
    private readonly Color _offColor = Color.FromArgb("#2A0000");

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        // 1. Очищаем фон (черный)
        canvas.FillColor = Colors.Black;
        canvas.FillRectangle(dirtyRect);

        // 2. Получаем текущее время
        var now = DateTime.Now;
        string timeString = now.ToString("HH:mm:ss");

        // 3. Настройки для центрирования
        float totalWidth = CalculateTotalWidth(timeString);
        float startX = (dirtyRect.Width - totalWidth) / 2;
        float startY = dirtyRect.Height / 2 - (7 * (PixelSize + PixelSpacing)) / 2;

        // 4. Рисуем каждый символ
        float currentX = startX;
        foreach (char c in timeString)
        {
            if (c == ':')
            {
                DrawCharacter(canvas, Font.Colon, currentX, startY);
                currentX += 5 * (PixelSize + PixelSpacing) + DigitSpacing;
            }
            else if (char.IsDigit(c))
            {
                int digitIndex = c - '0';
                byte[] digitData = new byte[35];
                for (int i = 0; i < 35; i++) digitData[i] = Font.Digits[digitIndex, i];

                DrawCharacter(canvas, digitData, currentX, startY);
                currentX += 5 * (PixelSize + PixelSpacing) + DigitSpacing;
            }
        }
    }

    private void DrawCharacter(ICanvas canvas, byte[] matrix, float startX, float startY)
    {
        for (int row = 0; row < 7; row++)
        {
            for (int col = 0; col < 5; col++)
            {
                int index = row * 5 + col;
                bool isOn = matrix[index] == 1;

                float x = startX + col * (PixelSize + PixelSpacing);
                float y = startY + row * (PixelSize + PixelSpacing);

                // Рисуем прямоугольник с закругленными углами (пиксель)
                canvas.FillColor = isOn ? _onColor : _offColor;

                float radius = PixelSize / 2f;
                canvas.FillRoundedRectangle(x, y, PixelSize, PixelSize, radius);
            }
        }
    }

    private float CalculateTotalWidth(string text)
    {
        float width = 0;
        foreach (char c in text)
        {
            width += 5 * (PixelSize + PixelSpacing) + DigitSpacing;
        }
        return width - DigitSpacing;
    }
}