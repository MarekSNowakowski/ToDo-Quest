using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubQuestDisplayer : MonoBehaviour
{
    [SerializeField]
    GameObject subQuestObject;

    List<SubQuest> subQuestObjects = new List<SubQuest>();
    protected float subQuestHeightRatio = 0.045f;
    int questCount;

    public void LoadSubQuests(List<SubQuestData> subQuests)
    {
        foreach(SubQuestData data in subQuests)
        {
            GameObject ob = Instantiate(subQuestObject);
            ob.transform.SetParent(transform);
            SubQuest subQuest = ob.GetComponent<SubQuest>();
            subQuest.Set(data);
            subQuestObjects.Add(subQuest);
        }
    }

    public void Unload()
    {
        foreach(SubQuest subQuest in subQuestObjects)
        {
            Destroy(subQuest.gameObject);
        }
        subQuestObjects.Clear();
    }

    private void Start()
    {
        CountChildren();

        RectTransform myRectTransform = gameObject.GetComponent<RectTransform>();

        float questHeight = subQuestHeightRatio * Screen.height;

        var height = questCount * questHeight;

        myRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        myRectTransform.anchoredPosition = new Vector3(0, -0.5f * height);
    }

    public virtual void CountChildren()
    {
        questCount = 0;

        foreach (Transform child in transform)
        {
            if (child.tag == "SubQuest") questCount++;
        }
    }
}
