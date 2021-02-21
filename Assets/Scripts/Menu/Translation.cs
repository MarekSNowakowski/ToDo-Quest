using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class Translation : MonoBehaviour
{
    [SerializeField]
    int ID;
    [SerializeField]
    TranslationManager translationManager;

    private void Start()
    {
        GetComponent<TextMeshProUGUI>().text = translationManager.GetStaticString(ID);
    }
}
