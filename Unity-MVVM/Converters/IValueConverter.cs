using System;

namespace UnityMVVM.Binding.Converters
{
    public interface IValueConverter
    {
        object Convert(object value, Type targetType, object parameter);
        object ConvertBack(object value, Type targetType, object parameter);
    }

    public static class Converter
    {
        public static object ChainConvert(this IValueConverter[] converters, object value, Type targetType, object parameter)
        {
            foreach (var converter in converters)
            {
                if (converter is null) continue;
                value = converter.Convert(value, targetType, parameter);
            }

            return value;
        }

        public static object ChainConvertBack(this IValueConverter[] converters, object value, Type targetType, object parameter)
        {
            for (var index = converters.Length - 1; index >= 0; index--)
            {
                if (converters[index] is null) continue;
                value = converters[index].ConvertBack(value, targetType, parameter);
            }

            return value;
        }
    }
}
