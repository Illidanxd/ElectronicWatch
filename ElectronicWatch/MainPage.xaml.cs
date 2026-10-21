using System.Timers;

namespace ElectronicWatch;

public partial class MainPage : ContentPage
{
    private ClockDrawable _clockDrawable;
    private System.Timers.Timer _timer;

    public MainPage()
    {
        InitializeComponent();

        // Создаем объект для рисования
        _clockDrawable = new ClockDrawable();
        ClockView.Drawable = _clockDrawable;

        // Настраиваем таймер на обновление каждую секунду
        _timer = new System.Timers.Timer(1000);
        _timer.Elapsed += OnTimerElapsed;
        _timer.AutoReset = true;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _timer.Start(); // Запускаем таймер при появлении страницы
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _timer.Stop(); // Останавливаем таймер, когда страница скрывается
    }

    private void OnTimerElapsed(object? sender, ElapsedEventArgs e)
    {
        // Вызов перерисовки должен происходить в UI-потоке
        MainThread.BeginInvokeOnMainThread(() =>
        {
            // Эта команда заставляет GraphicsView перерисоваться
            ClockView.Invalidate();
        });
    }
}