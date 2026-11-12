
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAim : MonoBehaviour
{
    [Header("References")]
    public Transform aimPivot;
    public Camera mainCamera;

    [Header("Gamepad Settings")]
    public float deadZone = 0.2f;

    // Guarda a direção do analógico direito.
    private Vector2 gamepadAim;

    private void Awake()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    private void Update()
    {
        if (aimPivot == null || mainCamera == null)
            return;

        Vector2 aimDirection;

        // Se o jogador estiver movimentando
        // o analógico direito, utiliza o controle.
        if (gamepadAim.magnitude > deadZone)
        {
            aimDirection = gamepadAim;
        }
        else
        {
            // Caso contrário, utiliza o mouse.
            if (Mouse.current == null)
                return;

            Vector2 mousePosition =
                Mouse.current.position.ReadValue();

            Vector3 mouseWorldPosition =
                mainCamera.ScreenToWorldPoint(
                    new Vector3(
                        mousePosition.x,
                        mousePosition.y,
                        -mainCamera.transform.position.z
                    )
                );

            aimDirection =
                mouseWorldPosition - aimPivot.position;
        }

        float angle = Mathf.Atan2(
            aimDirection.y,
            aimDirection.x
        ) * Mathf.Rad2Deg;

        aimPivot.rotation =
            Quaternion.Euler(0f, 0f, angle);

        // Corrige a orientação visual da arma.
        aimPivot.localScale = new Vector3(
            1f,
            aimDirection.x < 0f ? -1f : 1f,
            1f
        );
    }

    // Recebe a direção do analógico direito.
    public void Aim(InputAction.CallbackContext context)
    {
        gamepadAim = context.ReadValue<Vector2>();
    }
}
