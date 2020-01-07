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
        public Task<object> BuildModelAsync(Type propertyType, object value,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (value == null)
                throw new UnhandledParameterException();

            // Get property type.
            var underlyingType = Nullable.GetUnderlyingType(propertyType);

            // Property is Enum.
            if (propertyType.IsEnum)
            {
                try
                {
                    var handledValue = ConvertToEnum(propertyType, value.ToString());
                    return Task.FromResult(handledValue);
                }
                catch
                {
                    throw new UnhandledParameterException();
                }
            }

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
            object handledEnum = null;

            if (int.TryParse(val, out var num))
                handledEnum = Enum.ToObject(type, num);
            else
                handledEnum = Enum.Parse(type, val, true);

            if (!Enum.IsDefined(type, handledEnum))
                throw new UnhandledParameterException();

            return handledEnum;
        }
    }
}