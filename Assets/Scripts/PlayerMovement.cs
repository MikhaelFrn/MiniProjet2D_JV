using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float vitesse = 5f;

    private Rigidbody2D corps;
    private Vector2 direction;

    private void Awake()
    {
        corps = GetComponent<Rigidbody2D>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    private void Update()
    {
        LireClavier();
    }

    private void FixedUpdate()
    {
        corps.linearVelocity = direction * vitesse;
    }

    private void LireClavier()
    {
        if (Keyboard.current == null)
        {
            direction = Vector2.zero;
            return;
        }

        float horizontal = 0f;
        float vertical = 0f;

        if (Keyboard.current.leftArrowKey.isPressed || Keyboard.current.aKey.isPressed)
            horizontal = -1f;

        if (Keyboard.current.rightArrowKey.isPressed || Keyboard.current.dKey.isPressed)
            horizontal = 1f;

        if (Keyboard.current.downArrowKey.isPressed || Keyboard.current.sKey.isPressed)
            vertical = -1f;

        if (Keyboard.current.upArrowKey.isPressed || Keyboard.current.wKey.isPressed)
            vertical = 1f;

        direction = new Vector2(horizontal, vertical).normalized;
    }

}
