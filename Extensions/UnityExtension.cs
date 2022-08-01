using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace RF5.HisaCat.AllItemsHere.Extensions
{
    internal static class TransformExtension
    {
        #region DestroyAllChildren
        public static void DestroyAllChildren(this MonoBehaviour behaviour)
        {
            TransformExtension.DestroyAllChildren(behaviour.transform);
        }
        public static void DestroyAllChildren(this GameObject gameObject)
        {
            TransformExtension.DestroyAllChildren(gameObject.transform);
        }
        public static void DestroyAllChildren(this Transform transform)
        {
            var count = transform.childCount;
            for (int i = count - 1; i >= 0; i--)
            {
                var child = transform.GetChild(i);
                GameObject.Destroy(child.gameObject);
            }
        }
        public static void DestroyAllChildren(this MonoBehaviour behaviour, IEnumerable<Transform> except)
        {
            TransformExtension.DestroyAllChildren(behaviour.transform, except);
        }
        public static void DestroyAllChildren(this GameObject gameObject, IEnumerable<Transform> except)
        {
            TransformExtension.DestroyAllChildren(gameObject.transform, except);
        }
        public static void DestroyAllChildren(this Transform transform, IEnumerable<Transform> except)
        {
            if (except == null || except.Any() == false)
            {
                DestroyAllChildren(transform);
                return;
            }

            var count = transform.childCount;
            for (int i = count - 1; i >= 0; i--)
            {
                var child = transform.GetChild(i);

                if (!except.Contains(child))
                    GameObject.Destroy(child.gameObject);
            }
        }
        #endregion DestroyAllChildren
    }
    internal static class RectTransformExtension
    {
        public static void SetLeft(this RectTransform rectTransform, float value)
        {
            rectTransform.offsetMin = new Vector2(value, rectTransform.offsetMin.y);
        }
        public static void SetRight(this RectTransform rectTransform, float value)
        {
            rectTransform.offsetMax = new Vector2(-value, rectTransform.offsetMax.y);
        }
        public static void SetTop(this RectTransform rectTransform, float value)
        {
            rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, -value);
        }
        public static void SetBottom(this RectTransform rectTransform, float value)
        {
            rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, value);
        }
        public static void SetFitToParent(this RectTransform rectTransform)
        {
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
        }

        public static Rect GetGlobalRect(this RectTransform rt, Canvas canvas, Camera camera)
        {
            RectTransform s;
            var scale = rt.localScale;

            // Convert the rectangle to world corners and grab the top left
            Vector3[] worldConers = new Vector3[4];
            rt.GetWorldCorners(worldConers);

            //World position to ugui position
            RectTransform CanvasRect = canvas.GetComponent<RectTransform>();
            var cs = canvas.GetComponent<CanvasScaler>();


            Vector2 ViewportPosition = camera.WorldToViewportPoint(worldConers[0]);

            Vector2 topLeft = new Vector2(
            (ViewportPosition.x * CanvasRect.sizeDelta.x),
            (ViewportPosition.y * CanvasRect.sizeDelta.y));

            // Rescale the size appropriately based on the current Canvas scale
            Vector2 scaledSize = new Vector2(scale.x * rt.rect.size.x, scale.y * rt.rect.size.y);

            return new Rect(topLeft, scaledSize);
        }
    }
}
