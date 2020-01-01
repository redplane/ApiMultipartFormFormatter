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
    public class EnumModelBinderService : IModelBinderService
    {
        public object BuildModel(PropertyInfo propertyInfo, object value)
        {
            throw new NotImplementedException();
        }

        public Task<object> BuildModelAsync(Type propertyType, object value,
            CancellationToken cancellationToken = default)
        {
            if (value == null)
                throw new UnhandledParameterException();

            // Get property type.
            var underlyingType = Nullable.GetUnderlyingType(propertyType);

            // Property is Enum.
            if (propertyType.IsEnum)
                return Task.FromResult(ConvertToEnum(propertyType, value.ToString()));

            if (underlyingType != null && underlyingType.IsEnum)
            {
                if (string.IsNullOrWhiteSpace(value.ToString()))
                    throw new UnhandledParameterException();

                return Task.FromResult(ConvertToEnum(underlyingType, value.ToString()));
            }

            throw new UnhandledParameterException();
        }

        protected virtual object ConvertToEnum(Type type, string val)
        {
            if (int.TryParse(val, out var num))
                return Enum.ToObject(type, num);

            return Enum.Parse(type, val, true);
        }
    }
}