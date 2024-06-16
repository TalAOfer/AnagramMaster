using System;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
public class ColoredFieldAttribute : Attribute
{
    public string ColorMethodName;

    public ColoredFieldAttribute(string colorMethodName)
    {
        this.ColorMethodName = colorMethodName;
    }
}