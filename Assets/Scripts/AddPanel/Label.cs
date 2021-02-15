using UnityEngine;

public abstract class Label : MonoBehaviour
{
    protected string labelID;
    public int questsInside;
    protected float labelHeightRatio = 0.03f;

    private void Start()
    {
        GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, labelHeightRatio * Screen.height);
    }

    virtual public void QuestAdded()
    {
        questsInside++;
    }

    public void QuestRemoved()
    {
        questsInside--;
    }

    public int GetNumberOfQuestsInside()
    {
        return questsInside;
    }

    public string GetID()
    {
        return labelID;
    }
}

