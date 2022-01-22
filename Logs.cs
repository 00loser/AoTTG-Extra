//A "extension" for Logger.cs
using System;
public static class Logs
{
	public static void LogInfo(string log) => Log(log, "Info", "#7C7C7C");
	public static void LogError(string log) => Log(log, "Error", "red");
	public static void LogWarn(string log) => Log(log, "Warn", "yellow");
	public static void LogAntis(string log) => Log(log, "Antis", "#39FF84");
	
	private static void Log(string log, string type, string color)
	{
		Logger.AddLog(DateTime.Now.ToString("hh:mm") + $" <color={color}>[{type}]</color> {log}");
	}
}
