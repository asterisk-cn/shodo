using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class VirtualMouse : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private float _cursorSpeed = 300f;
    
    private Vector3 _prevMousePosition;
    
    public Transform CursorTransform => _image.transform;
    public float CursorSpeed { get => _cursorSpeed; set => _cursorSpeed = value; }

    private void Awake()
    {
        _prevMousePosition = Input.mousePosition;
    }

    private void OnMove(InputValue value)
    {
        var delta = value.Get<Vector2>() * CursorSpeed * Time.deltaTime;
        _image.transform.position += new Vector3(delta.x, delta.y, 0);
    }
}