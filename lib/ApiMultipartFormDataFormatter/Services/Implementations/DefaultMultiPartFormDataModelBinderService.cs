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
    public class DefaultMultiPartFormDataModelBinderService : IModelBinderService
    {
        #region Methods

        public virtual object BuildModel(Type propertyType, object value)
        {
            // Property is not defined.
            if (propertyType == null)
                return null;

            // Get property type.
            var underlyingType = Nullable.GetUnderlyingType(propertyType);

            // Other Nullable types
            if (underlyingType != null)
            {
                if (string.IsNullOrEmpty(value.ToString())) return null;
                propertyType = underlyingType;
            }

            try
            {
                if (value == null)
                    return Activator.CreateInstance(propertyType);

                return Convert.ChangeType(value, propertyType);
            }
            catch (InvalidCastException)
            {
                throw new UnhandledParameterException();
            }
        }

        public virtual Task<object> BuildModelAsync(Type propertyType, object value, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(BuildModel(propertyType, value));
        }

        #endregion
    }
}