using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Diagnostics;
using System.Threading;

public static class ProcessUtil
{
    public static Process ExecuteFile(string fileLocation, string args, ProcessWindowStyle windowStyle = ProcessWindowStyle.Normal)
    {
        Process proc = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = fileLocation,
                Arguments = "\"" + args + "\"",
                UseShellExecute = true,
                WindowStyle = windowStyle,
            }
        };
        proc.Start();
        return proc;
    }
}
