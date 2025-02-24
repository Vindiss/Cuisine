using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    
    private Vector2 _moveVector;
    
    private bool _isMoving = true;
    private void OnEnable()
    {
        moveAction.action.performed += OnMove;
        moveAction.action.canceled += OnMoveCanceled;
        moveAction.action.Enable();
    }
    
    private void OnDisable()
    {
        moveAction.action.performed -= OnMove;
        moveAction.action.canceled -= OnMoveCanceled;
        moveAction.action.Disable();
    }

    private void OnMoveCanceled(InputAction.CallbackContext obj)
    {
       _moveVector = Vector2.zero;
    }

    private void OnMove(InputAction.CallbackContext obj)
    {
        _moveVector = obj.ReadValue<Vector2>();
    }

    public bool getIsMoving()
    {
        return _isMoving;
    }

    public void setIsMoving(bool value)
    {
        _isMoving = value;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isMoving)
        {
            Vector3 movement = new Vector3(0, 0, _moveVector.y) * (moveSpeed * Time.deltaTime);
            transform.Translate(movement, Space.Self);

            // Rotation gauche/droite en fonction de _moveVector.x
            float rotation = _moveVector.x * rotationSpeed *Time.deltaTime;
            transform.Rotate(0, rotation, 0);
        }
        
    }
}
