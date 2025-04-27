using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace KrishnamohanYagneswaran.MiniLogViewerTool
{
    public class MiniLogViewer : EditorWindow
    {
        private List<string> logEntries = new List<string>();
        private string filter = "All";
        private bool showLogs = true;
        private Vector2 scrollPosition;

        private const string infoTag = "Info";
        private const string warningTag = "Warning";
        private const string errorTag = "Error";

        private const float windowWidth = 300f;
        private const float windowHeight = 200f;

        [MenuItem("Window/Mini Log Viewer")]
        public static void ShowWindow()
        {
            MiniLogViewer window = GetWindow<MiniLogViewer>("Mini Log Viewer");
            window.minSize = new Vector2(windowWidth, windowHeight);
            window.maxSize = new Vector2(windowWidth, windowHeight);
        }

        // Ensure this method is not duplicated
        private void OnEnable()
        {
            Application.logMessageReceived += HandleLog;
        }

        // Ensure this method is not duplicated
        private void OnDisable()
        {
            Application.logMessageReceived -= HandleLog;
        }

        // Ensure this method is not duplicated
        private void HandleLog(string logString, string stackTrace, LogType type)
        {
            string logMessage = $"{System.DateTime.Now:HH:mm:ss} - {type}: {logString}";
            if (filter == "All" || filter == type.ToString())
            {
                logEntries.Add(logMessage);
            }

            if (logEntries.Count > 200)
            {
                logEntries.RemoveAt(0);
            }

            Repaint();
        }

        // Ensure this method is not duplicated
        private void OnGUI()
        {
            GUILayout.Label("Mini Log Viewer", EditorStyles.boldLabel);

            if (GUILayout.Button(showLogs ? "Hide Logs" : "Show Logs"))
            {
                showLogs = !showLogs;
            }

            int filterIndex = EditorGUILayout.Popup("Filter by Log Type", GetFilterIndex(), new string[] { "All", "Info", "Warning", "Error" });
            filter = GetFilterFromIndex(filterIndex);

            if (showLogs)
            {
                scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Height(position.height - 70));

                foreach (var log in logEntries)
                {
                    DisplayLogEntry(log);
                }

                GUILayout.EndScrollView();

                if (GUILayout.Button("Clear Logs"))
                {
                    logEntries.Clear();
                }
            }
        }

        private string GetFilterFromIndex(int index)
        {
            switch (index)
            {
                case 1: return infoTag;
                case 2: return warningTag;
                case 3: return errorTag;
                default: return "All";
            }
        }

        private int GetFilterIndex()
        {
            switch (filter)
            {
                case infoTag: return 1;
                case warningTag: return 2;
                case errorTag: return 3;
                default: return 0;
            }
        }

        private void DisplayLogEntry(string log)
        {
            if (log.Contains("Info"))
            {
                GUI.color = Color.white;
            }
            else if (log.Contains("Warning"))
            {
                GUI.color = Color.yellow;
            }
            else if (log.Contains("Error"))
            {
                GUI.color = Color.red;
            }

            GUILayout.Label(log);
            GUI.color = Color.white;
        }
    }
}
