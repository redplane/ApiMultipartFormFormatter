using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using ApiMultiPartFormData.Exceptions;
using ApiMultiPartFormData.Models;
using ApiMultiPartFormData.Services.Interfaces;

namespace ApiMultiPartFormData.Services.Implementations
{
    public class DefaultMultiPartFormDataModelBinderService : IMultiPartFormDataModelBinderService
    {
        #region Methods

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual object BuildModel(PropertyInfo propertyInfo, object value)
        {
            // Property is not defined.
            if (propertyInfo == null)
                return null;

            // Get property type.
            var propertyType = propertyInfo.PropertyType;
            var underlyingType = Nullable.GetUnderlyingType(propertyType);

            // Other Nullable types
            if (underlyingType != null)
            {
                if (string.IsNullOrEmpty(value.ToString())) return null;
                propertyType = underlyingType;
            }

            try
            {
                return Convert.ChangeType(value, propertyType);
            }
            catch (InvalidCastException)
            {
                throw new UnhandledParameterException();
            }
        }

        public virtual Task<object> BuildModelAsync(PropertyInfo propertyInfo, object value, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(BuildModel(propertyInfo, value));
        }

        #endregion
    }
}