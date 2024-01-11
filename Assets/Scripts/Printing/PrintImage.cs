using System.Diagnostics;
using UnityEngine;

public class PrintImage : MonoBehaviour
{
    public void Print(string filePath)
    {
        ProcessStartInfo info = new ProcessStartInfo()
        {
            Verb = "print",
            FileName = filePath,
            CreateNoWindow = true,
            WindowStyle = ProcessWindowStyle.Hidden
        };

        Process.Start(info);
    }

    // Fungsi untuk membuka Notepad
    public void Open()
    {
        Process.Start("notepad.exe");
    }
}
