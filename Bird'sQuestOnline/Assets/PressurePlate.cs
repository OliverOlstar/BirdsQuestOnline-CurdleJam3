using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] private int requiredType = 0;
    [SerializeField] private bool playerCanPress = true;

    public UnityEvent Pressed;
    public UnityEvent Released;

    private int pressure = 0;
    private bool triggered = false;

    private List<Collider> others = new List<Collider>();

    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.layer == 9 && other.GetComponent<IPeckable>().Type() == requiredType) || (other.CompareTag("Player") && playerCanPress))
        {
            pressure++;

            if (other.gameObject.layer == 9)
                others.Add(other);

            if (triggered == false)
            {
                triggered = true;
                Pressed.Invoke();

                StopAllCoroutines();
                StartCoroutine(MoveChild(0.229f));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((other.gameObject.layer == 9 && other.GetComponent<IPeckable>().Type() == requiredType) || (other.CompareTag("Player") && playerCanPress))
        {
            pressure--;

            if (other.gameObject.layer == 9)
                others.Remove(other);

            if (pressure <= 0)
            {
                pressure = 0;
                Released.Invoke();
                triggered = false;

                StopAllCoroutines();
                StartCoroutine(MoveChild(0.155f));
            }
        }
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < others.Count; i++)
        {
            if (others[i].enabled == false)
            {
                pressure--;
                others.RemoveAt(i);

                if (pressure <= 0)
                {
                    pressure = 0;
                    Released.Invoke();
                    triggered = false;

                    StopAllCoroutines();
                    StartCoroutine(MoveChild(0.155f));
                }
            }
        }
    }

    private IEnumerator MoveChild(float pHeight)
    {
        float progress = 0;
        Vector3 pos = transform.GetChild(0).localPosition;
        float startHeight = pos.y;

        while (progress < 1)
        {
            progress += Time.deltaTime * 2;
            transform.GetChild(0).localPosition = new Vector3(pos.x, Mathf.Lerp(startHeight, pHeight, progress), pos.z);
            yield return null;
        }
    }
}
