# ApiMultipartFormDataFormatter [![Build status](https://ci.appveyor.com/api/projects/status/cwyutunb3xh8clik?svg=true)](https://ci.appveyor.com/project/redplane/apimultipartformformatter)

## Features:

 * Receives multipart/form-data request from client, parses information and bind to model.

---
## Implementation:
---

### WEB API 2 implementation:
To use this custom media format in your WEB API 2 project. Please following these steps:

 * Install the lastest nuget package by using command `Install-Package ApiMultipartFormDataFormatter`.
 * Open WebApiConfig.cs file and add the following command: `config.Formatters.Add(new MultipartFormDataFormatter());`
 
 	**WebApiConfig.cs:**
 	```
    public static void Register(HttpConfiguration config)
    {
        // Web API configuration and services

        // Web API routes
        config.MapHttpAttributeRoutes();

        config.Routes.MapHttpRoute(
            "DefaultApi",
            "api/{controller}/{id}",
            new {id = RouteParameter.Optional}
        );

        config.Formatters.Add(new MultipartFormDataFormatter());
    }
    ``` 	
    
    **In controller file**
    ```
    [RoutePrefix("api/account")]
    public class ApiAccountController : ApiController
    {
        [Route("register")]
        [HttpPost]
        public HttpResponseMessage Register(AccountRegistrationViewModel parameters)
        {
            if (parameters == null)
            {
                parameters = new AccountRegistrationViewModel();
                Validate(parameters);
            }

            if (!ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
    ```
    
 * Start posting a multipart/form-data to your WEB API 2 project and enjoy.
 
 
### Note:

 * To specify a parameter is a file, please use `HttpFile` class.
 	* For example:

```
public class Category
{
	public int Id { get; set; }

	public List<Photo> Photos { get; set; }
}

public class AccountRegistrationViewModel
{
  /// <summary>
  /// Account owner.
  /// </summary>
  [Required]
  public Owner Owner { get; set; }

  /// <summary>
  /// User age.
  /// </summary>
  [Required]
  public int Age { get; set; }

  /// <summary>
  /// Account photo.
  /// </summary>
  [Required]
  public HttpFile Photo { get; set; }
}
    
```

---
## Implementation of `IMultiPartFormDataModelBinderService`

- In version `2.0.0`, this plugin uses `IMultiPartFormDataModelBinderService` to help developer override `multpart-form/data` parameter serialization.
- You can take a look at basic implemenation of class `BaseMultiPartFormDataModelBinderService` to know how to implement it:

```
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
        if (propertyType == typeof(Guid) && Guid.TryParse(value.ToString(), out var guid))
            return guid;

        return Convert.ChangeType(value, propertyType);
    }

    #endregion
}
```

---

## Limitation:
* Currently, this formatter cannot deal with interfaces such as: IAccount, IList, IEnumerable, ... To use it with a collection, please specify : List, Enumerable, ....
* For a collection, please use List, Enumerable, ... instead of [] (array). This feature will be updated later.

## Change log:

* 2.1.0:
    * Merge [pull request 6](https://github.com/redplane/ApiMultipartFormFormatter/pull/6) created by [everfrown](https://github.com/everfrown) which supported extended data types binding such as: 
        - enum (string or number)
        - guid
        - and nullable value types
    
    * Removed ApiMultipartFormDataFormatter.cs class. Please use [MultipartFormDataFormatter.cs](https://github.com/redplane/ApiMultipartFormFormatter/blob/master/lib/ApiMultipartFormDataFormatter/MultipartFormDataFormatter.cs) instead.

* 1.0.4:
    * Fixed bug [#3](https://github.com/redplane/ApiMultipartFormFormatter/issues/3) : *Bad request while trying casting string to GUID*
    * Added `IMultiPartFormDataModelBinderService` for intercepting model serialization.  
    * `ApiMultipartFormDataFormatter` now is obsoleted. An exception is thrown if this class is used. Please using `MultipartFormDataFormatter` instead.
    * `FindContentDispositionParametersInterceptor`: Allow developer to custmize how parameter will be serialized. Please take a look at [MultipartFormDataFormatter.cs](https://github.com/redplane/ApiMultipartFormFormatter/blob/master/lib/ApiMultipartFormDataFormatter/MultipartFormDataFormatter.cs)
    
* 1.0.3:
    * Prevent dependencies such as `NewtonSoft.Json`, ... from being compiled and included in release nuget package. Therefore, the package size is smaller.
    * Prevent dependencies from being removed when `ApiMultipartFormDataFormatter` nuget is uninstalled.

* 1.0.1:
    * Fixed issue about list serialization, mentioned [here](https://github.com/redplane/ApiMultipartFormFormatter/issues/2)

* 1.0.2:
    * Incorrect release version. Please skip this.
    
* 1.0.0:
    * Initial release.

## IMPORTANT NOTE:
* While sending the request, please make sure not to attach `Content-Type` in header or make `Content-Type` be `NULL` 
* `ApiMultipartFormDataFormatter` is obsolete and will be removed in version after 1.0.3. Please use `MultipartFormDataFormatter` instead.


### Images:

[Postman request](http://i.imgur.com/q8Elrwv.png)

[Parameter analyzation](http://i.imgur.com/PttfICl.png)