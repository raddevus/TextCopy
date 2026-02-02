using System.Threading;

#pragma warning disable IDE0021
#if (NETSTANDARD2_1 || NET5_0_OR_GREATER)

static class LinuxClipboard
{
    static bool isWsl;

    static string clipCmd = string.Empty;

    static void SetClipCmd() =>
        Console.WriteLine("it's me 2.1");
//        Console.WriteLine($"this is me 2.1 - {BashRunner.Run("xsel")}");
    static LinuxClipboard()  
    {        isWsl = Environment.GetEnvironmentVariable("WSL_DISTRO_NAME") != null;
        SetClipCmd();
    }

    public static async Task SetTextAsync(string text, CancellationToken cancellation)
    {
        var tempFileName = Path.GetTempFileName();
        await File.WriteAllTextAsync(tempFileName, text, cancellation);

        if (cancellation.IsCancellationRequested)
        {
            return;
        }

        InnerSetText(tempFileName);
    }

    public static void SetText(string text)
    {
        var tempFileName = Path.GetTempFileName();
        File.WriteAllText(tempFileName, text);
        InnerSetText(tempFileName);
    }

    static void InnerSetText(string tempFileName)
    {
        try
        {
            if (isWsl)
            {
                BashRunner.Run($"cat {tempFileName} | clip.exe ");
            }
            else
            {
                try{
                    Console.WriteLine("Huh! It's 2.1");
                BashRunner.Run($"cat {tempFileName} | xsel -i --clipboard ");
                }
                catch(Exception ex){Console.WriteLine($"{ex.Message}");}
                BashRunner.Run($"cat {tempFileName} | xclip -sel c");
            }
        }
        finally
        {
            File.Delete(tempFileName);
        }
    }

    public static string? GetText()
    {
        var tempFileName = Path.GetTempFileName();
        try
        {
            InnerGetText(tempFileName);
            return File.ReadAllText(tempFileName);
        }
        finally
        {
            File.Delete(tempFileName);
        }
    }

    public static async Task<string?> GetTextAsync(CancellationToken cancellation)
    {
        var tempFileName = Path.GetTempFileName();
        try
        {
            InnerGetText(tempFileName);
            return await File.ReadAllTextAsync(tempFileName, cancellation);
        }
        finally
        {
            File.Delete(tempFileName);
        }
    }

    static void InnerGetText(string tempFileName)
    {
        if (isWsl)
        {
            BashRunner.Run($"powershell.exe -NoProfile Get-Clipboard  > {tempFileName}");
        }
        else
        {
            BashRunner.Run($"xsel -o --clipboard  > {tempFileName}");
        }
    }
}
#endif
#pragma warning restore IDE0021


