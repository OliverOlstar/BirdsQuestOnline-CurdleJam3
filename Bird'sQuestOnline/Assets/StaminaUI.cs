using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaUI : MonoBehaviour
{
    private Image[] bars;

    void Awake()
    {
        bars = GetComponentsInChildren<Image>();
        foreach(Image bar in bars)
        {
            bar.gameObject.SetActive(false);
        }
    }

    public void UpdateStamina(float pValue)
    {
        for (int i = 0; i < bars.Length; i++)
        {
            if (i <= pValue)
            {
                bars[i].color = Color.white;
            }
            else
            {
                bars[i].color = Color.grey;
            }
        }
    }

    public void IncreaseMax(int pNewMax)
    {
        bars[pNewMax].gameObject.SetActive(true);
    }
}
