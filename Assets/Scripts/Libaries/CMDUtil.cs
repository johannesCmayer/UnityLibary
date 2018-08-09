using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Diagnostics;

public static class CMDUtil 
{
    public static string anacondaStartEnvMLAgents_Command = @"%userprofile%\AppData\Local\Continuum\anaconda3\Scripts\activate.bat " +
                                                @"%userprofile%\AppData\Local\Continuum\anaconda3\envs\ml-agents";
    public static string Internal_anacondaStartEnvMLAgents_Command {
        get
        {
            return
            $@"{Application.dataPath}\..\anaconda3\Scripts\activate.bat {Application.dataPath}\..\anaconda3\envs\ml-agents";
        }
    }

    public static Process ExecuteInCMD(string args, bool autocloseConsole)
    {
        if (autocloseConsole)
            args = " /C " + args;
        else
            args = " /K" + args;
        return ProcessUtil.ExecuteFile("cmd.exe", args);
    }
}
