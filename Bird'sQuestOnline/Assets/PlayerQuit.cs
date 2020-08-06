using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerQuit : MonoBehaviour
{
    private Text text;

    private void Start()
    {
        text = GetComponent<Text>();
    }

    public void OnQuit(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            StartCoroutine(QuitRoutine());
        }
        else if (ctx.canceled)
        {
            StopAllCoroutines();
            text.text = "";
        }
    }

    private IEnumerator QuitRoutine()
    {
        float time = 2;
        text.text = Mathf.Round(time * 100) / 100 + " - Hold to quit";
        yield return null;

        while (time > 0)
        {
            time -= Time.deltaTime;
            text.text = Mathf.Round(time * 100) / 100 + " - Hold to quit";
            yield return null;
        }

        Application.Quit();
    }
}
