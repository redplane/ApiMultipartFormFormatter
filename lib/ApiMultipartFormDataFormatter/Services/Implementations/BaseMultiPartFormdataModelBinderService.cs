using System;
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

            // Property is GUID.
            if ((propertyType == typeof(Guid) || propertyType == typeof(Guid?)) && Guid.TryParse(value.ToString(), out var guid))
                return guid;

            return Convert.ChangeType(value, propertyType);
        }

        #endregion
    }
}