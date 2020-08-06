using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawBridge : MonoBehaviour
{
    [SerializeField] private Vector3 targetRot;
    private bool Activated = false;
    private int Triggers = 0;
    
    public void Trigger()
    {
        Triggers++;

        if (Activated == false && Triggers >= 3)
        {
            StartCoroutine(FallRoutine());
            Activated = true;
        }
    }

    public void Untrigger()
    {
        Triggers--;
    }

    private IEnumerator FallRoutine()
    {
        Quaternion targetQ = Quaternion.Euler(targetRot);
        Quaternion startingRot = transform.rotation;

        float progress = 0;
        while (progress < 1)
        {
            progress += Time.deltaTime / 2;
            transform.rotation = Quaternion.Lerp(startingRot, targetQ, progress);
            yield return null;
        }

        Destroy(this);
    }
}
