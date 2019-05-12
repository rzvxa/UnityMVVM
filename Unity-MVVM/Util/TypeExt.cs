using System;
using System.Reflection;

namespace Util
{
    public static class TypeExt
    {
        public static bool IsAssignableToGenericType(this Type givenType, Type genericType)
        {
            var interfaceTypes = givenType.GetInterfaces();

            foreach (var it in interfaceTypes)
            {
                if (it.IsGenericType && it.GetGenericTypeDefinition() == genericType)
                    return true;
            }

            if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
                return true;

            Type baseType = givenType.BaseType;
            if (baseType == null) return false;

            return IsAssignableToGenericType(baseType, genericType);
        }

        public static FieldInfo GetFieldRecursive(this Type givenType, string fieldName, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic)
        {
            FieldInfo fi = null;

            while (givenType != null) 
            {
                fi = givenType.GetField(fieldName, bindingFlags);

                if (fi != null) break;

                givenType = givenType.BaseType; 
            }

            if (fi == null)
            {
                throw new Exception($"Field '{fieldName}' not found in type hierarchy.");
            }

            return fi;
        }

        public static PropertyInfo GetPropertyRecursive(this Type givenType, string propertyName, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic)
        {
            PropertyInfo pi = null;
            while(givenType != null)
            {
                pi = givenType.GetProperty(propertyName, bindingFlags);

                if(pi != null) break;

                givenType = givenType.BaseType;
            }

            if (pi is null)
            {
                throw new Exception($"Property '{propertyName}' not found in type hierarchy.");
            }

            return pi;
        }
    }
}
