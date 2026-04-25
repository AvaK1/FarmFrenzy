using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static Crop;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public int playerHealth = 100;

    public InputAction moveInput;
    private Vector2 movementDirection = Vector2.zero;
    public float moveSpeed = 1.0f;

    public event Action<Vector2> OnMove; //change this to be InputSystem_Actions
    private Rigidbody2D rbody;
    [SerializeField] private GameObject deadPlayerPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveInput.Enable();
        moveInput.performed += GetMoveVector;
        moveInput.canceled += GetMoveVector;
        rbody = GetComponent<Rigidbody2D>();
    }

    public void TakeDamage(int damage)
    {
        playerHealth -= damage;
        Debug.Log("Player health: " + playerHealth);
        GameUIManager.Instance.UpdatePestsAndHealth();
        if (playerHealth <= 0)
        {
            //death logic
            Die();
        }
    }

    private void Die()
    {
        GameUIManager.Instance.DisplayGameOver();
        Instantiate(deadPlayerPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Crop"))
        {
            Crop crop = collision.GetComponent<Crop>();
            if (crop.currentCropState == CropState.Harvestable)
            {
                crop.Harvest();
            }
        }

        if (collision.CompareTag("WeaponBox"))
        {
            GameUIManager.Instance.OpenOrCloseWeaponBox();
            Destroy(collision.gameObject);
        }
    }

    #region movement
    public void GetMoveVector(InputAction.CallbackContext context)
    {
        movementDirection = context.ReadValue<Vector2>();
        OnMove?.Invoke(movementDirection);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rbody.linearVelocity = movementDirection.normalized * moveSpeed * Time.deltaTime;
    }
    #endregion
}
