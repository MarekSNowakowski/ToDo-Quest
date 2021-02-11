using UnityEngine;

public abstract class Tutorial : MonoBehaviour
{
    [SerializeField]
    protected readonly int tutorialStages;
    [SerializeField]
    protected int currentTutorialStage;

    public void Next()
    {
        currentTutorialStage++;
        OnPageChange();
    }

    public void Previous()
    {
        currentTutorialStage--;
        OnPageChange();
    }

    public abstract void OnPageChange();
}
