using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AskForReview : MonoBehaviour
{
    [SerializeField]
    GameObject AskForReviewPanel;
    [SerializeField]
    int runTimesTillAsk;
    [SerializeField]
    float secondsTillAsk;
    int currentRun;

    // Start is called before the first frame update
    void Start()
    {
        currentRun = PlayerPrefs.GetInt("run", 1);
        if(currentRun <= runTimesTillAsk)
        {
            PlayerPrefs.SetInt("run", currentRun+1);
            if(currentRun == runTimesTillAsk)
                StartCoroutine(AskForReviewCO());
        }
    }

    IEnumerator AskForReviewCO()
    {
        yield return new WaitForSeconds(secondsTillAsk);
        AskForReviewPanel.SetActive(true);
    }
}
