using System.Windows;
using Velopack;

namespace StartTime.App;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        // Must run first: handles Velopack's install/uninstall/update lifecycle hooks.
        VelopackApp.Build().Run();

        base.OnStartup(e);

        _ = UpdateService.CheckForUpdatesAsync(version =>
        {
            Dispatcher.Invoke(() =>
            {
                if (MainWindow is MainWindow startTimeWindow)
                    startTimeWindow.ShowUpdateReady(version);
            });
        });
    }
}
