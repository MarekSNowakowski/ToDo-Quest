using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CategoryColor : MonoBehaviour
{
    [SerializeField]
    AddPanelManager addPanelManager;

    Color color;

    GameObject Blocker
    {
        get
        {
            return transform.GetChild(0).gameObject;
        }
    }

    Button button;

    private void Start()
    {
        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(OnColorChoose);
        color = gameObject.GetComponent<Image>().color;
        CheckIfLocked();
    }

    public void Block()
    {
        Blocker.SetActive(true);
        button.interactable = false;
    }

    public void UnBlock()
    {
        Blocker.SetActive(false);
        button.interactable = true;
    }

    public void CheckIfLocked()
    {
        if (addPanelManager.GetCategoryManager().CheckColor(color))
        {
            Block();
        }
        else
        {
            UnBlock();
        }
    }

    public void OnColorChoose()
    {
        addPanelManager.OnColorChoose(this);
    }

    public Color GetColor()
    {
        return color;
    }
}
