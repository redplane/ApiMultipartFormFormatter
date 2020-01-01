using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace ApiMultiPartFormData.Services.Interfaces
{
    public interface ICollectionBinderService
    {
        #region Methods

        /// <summary>
        /// Build request model value from property information and value.
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <param name="value"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<object> BuildModelAsync(PropertyInfo propertyInfo, object value, CancellationToken cancellationToken = default);

        #endregion
    }
}