using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using StartTime.Core;

namespace StartTime.App;

public partial class MainWindow : Window
{
    private static readonly string[] LoadingCaptions =
    {
        "Scanning event log...",
        "Rewinding the clock...",
        "Analyzing timestamps...",
    };

    private Storyboard? _clockSpin;
    private DispatcherTimer? _captionTimer;
    private int _captionIndex;

    public MainWindow()
    {
        InitializeComponent();
        Loaded += MainWindow_Loaded;
    }

    private void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        PositionTopRight();
        StartLoadingAnimation();
        _ = LoadEarliestEventAsync();
    }

    private void StartLoadingAnimation()
    {
        _clockSpin = (Storyboard)FindResource("ClockSpin");
        _clockSpin.Begin();

        _captionTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1.4) };
        _captionTimer.Tick += (_, _) =>
        {
            _captionIndex = (_captionIndex + 1) % LoadingCaptions.Length;
            LoadingCaption.Text = LoadingCaptions[_captionIndex];
        };
        _captionTimer.Start();
    }

    private void ShowResult(string text)
    {
        _clockSpin?.Stop();
        _captionTimer?.Stop();

        TimestampText.Text = text;

        var fadeOut = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(250));
        fadeOut.Completed += (_, _) => LoadingPanel.Visibility = Visibility.Collapsed;
        LoadingPanel.BeginAnimation(OpacityProperty, fadeOut);

        var fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(400))
        {
            BeginTime = TimeSpan.FromMilliseconds(150)
        };
        TimestampText.BeginAnimation(OpacityProperty, fadeIn);
    }

    private void PositionTopRight()
    {
        var workArea = SystemParameters.WorkArea;
        Left = workArea.Right - Width - 16;
        Top = workArea.Top + 16;
    }

    private async Task LoadEarliestEventAsync()
    {
        var service = new EarliestEventReportService(
            new IEventTimestampSource[]
            {
                new SystemEventLogTimestampSource("System"),
                new SystemEventLogTimestampSource("Application"),
            },
            new EarliestEventFinder(),
            new SystemClock());

        try
        {
            var result = await Task.Run(() => service.GetEarliestEventToday());
            ShowResult(result.HasValue ? result.Value.ToString("HH:mm:ss") : "No events");
        }
        catch
        {
            ShowResult("Unavailable");
        }
    }

    /// <summary>Called by <see cref="App"/> once an update has finished downloading.</summary>
    public void ShowUpdateReady(string version)
    {
        UpdateText.Text = $"Update {version} downloaded — click to restart now";
        UpdateText.Visibility = Visibility.Visible;
    }

    private void UpdateText_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        UpdateService.ApplyUpdateAndRestart();
    }

    private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.OriginalSource is not Button && !ReferenceEquals(e.OriginalSource, UpdateText))
            DragMove();
    }

    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
            Close();
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}
