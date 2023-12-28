using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenuUIExtension
{
    public static class GameObjectExtensions
    {
        public static GameObject[] GetChildren(this GameObject @object, int maxChildren = 0)
        {
            int childCount = @object.transform.childCount;
            if (maxChildren == 0) maxChildren = childCount;
            GameObject[] children = new GameObject[childCount];
            for (int i = 0; i < maxChildren; i++)
            {
                children[i] = @object.transform.GetChild(i).gameObject;
            }
            return children;
        }
    }
    internal class utils
    {
        internal static Font GetFont(string name)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            Object[] fonts = Object.FindObjectsOfTypeAll(typeof(Font));
#pragma warning restore CS0618 // Type or member is obsolete
            foreach (Font font in fonts.Cast<Font>())
            {
                if (font.name == name) return font;
            }
            return null;
        }
        internal static GameObject CreateBaseUIButton()
        {
            GameObject obj = new("Text");
            Text text = obj.AddComponent<Text>();
            obj.AddComponent<Button>();
            text.font = GetFont("Arial");
            text.fontSize = 30;
            text.alignment = TextAnchor.UpperLeft;
            text.horizontalOverflow = HorizontalWrapMode.Overflow;
            obj.AddComponent<RectTransform>();
            obj.AddComponent<CanvasRenderer>();
            return obj;
        }
    }
}
