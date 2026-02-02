using System.Threading;
#if (NETSTANDARD2_0 || NETFRAMEWORK)

static class LinuxClipboard
{
    static bool isWsl;
    static string clipCmd = string.Empty;

    static void SetClipCmd(){
        Console.WriteLine("it's me!");
//        Console.WriteLine($"this is me 2.0 - {BashRunner.Run("xsel")}");
    }

    static LinuxClipboard()
    {
        isWsl = Environment.GetEnvironmentVariable("WSL_DISTRO_NAME") != null;
        SetClipCmd();
    }

    public static Task SetTextAsync(string text, CancellationToken cancellation)
    {
        SetText(text);

        return Task.CompletedTask;
    }

    public static void SetText(string text)
    {
        var tempFileName = Path.GetTempFileName();
        File.WriteAllText(tempFileName, text);
        try
        {
            if (isWsl)
            {
                BashRunner.Run($"cat {tempFileName} | clip.exe ");
            }
            else
            {
                Console.WriteLine("it's 2.0!");
                if (BashRunner.FileExists("xsel")){
                BashRunner.Run($"cat {tempFileName} | xsel -i --clipboard ");
                }
                else{
                    BashRunner.Run($"cat {tempFileName} | xclip -sel c ");
                }
            }
        }
        finally
        {
            File.Delete(tempFileName);
        }
    }

    public static Task<string?> GetTextAsync(Cancellation cancellation)
    {
        return Task.FromResult<string?>(GetText());
    }

    public static string GetText()
    {
        var tempFileName = Path.GetTempFileName();
        try
        {
            if (isWsl)
            {
                BashRunner.Run($"powershell.exe -NoProfile Get-Clipboard  > {tempFileName}");
            }
            else
            {
                BashRunner.Run($"xsel -o --clipboard  > {tempFileName}");
            }
            return File.ReadAllText(tempFileName);
        }
        finally
        {
            File.Delete(tempFileName);
        }
    }
}
#endif
