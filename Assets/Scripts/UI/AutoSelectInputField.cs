using UnityEngine;
using TMPro;
using System.Collections;

[RequireComponent(typeof(TMP_InputField))]
public class AutoSelectInputField : MonoBehaviour
{
    TMP_InputField inputField;

    void OnEnable()
    {
        inputField = GetComponent<TMP_InputField>();
        StartCoroutine(CheckIfInputFieldSelected());
    }

    IEnumerator CheckIfInputFieldSelected()
    {
        yield return new WaitForSeconds(0.3f);
        inputField.ActivateInputField();
        inputField.Select();
    }
}
