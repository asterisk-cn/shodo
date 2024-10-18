using UnityEngine;
using UnityEngine.InputSystem;

public class VirtualMouseModifier : MonoBehaviour
{
    [SerializeField] float _minSpeed;
    [SerializeField] float _maxSpeed;
    [SerializeField] float _defaultSpeed;
    
    private VirtualMouse _virtualMouse;
    private GetColor _getColor;

    private void Awake()
    {
        _virtualMouse = GetComponent<VirtualMouse>();
        _getColor = GetComponent<GetColor>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        Color color = _getColor.GetPixelColorFromImage(_virtualMouse.CursorTransform.position);
        UpdateSpeedWithColor(color);

        LockCursorInScreen();
    }

    private void UpdateSpeedWithColor(Color color)
    {
        float speed;
        if (IsColorTransparent(color))
        {
            speed = _defaultSpeed;
        }
        else
        {
            var hue = ColorToHue(color);
            speed = Mathf.Lerp(_minSpeed, _maxSpeed, hue);
        }
        _virtualMouse.CursorSpeed = speed;
    }

    private float ColorToHue(Color color)
    {
        Color.RGBToHSV(color, out var h, out var s, out var v);
        return h;
    }
    
    private bool IsColorTransparent(Color color)
    {
        return color.a < 0.1f;
    }

    private void LockCursorInScreen()
    {
        var screenPosition = _virtualMouse.CursorTransform.position;
        var clampedPosition = new Vector2(
            Mathf.Clamp(screenPosition.x, 0, Screen.width),
            Mathf.Clamp(screenPosition.y, 0, Screen.height)
        );

        _virtualMouse.CursorTransform.position = clampedPosition;
    }
}
