using System;
using System.Linq;
using System.Reflection;
using ApiMultiPartFormData.Services.Interfaces;

namespace ApiMultiPartFormData.Services.Implementations
{
    public class BaseMultiPartFormDataModelBinderService : IMultiPartFormDataModelBinderService
    {
        #region Methods

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public object BuildModel(PropertyInfo propertyInfo, object value)
        {
            // Property is not defined.
            if (propertyInfo == null)
                return null;

            // Get property type.
            var propertyType = propertyInfo.PropertyType;
            var underlyingType = Nullable.GetUnderlyingType(propertyType);

            // Property is GUID.
            if (propertyType == typeof(Guid) && Guid.TryParse(value.ToString(), out var guid))
                return guid;
            if (underlyingType == typeof(Guid))
            {
                if (Guid.TryParse(value.ToString(), out guid))
                    return guid;
                return null;
            }

            // Property is Enum.
            if (propertyType.IsEnum)
                return convertToEnum(propertyType, value.ToString());
            if (underlyingType != null && underlyingType.IsEnum)
            {
                if (string.IsNullOrWhiteSpace(value.ToString()))
                    return null;
                return convertToEnum(underlyingType, value.ToString());
            }

            // Other Nullable types
            if (underlyingType != null)
            {
                if (string.IsNullOrEmpty(value.ToString())) return null;
                propertyType = underlyingType;
            }

            return Convert.ChangeType(value, propertyType);
        }

        object convertToEnum(Type type, string val)
        {
            if (int.TryParse(val, out var num))
                return Enum.ToObject(type, num);

            return Enum.Parse(type, val, true);
        }

        #endregion
    }
}