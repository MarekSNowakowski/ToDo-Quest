using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class BlockerFade : MonoBehaviour
{
    Image image;
    float blockedAlpha = 50;

    Image Image
    {
        get
        {
            if (!image)
            {
                image = GetComponent<Image>();
            }
            return image;
        }
    }

    private void OnEnable()
    {
        StartCoroutine(FadeInCo());
    }

    public void DisableBlocker(GameObject turnOff)
    {
        StartCoroutine(FadeOut(turnOff));
    }

    IEnumerator FadeInCo()
    {
        Image.color = new Color(0, 0, 0, 0);

        for(int alpha = 0; alpha < blockedAlpha; alpha+=2)
        {
            Image.color = new Color(0,0,0, (float) alpha / 255);
            yield return null;
        }

        Image.color = new Color(0, 0, 0, blockedAlpha / 255f);
    }

    IEnumerator FadeOut(GameObject turnOff)
    {
        Image.color = new Color(0, 0, 0, blockedAlpha / 255f);

        for (int alpha = (int)blockedAlpha; alpha > 0; alpha-=2)
        {
            Image.color = new Color(0, 0, 0, (float) alpha / 255);
            yield return null;
        }

        Image.color = new Color(0, 0, 0, 0);

        turnOff.SetActive(false);
    }
}
