using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

/// <summary>
/// 挂载到手机号 InputField 上，实时过滤输入：
/// - 只允许数字
/// </summary>
public class PhoneInputFilter : MonoBehaviour
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

        string filtered = Regex.Replace(value, @"[^0-9]", "");

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