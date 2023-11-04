using UnityEngine;
using UnityEngine.UI;

public class DebugConsole : MonoBehaviour
{
    public Text debugText;

    private static DebugConsole instance;
    private static string log = "";

    private void Awake()
    {
        instance = this;
    }

    public static void Log(string message)
    {
        log = message + "\n" + log;
        instance.debugText.text = log;
    }

    public static void Clear()
    {
        log = "";
        instance.debugText.text = log;
    }
}
