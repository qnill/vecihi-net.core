using System.ComponentModel;

namespace vecihi.helper
{
    public static class Convert
    {
        public static Type ToGenericType<Type>(string text)
        {
            return (Type)TypeDescriptor.GetConverter(typeof(Type)).ConvertFromInvariantString(text);
        }
    }
}
