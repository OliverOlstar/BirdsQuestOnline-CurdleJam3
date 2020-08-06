using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerRespawn : MonoBehaviour
{
    private Image panel;
    [SerializeField] private PlayerAbilities abilities;
    private bool started = false;

    void Start()
    {
        panel = GetComponent<Image>();
    }

    public void Respawn()
    {
        if (started == false)
        {
            StopAllCoroutines();
            StartCoroutine(Fade());
        }
    }

    public void OnRespawn(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
            Respawn();
    }

    private IEnumerator Fade()
    {
        started = true;
        float progress = 0;
        while (progress < 1)
        {
            progress += Time.deltaTime;
            panel.color = new Color(panel.color.r, panel.color.g, panel.color.b, progress);
            yield return null;
        }

        ResetMe._instance.RespawnMe(transform.parent.parent);
        abilities.DropObject();
        yield return new WaitForSeconds(0.6f);

        progress = 0;
        while (progress < 1)
        {
            progress += Time.deltaTime;
            panel.color = new Color(panel.color.r, panel.color.g, panel.color.b, 1 - progress);
            yield return null;
        }

        started = false;
    }
}
