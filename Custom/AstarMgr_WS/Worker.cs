using AgilogDll.MFC.Astar;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using mSwDllUtils;
using mSwDllWPFUtils;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace AstarMgr_WS;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private AstarWSLauncher _astarWSLauncher;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            await Task.Delay(60000, stoppingToken);
        }
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        await base.StartAsync(cancellationToken);

        try
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);

            FileLogger.Log(Environment.NewLine, false);
            FileLogger.Log("-----------------------------------------------", false);
            FileLogger.Log("Starting service...");
            FileLogger.Log($"Current directory: {Environment.CurrentDirectory}");
        }
        catch (Exception ex)
        {
            FileLogger.Log($"Service exception 001: {ex.Message}");
            return;
        }

        try
        {
            var global = new Global(0);
            global.InitializeSolo(AppSettings.Instance.XmlRelativePath);
            var conn = new SqlConnection(Global.Instance.ConnGlobal.ConnectionString);
            _astarWSLauncher = new AstarWSLauncher(conn, AppSettings.Instance.CHL_Id);
            _astarWSLauncher.OnNotify += _astarWSLauncher_OnNotify;
            if (!_astarWSLauncher.Init()) throw new Exception("Cannot initialize AstarWSLauncher");
        }
        catch (Exception ex)
        {
            FileLogger.Log($"Service exception 002: {ex.Message}");
            return;
        }
    }

    private void _astarWSLauncher_OnNotify(object sender, GenericEventArgs e)
    {
        FileLogger.Log(e.Argument.ToString());
    }
}

public class FileLogger
{
    public static void Log(string Message, bool AddDate = true)
    {
        var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "AstarMgr_WS.log.txt");
        if (!File.Exists(filePath)) File.WriteAllText(filePath, string.Empty);

        File.AppendAllText(filePath, Environment.NewLine);
        File.AppendAllText(filePath, $"{(AddDate ? DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss:fff") : string.Empty)} {Message}".Trim());
    }
}
