using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_InputField))]
public class AutoSelectInputField : MonoBehaviour
{
    TMP_InputField inputField;

    void OnEnable()
    {
        inputField = GetComponent<TMP_InputField>();
        inputField.ActivateInputField();
        inputField.Select();
    }
}
