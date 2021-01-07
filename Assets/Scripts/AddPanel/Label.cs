using UnityEngine;

public abstract class Label : MonoBehaviour
{
    protected string labelID;
    protected int questsInside;

    public void QuestAdded()
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

