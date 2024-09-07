using UnityEngine;
using UnityEditor;
using System.IO;

public class CreateScriptShortcut : MonoBehaviour
{
    // ������������� ��������� ������ �� Ctrl + Shift + M
    [MenuItem("Assets/Create New C# Script %#m")] // Ctrl + Shift + M
    static void CreateNewCSharpScript()
    {
        // ���������� ���� ��� ������ ������� � ��������� �����
        string folderPath = "Assets/";
        if (Selection.activeObject != null)
        {
            string selectedPath = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (Directory.Exists(selectedPath))
            {
                folderPath = selectedPath + "/";
            }
            else if (File.Exists(selectedPath))
            {
                folderPath = Path.GetDirectoryName(selectedPath) + "/";
            }
        }

        // ��������� ��� ��� ������ �������
        string scriptName = "NewScript.cs";
        string scriptPath = folderPath + scriptName;

        // ���������, ���� ���� ��� ����������, � �������� ���
        int counter = 1;
        while (File.Exists(scriptPath))
        {
            scriptName = $"NewScript{counter}.cs";
            scriptPath = folderPath + scriptName;
            counter++;
        }

        // ������ ���� ��� ������ �������
        string scriptTemplate =
@"using UnityEngine;

public class NewScript : MonoBehaviour
{
    // Start ���������� ����� ������ ������
    void Start()
    {
        
    }

    // Update ���������� ������ ����
    void Update()
    {
        
    }
}";

        // �������� ������ C# �������
        File.WriteAllText(scriptPath, scriptTemplate);

        // ��������� ���� ������ Unity
        AssetDatabase.Refresh();

        // �������� ����� ��������� ������ � �������
        UnityEngine.Object newScript = AssetDatabase.LoadAssetAtPath(scriptPath, typeof(MonoScript));
        Selection.activeObject = newScript;

        Debug.Log("����� ������ ������: " + scriptPath);
    }
}
