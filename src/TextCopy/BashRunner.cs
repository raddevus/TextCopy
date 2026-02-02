#if (NETSTANDARD || NETFRAMEWORK || NET5_0_OR_GREATER)

static class BashRunner
{

    public static bool FileExists(string fileName)
    {
        var paths = (Environment.GetEnvironmentVariable("PATH") ?? "").Split(':');
        foreach (var path in paths)
        {
            var full = System.IO.Path.Combine(path, fileName);
            if (System.IO.File.Exists(full))
                return true;
        }
        return false;
    }

    public static bool CommandExists(string cmd)
    {
        Console.WriteLine($"Checking cmd : {cmd}");
        var p = new Process();
        p.StartInfo.FileName = "/bin/bash";
        p.StartInfo.Arguments = $"-c \"command -v {cmd}\"";
        p.StartInfo.RedirectStandardOutput = true;
        p.StartInfo.RedirectStandardError = true;
        p.StartInfo.UseShellExecute = false;

        p.Start();
        p.WaitForExit(3);

        return p.ExitCode == 0;
    }

    public static string Run(string commandLine)
    {
        StringBuilder errorBuilder = new();
        StringBuilder outputBuilder = new();
        var arguments = $"-c \"{commandLine}\"";
        using Process process = new()
        {
            StartInfo = new()
            {
                FileName = "bash",
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            }
        };
        process.Start();
        process.OutputDataReceived += (_, args) => { outputBuilder.AppendLine(args.Data); };
        process.BeginOutputReadLine();
        process.ErrorDataReceived += (_, args) => { errorBuilder.AppendLine(args.Data); };
        process.BeginErrorReadLine();
        if (!process.WaitForExit(500))
        {
            var timeoutError = $@"Process timed out. Command line: bash {arguments}.
Output: {outputBuilder}
Error: {errorBuilder}";
            throw new(timeoutError);
        }
        if (process.ExitCode == 0)
        {
            return outputBuilder.ToString();
        }

        var error = $@"Could not execute process. Command line: bash {arguments}.
Output: {outputBuilder}
Error: {errorBuilder}";
       return error;
       throw new(error);
    }

}
#endif
