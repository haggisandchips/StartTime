using Velopack;
using Velopack.Sources;

namespace StartTime.App;

/// <summary>
/// Wraps Velopack update checks against the app's GitHub releases.
/// Set <see cref="RepoUrl"/> to the GitHub repository that the release workflow publishes to.
/// </summary>
public static class UpdateService
{
    private const string RepoUrl = "https://github.com/haggisandchips/StartTime";

    private static UpdateManager CreateManager() => new(new GithubSource(RepoUrl, null, false));

    /// <summary>
    /// Checks for, and downloads, an available update. Invokes <paramref name="onUpdateReady"/>
    /// with the new version once the download completes; never throws.
    /// </summary>
    public static async Task CheckForUpdatesAsync(Action<string> onUpdateReady)
    {
        try
        {
            var manager = CreateManager();
            if (!manager.IsInstalled)
                return;

            var updateInfo = await manager.CheckForUpdatesAsync();
            if (updateInfo is null)
                return;

            await manager.DownloadUpdatesAsync(updateInfo);
            onUpdateReady(updateInfo.TargetFullRelease.Version.ToString());
        }
        catch
        {
            // Update checks are best-effort and must never block or crash the app.
        }
    }

    /// <summary>Applies the already-downloaded update and restarts the app.</summary>
    public static void ApplyUpdateAndRestart()
    {
        try
        {
            CreateManager().ApplyUpdatesAndRestart(null);
        }
        catch
        {
            // If this fails the panel simply stays open with the update still pending.
        }
    }
}
