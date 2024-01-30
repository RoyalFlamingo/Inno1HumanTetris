using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CenterMessageScript : MonoBehaviour
{
    public void ShowText(string text, float duration)
    {
        GetComponentInChildren<TextMeshProUGUI>().text = text;

        if (duration > 0)
            StartCoroutine(Destory(duration));
    }

    private IEnumerator Destory(float destroyAfterTime)
    {
        yield return new WaitForSeconds(destroyAfterTime);
    }
}
