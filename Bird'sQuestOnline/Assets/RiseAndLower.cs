using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiseAndLower : MonoBehaviour
{
    [SerializeField] private Vector3 targetPos;
    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
    }

    public void Rise()
    {
        StopAllCoroutines();
        StartCoroutine(Move(targetPos));
    }

    public void Lower()
    {
        StopAllCoroutines();
        StartCoroutine(Move(startPos));
    }

    private IEnumerator Move(Vector3 pPos)
    {
        float progress = 0;
        Vector3 pos = transform.position;

        while (progress < 1)
        {
            progress += Time.deltaTime;
            transform.position = Vector3.Lerp(pos, pPos, progress);
            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(targetPos, 0.5f);
    }
}
