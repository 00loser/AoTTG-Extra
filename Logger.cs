using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

public class Logger : MonoBehaviour
{
    private void Start() => UpdateGUI();
    
    public static void AddLog(string log)
    {
        Logs.Add(log);
        UpdateGUI();
    }
    private static void UpdateGUI()
    {
        ToDraw = string.Empty;
        StringBuilder builder = new StringBuilder();
        if (Logs.Count < 13)
        {
            for (int i = 0; i < Logger.Logs.Count; i++)
            {
                builder.AppendLine(Logger.Logs[i]);
            }
        }
        else
        {
            for (int j = Logs.Count - 13; j < Logger.Logs.Count; j++)
            {
                builder.AppendLine(Logs[j]);
            }
        }
        ToDraw = builder.ToString();
    }
    public void OnGUI()
    {
        if (!Visible)
        {
            return;
        }
        GUI.Box(ConsoleRect, string.Empty);
        GUI.Label(ConsoleRect, ToDraw);
    }
    public void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            Visible = !Visible;
        }
    }
 
    public static List<string> Logs = new List<string>();
    private Rect ConsoleRect = new Rect((float)Screen.width - 410f, (float)Screen.height - 220f, 400f, 210f);
    private static string ToDraw;
    private bool Visible;
}