using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

/// <summary>
/// 挂载到姓名 InputField 上，实时过滤输入：
/// - 只允许英文字母和空格
/// - 自动将连续多个空格压缩为单个空格
/// - 自动去除首尾空格
/// </summary>
public class NameInputFilter : MonoBehaviour
{
    private InputField inputField;

    void Start()
    {
        inputField = GetComponent<InputField>();
        if (inputField != null)
        {
            inputField.onValueChanged.AddListener(OnInputChanged);
        }
    }

    private void OnInputChanged(string value)
    {
        if (string.IsNullOrEmpty(value))
            return;

        // 1. 只保留英文字母和空格
        string filtered = Regex.Replace(value, @"[^A-Za-z ]", "");

        // 2. 压缩连续多个空格为单个空格
        filtered = Regex.Replace(filtered, @"\s+", " ");

        // 3. 去除首尾空格
        filtered = filtered.Trim();

        // 4. 更新输入框（如果变化）
        if (inputField.text != filtered)
        {
            int cursorPos = inputField.caretPosition;
            inputField.text = filtered;
            inputField.caretPosition = Mathf.Min(cursorPos, filtered.Length);
        }
    }

    void OnDestroy()
    {
        if (inputField != null)
            inputField.onValueChanged.RemoveListener(OnInputChanged);
    }
}