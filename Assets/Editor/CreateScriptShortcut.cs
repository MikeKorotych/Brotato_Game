using UnityEngine;
using UnityEditor;
using System.IO;

public class CreateScriptShortcut : MonoBehaviour
{
    // Устанавливаем сочетание клавиш на Ctrl + Shift + M
    [MenuItem("Assets/Create New C# Script %#m")] // Ctrl + Shift + M
    static void CreateNewCSharpScript()
    {
        // Определяем путь для нового скрипта в выбранной папке
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

        // Указываем имя для нового скрипта
        string scriptName = "NewScript.cs";
        string scriptPath = folderPath + scriptName;

        // Проверяем, если файл уже существует, и изменяем имя
        int counter = 1;
        while (File.Exists(scriptPath))
        {
            scriptName = $"NewScript{counter}.cs";
            scriptPath = folderPath + scriptName;
            counter++;
        }

        // Шаблон кода для нового скрипта
        string scriptTemplate =
@"using UnityEngine;

public class NewScript : MonoBehaviour
{
    // Start вызывается перед первым кадром
    void Start()
    {
        
    }

    // Update вызывается каждый кадр
    void Update()
    {
        
    }
}";

        // Создание нового C# скрипта
        File.WriteAllText(scriptPath, scriptTemplate);

        // Обновляем базу данных Unity
        AssetDatabase.Refresh();

        // Выделяем новый созданный скрипт в проекте
        UnityEngine.Object newScript = AssetDatabase.LoadAssetAtPath(scriptPath, typeof(MonoScript));
        Selection.activeObject = newScript;

        Debug.Log("Новый скрипт создан: " + scriptPath);
    }
}
