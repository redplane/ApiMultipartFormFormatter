using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using ApiMultiPartFormData.Models;

namespace ApiMultiPartFormData.Services.Interfaces
{
    public interface IModelBinderService
    {
        #region Methods

        /// <summary>
        /// Build request model value from property information and value.
        /// </summary>
        /// <param name="propertyType"></param>
        /// <param name="value"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<object> BuildModelAsync(Type propertyType, object value, CancellationToken cancellationToken = default);

        #endregion
    }
}