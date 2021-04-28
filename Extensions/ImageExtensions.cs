using UnityEngine;
using UnityEngine.UI;

namespace Mox
{
    public static partial class Extensions
    {
        public static void Expand(this Image image, RectTransform.Axis axis, float amount)
		{
            if (image.type != Image.Type.Tiled)
            {
                Log.Warning(LogCat.UX, $"Image {image.name} is not tiled. ");
                return;
            }

            // L = x, R = z, T = w, B = y
            var border = image.sprite.border;

            var margin = axis == RectTransform.Axis.Horizontal ? border.x + border.z : border.w + border.y;
            var scalar = (axis == RectTransform.Axis.Horizontal ? image.sprite.rect.width : image.sprite.rect.height) - margin;

            var size = amount * scalar + margin;

            image.rectTransform.SetSizeWithCurrentAnchors(axis, size);
        }

        public static Color AddAlpha(this Color color, float alpha) => new Color(color.r, color.g, color.b, alpha);
    }
}