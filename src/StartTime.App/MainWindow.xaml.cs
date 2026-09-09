using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using StartTime.Core;

namespace StartTime.App;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        Loaded += MainWindow_Loaded;
    }

    private void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        PositionTopRight();
        _ = LoadEarliestEventAsync();
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
            TimestampText.Text = result.HasValue
                ? result.Value.ToString("HH:mm:ss")
                : "No events";
        }
        catch
        {
            TimestampText.Text = "Unavailable";
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
