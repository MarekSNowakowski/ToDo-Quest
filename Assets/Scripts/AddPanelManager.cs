using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AddPanelManager : MonoBehaviour
{
    [Header("MainAddPanel")]
    [SerializeField]
    GameObject mainAddPanel;
    [SerializeField]
    TMP_InputField questNameFeild;
    [SerializeField]
    TMP_InputField rewardFeild;
    int weight = 0;
    [SerializeField]
    Image weightImage;
    [SerializeField]
    Image categoryIcon;
    Category activeCategory;
    [SerializeField]
    Sprite bookmarIcon;
    Sprite bookmarkEmpty;

    [Header("CommentPanel")]
    [SerializeField]
    GameObject commentPanel;
    [SerializeField]
    TMP_InputField commentFeild;
    private string comment;

    [Header("Other")]
    [SerializeField]
    QuestManager questManager;
    [SerializeField]
    AddPanelView addPanelView;
    string editingID;

    [Header("CategoryPanel")]
    [SerializeField]
    CategoryManager categoryManager;
    [SerializeField]
    GameObject categoryPanel;
    [SerializeField]
    TMP_InputField categoryInputField;
    [SerializeField]
    Image categoryCreationIcon;
    CategoryColor categoryColor = null;
    

    public void Submit()
    {
        if (questNameFeild.text == "") Debug.Log("Can't create quest without a name");
        else {
            QuestData questData = new QuestData();
            questData.questName = questNameFeild.text;
            if (rewardFeild.text != "") questData.reward = rewardFeild.text;
            if (comment != "") questData.comment = comment;
            questData.weight = weight;
            if (activeCategory != null)
            {
                questData.category = activeCategory;
            }

            if (editingID!=null)
            {
                questManager.RemoveQuest(editingID);
            }
            questManager.AddQuest(questData);


            Close();
        }
    }

    private void Start()
    {
        bookmarkEmpty = categoryIcon.sprite;
        this.gameObject.SetActive(false); 
    }

    public void SubmitComment()
    {
        comment = commentFeild.text;
        addPanelView.CloseCommentPanel();
    }

    public void IncreaseWeight()
    {
        weight++;
        if (weight == 1) weightImage.color = new Color(0, 210, 0);
        else if (weight == 2) weightImage.color = new Color(0, 0, 210);
        else if (weight == 3) weightImage.color = new Color(210, 0, 0);
        else if (weight == 4 || weight == 0)
        {
            weight = 0;
            weightImage.color = new Color(255, 255, 255);
        }
    }

    public void ResetWeight()
    {
        weight = 0;
        weightImage.color = new Color(255, 255, 255);
    }

    public void EditQuest(QuestData questData)
    {
        questNameFeild.text = questData.questName;
        rewardFeild.text = questData.reward;
        weight = questData.weight - 1;
        IncreaseWeight();
        commentFeild.text = questData.comment;
        editingID = questData.ID;
    }

    public void Close()
    {
        if (mainAddPanel.activeInHierarchy || commentPanel.activeInHierarchy)
        {
            Clear();
        }
        if (categoryPanel.activeInHierarchy)
        {
            CloseCategory();
        }

        addPanelView.Close();
    }

    void Clear()
    {
        ResetWeight();
        questNameFeild.text = "";
        rewardFeild.text = "";
        editingID = null;
        comment = "";
        commentFeild.text = "";
        activeCategory = null;
        categoryIcon.color = Color.white;
        categoryIcon.sprite = bookmarkEmpty;
    }

    public void OpenCategory()
    {
        commentPanel.SetActive(false);
        mainAddPanel.SetActive(false);
        categoryPanel.SetActive(true);
    }

    public void CloseCategory()
    {
        categoryPanel.SetActive(false);
        categoryInputField.text = "";
    }

    public void SubmitCategory()
    {
        string name = categoryInputField.text;
        if(categoryColor!=null)
        {
            if(name!=null && name!="")
            {
                categoryManager.AddCategory(name, categoryColor.GetColor());
                categoryColor.Block();
                CloseCategory();
            }
            else
            {
                Debug.LogWarning("Nie można utworzyć kategorii z powodu braku nazwy!");
            }
        }
        else
        {
            if (!categoryManager.CheckColor(Color.white))
            {
                if (name != null && name != "")
                {
                    categoryManager.AddCategory(name, Color.white);
                    CloseCategory();
                }
                else
                {
                    Debug.LogWarning("Nie można utworzyć kategorii z powodu braku wybranego koloru!");
                }
            }
            else
            {
                Debug.LogWarning("Nie można utworzyć kategorii z powodu braku nazwy!");
            }
        }
    }

    public void OnColorChoose(CategoryColor categoryColor)
    {
        this.categoryColor = categoryColor;
        categoryCreationIcon.color = categoryColor.GetColor();
    }

    public CategoryManager GetCategoryManager()
    {
        return categoryManager;
    }

    public void ChooseCategory(Category category)
    {
        categoryIcon.sprite = bookmarIcon;
        categoryIcon.color = category.GetColor();
        activeCategory = category;
    }

    public void SubmitDate()
    {

    }
}
