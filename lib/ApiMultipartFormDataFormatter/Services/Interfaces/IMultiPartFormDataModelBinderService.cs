using System.Reflection;

namespace ApiMultiPartFormData.Interfaces
{
    public interface IMultiPartFormDataModelBinderService
    {
        #region Methods

        /// <summary>
        /// Build request model value from property information and value.
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        object BuildModel(PropertyInfo propertyInfo, object value);

        #endregion
    }
}