using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBar : MonoBehaviour
{
    [SerializeField]
    float menuWidthRatio;
    [SerializeField]
    float slideSpeed;
    [SerializeField]
    GameObject blocker;
    [Header("Buttons")]
    [SerializeField]
    GameObject addButton;
    [SerializeField]
    GameObject backButton;
    [Header("Panels")]
    [SerializeField]
    GameObject settingPanel;
    [SerializeField]
    GameObject aboutPanel;
    [SerializeField]
    GameObject helpPanel;

    float width;
    int currentPosition;
    bool opened = false;

    RectTransform myRectTransform;

    private void Start()
    {
        myRectTransform = GetComponent<RectTransform>();
        width = Screen.width * menuWidthRatio;
        myRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        transform.position = new Vector2(-width / 2, transform.position.y);
    }

    public void OnMenuButtonPress()
    {
        StopAllCoroutines();
        currentPosition = (int)transform.position.x;
        blocker.SetActive(true);
        StartCoroutine(MenuOpeningCorutine());
    }

    public void OnBlockerPress()
    {
        StopAllCoroutines();
        currentPosition = (int)transform.position.x;
        blocker.SetActive(false);
        StartCoroutine(MenuClosingCorutine());
    }

    IEnumerator MenuOpeningCorutine()
    {
        opened = true;
        for( ; currentPosition <= width/2 ; currentPosition += (int)(slideSpeed*Screen.width))
        {
            yield return null;
            transform.position = new Vector2(currentPosition, transform.position.y);
        }
        currentPosition = (int) width / 2;
        transform.position = new Vector2(currentPosition, transform.position.y);
    }

    IEnumerator MenuClosingCorutine()
    {
        opened = false;
        for (; currentPosition >= -width / 2; currentPosition -= (int)(slideSpeed*Screen.width))
        {
            yield return null;
            transform.position = new Vector2(currentPosition, transform.position.y);
        }
        currentPosition = (int) -width / 2;
        transform.position = new Vector2(currentPosition, transform.position.y);
    }

    public void OnSettingsButtonPress()
    {
        OnPanelOpen();
        settingPanel.SetActive(true);
    }

    public void OnAboutButtonPress()
    {
        OnPanelOpen();
        aboutPanel.SetActive(true);
    }

    public void OnHelpButtonpress()
    {
        OnPanelOpen();
        helpPanel.SetActive(true);
    }

    public void OnPanelOpen()
    {
        ClosePanels();
        OnBlockerPress();
        backButton.SetActive(true);
        addButton.SetActive(false);
    }

    void ClosePanels()
    {
        if (settingPanel.activeInHierarchy)
        {
            settingPanel.SetActive(false);
        }
        if (aboutPanel.activeInHierarchy)
        {
            aboutPanel.SetActive(false);
        }
        if (helpPanel.activeInHierarchy)
        {
            helpPanel.SetActive(false);
        }
    }

    public void OnBackButtonPress()
    {
        ClosePanels();
        addButton.SetActive(true);
        backButton.SetActive(false);
        if (opened)
        {
            OnBlockerPress();
        }
    }
}
