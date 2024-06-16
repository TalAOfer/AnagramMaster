using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ColoredFieldAttribute))]
public class ColoredFieldDrawer : OdinAttributeDrawer<ColoredFieldAttribute>
{
    private Color ToneDownColor(Color originalColor, float saturationFactor = 0.4f, float brightnessFactor = 1f)
    {
        // Convert to HSV
        Color.RGBToHSV(originalColor, out float h, out float s, out float v);

        // Adjust saturation and brightness
        s *= saturationFactor;
        v *= brightnessFactor;

        // Convert back to RGB
        return Color.HSVToRGB(h, s, v);
    }

    protected override void DrawPropertyLayout(GUIContent label)
    {
        var attribute = (ColoredFieldAttribute)this.Attribute;
        var colorMethod = this.Property.ParentType.GetMethod(attribute.ColorMethodName, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public);

        if (colorMethod == null || colorMethod.ReturnType != typeof(Color))
        {
            SirenixEditorGUI.ErrorMessageBox($"Method '{attribute.ColorMethodName}' not found or does not return a Color.");
            this.CallNextDrawer(label);
            return;
        }

        var color = (Color)colorMethod.Invoke(this.Property.ParentValues[0], null);
        var tonedDownColor = ToneDownColor(color);
        GUIHelper.PushColor(tonedDownColor);
        this.CallNextDrawer(label);
        GUIHelper.PopColor();
    }
}
