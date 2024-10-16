using UnityEngine;
using UnityEngine.UI;

public class GetColor : MonoBehaviour
{
    [SerializeField] private Image _targetImage; // クリックしたいオブジェクト
    private Texture2D _texture;

    [SerializeField] private Color _color; // 取得した色
    [SerializeField] private bool _isWhite;

    private void Awake()
    {
        if (_targetImage && _targetImage.sprite)
        {
            _texture = _targetImage.sprite.texture;
        }
        else
        {
            Debug.LogError("Target Image or Sprite is not assigned.");
        }
    }

    public Color GetPixelColorFromImage(Vector3 screenPosition)
    {
        _color = Color.clear;

        // Imageのサイズに基づいてUV座標を計算
        var rect = _targetImage.rectTransform.rect;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_targetImage.rectTransform, screenPosition, null,
            out var localPoint);

        // アスペクト比を考慮してUV座標を計算
        var aspectRatio = (float)_texture.width / _texture.height;
        var imageWidth = rect.width;
        var imageHeight = rect.height;

        if (_targetImage.preserveAspect)
        {
            // アスペクト比に基づいて高さまたは幅を調整
            if (imageWidth / imageHeight > aspectRatio) // Width is greater
            {
                var adjustedWidth = imageHeight * aspectRatio;
                var offsetX = (imageWidth - adjustedWidth) / 2;
                localPoint.x -= offsetX;
                imageWidth = adjustedWidth;
            }
            else // Height is greater
            {
                var adjustedHeight = imageWidth / aspectRatio;
                var offsetY = (imageHeight - adjustedHeight) / 2;
                localPoint.y -= offsetY;
                imageHeight = adjustedHeight;
            }
        }

        // UV座標を計算
        var normalizedX = (localPoint.x - rect.x) / imageWidth;
        var normalizedY = (localPoint.y - rect.y) / imageHeight;

        // ピクセル座標を計算
        var pixelX = Mathf.FloorToInt(normalizedX * _texture.width);
        var pixelY = Mathf.FloorToInt(normalizedY * _texture.height);

        // ピクセル情報を取得
        if (pixelX >= 0 && pixelX < _texture.width && pixelY >= 0 && pixelY < _texture.height)
        {
            _color = _texture.GetPixel(pixelX, pixelY);
            // Debug.Log($"Pixel Color at ({pixelX}, {pixelY}): {_color}");
        }
        else
        {
            // Debug.LogError("Invalid pixel coordinates.");
        }


        return _color;
    }
}