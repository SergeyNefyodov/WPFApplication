using System.Diagnostics;
using Microsoft.Web.WebView2.Core;
using System.IO;
using System.Text.Json;

namespace WPFApplication.DataExchangeExample;

public partial class WebView2BrowserWindow
{
    private readonly WindowDto _dto;
    private readonly WindowService _windowService;

    public WebView2BrowserWindow(WindowDto dto, WindowService windowService)
    {
        _dto = dto;
        _windowService = windowService;
        InitializeComponent();
        InitializeWebViewAsync();
    }

    private async void InitializeWebViewAsync()
    {
        try
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "WpfApplication");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var env = await CoreWebView2Environment.CreateAsync(userDataFolder: path);
            await WebView.EnsureCoreWebView2Async(env);
            WebView.CoreWebView2.NavigationCompleted += OnNavigationCompleted;
            WebView.CoreWebView2.WebMessageReceived += OnWebMessageReceived;
            WebView.Source = new Uri(@"path-to-your-html-file.html");
        }
        catch (Exception exception)
        {
            //async-related errors
            Debug.WriteLine(exception);
        }
    }

    private void OnNavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs args)
    {
        WebView.CoreWebView2.PostWebMessageAsString(JsonSerializer.Serialize(_dto));
    }

    private void OnWebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs args)
    {
        var json = args.WebMessageAsJson;
        var updated = JsonSerializer.Deserialize<WindowDto>(json);
        if (updated == null) return;

        Dispatcher.Invoke(() => _windowService.UpdateWindowHeight(updated));
    }
}