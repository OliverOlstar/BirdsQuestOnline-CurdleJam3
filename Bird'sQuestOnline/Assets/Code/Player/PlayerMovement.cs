using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerMovement : INotMine
{
    public UnityEvent Jumped;

    [SerializeField] private float jumpForce = 5;
    [SerializeField] private float jumpAirForce = 3;
    [SerializeField] private float moveSpeed = 30;
    [SerializeField] private float moveAirSpeed = 10;
    [SerializeField] private float sprintSpeedMult = 2;
    private float speed = 0;

    [SerializeField] private Transform camera;
    [SerializeField] private Transform model;
    private Rigidbody rb;
    [SerializeField] private StaminaUI staminaUI;
    private OnGround onGround;

    private Vector2 moveInput = new Vector2(0, 0);
    private bool grounded = true;
    private bool sprinting = false;

    void Start()
    {
        onGround = GetComponent<OnGround>();
        rb = GetComponent<Rigidbody>();
        speed = moveSpeed;
    }

    private void Update()
    {
        if (moveInput != Vector2.zero)
        {
            Vector3 moveDir = new Vector3(moveInput.x, 0, moveInput.y);
            moveDir = camera.TransformDirection(moveDir);
            moveDir.y = 0;

            float mult = (Vector3.Dot(moveDir, model.forward) + 1) / 5 + 1;

            if (moveDir != Vector3.zero)
            {
                if (sprinting && stamina > 0)
                {
                    rb.AddForce(moveDir.normalized * Time.deltaTime * speed * sprintSpeedMult * mult, ForceMode.VelocityChange);
                    stamina -= Time.deltaTime;
                }
                else
                {
                    rb.AddForce(moveDir.normalized * Time.deltaTime * speed * mult, ForceMode.VelocityChange);
                }
            }
        }
    }

    public void OnMove(InputAction.CallbackContext ctx) => moveInput = ctx.ReadValue<Vector2>().normalized;

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (grounded)
            {
                rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
                Jumped.Invoke();
            }
            else if (stamina >= 1)
            {
                rb.velocity = new Vector3(rb.velocity.x, jumpAirForce, rb.velocity.z);
                stamina -= 1;
                Jumped.Invoke();
            }
        }
    }

    public void OnSprint(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            sprinting = true;
        }
        else if (ctx.canceled)
        {
            sprinting = false;
        }
    }

    public void OnGround()
    {
        grounded = true;
        if (onGround.OnIce == false)
        {
            rb.drag = 4;
            speed = moveSpeed;
        }
        else
        {
            rb.drag = 0.2f;
            speed = moveAirSpeed;
        }
    }

    public void OffGround()
    {
        grounded = false;
        rb.drag = 1;
        speed = moveAirSpeed;
    }

    #region Stamina
    private int _collectables = 0;
    public int collectables
    {
        get
        {
            return _collectables;
        }

        set
        {
            _collectables = value;
            stamina = _collectables;
            staminaUI.IncreaseMax(_collectables);
        }
    }

    private float _stamina = 0;
    private Coroutine staminaRoutine;
    public float stamina
    {
        get
        {
            return _stamina;
        }

        set
        {
            if (_stamina > value)
            {
                if (staminaRoutine != null)
                    StopCoroutine(staminaRoutine);
                staminaRoutine = StartCoroutine(staminaRegenRoutine());
            }

            _stamina = value;
            staminaUI.UpdateStamina(_stamina);
        }
    }

    private IEnumerator staminaRegenRoutine()
    {
        yield return new WaitForSeconds(1);

        while (stamina < collectables)
        {
            stamina += Time.deltaTime;
            yield return null;
        }

        stamina = collectables;
    }
    #endregion
}
