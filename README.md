# ApiMultipartFormDataFormatter [![Build status](https://ci.appveyor.com/api/projects/status/cwyutunb3xh8clik?svg=true)](https://ci.appveyor.com/project/redplane/apimultipartformformatter)

## A. Description:

 * Handle data uploaded from client using `multipart/form-data` Content-Type. Previously, to handle `multipart/form-data` in `WEB API 2`, there is just only one possible solution mentioned in [this topic](https://stackoverflow.com/questions/28369529/how-to-set-up-a-web-api-controller-for-multipart-form-data).
 * This library helps developers to serialize information into view model in `ASP.NET WEB API 2` just like ASP.Net does (please refer [this tutorial](https://stackoverflow.com/questions/54411250/how-to-send-multipart-form-data-to-asp-net-core-web-api)).
 * Developers can re-use [WEB API 2 Data annotation classes](https://docs.microsoft.com/en-us/aspnet/mvc/overview/older-versions/mvc-music-store/mvc-music-store-part-6) without repeating themselves doing manual data validation.
 

## B. Implementation:

**I**. **Installation**: 
- You can choose one of following source to install nuget package into your project:
    
    - [Nuget](https://www.nuget.org/packages/ApiMultipartFormDataFormatter/) (**Stable releases**)
    
    - [MyGet](https://www.myget.org/feed/apimultipartformdataformatter/package/nuget/ApiMultipartFormDataFormatter) (_nightly builds_)
    
      
**II**. **Formatter registration**:
 - Please select one of the following implementation below for `WebApiConfig.cs` or `Startup.cs`:

    1. **WITHOUT** dependency injection
    
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
      
    2. **WITH** dependency injection
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
            var containerBuilder = new ContainerBuilder();

            // Register formatter as a single instance in the system.
            containerBuilder.RegisterType<MultipartFormDataFormatter>()
                .InstancePerLifetimeScope();

            var container = containerBuilder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            // Register multipart/form-data formatter.
            var instance =
                (MultipartFormDataFormatter) config.DependencyResolver.GetService(typeof(MultipartFormDataFormatter));
       
            config.Formatters.Add(instance);
        }
        ```
    
**III**. **API Controller**
    
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


 
## C. HttpFile or HttpFileBase usage:

- To specify a parameter to be a file in a `ViewModel` class, please use `HttpFile` or `HttpFileBase` class. `HttpFile` and `HttpFileBase` are treated as [HttpPostedFileBase](https://docs.microsoft.com/en-us/dotnet/api/system.web.httppostedfilebase?view=netframework-4.8) class in `ASP.NET MVC`.
- Below is the example of `HttpFile` and `HttpFileBase` usage:

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

  [Required]
  public List<HttpFileBase> Photos { get; set; }
}
    
```

## IV. Data serializer customization with `IModelBinderService` 
(_Previously `IMultiPartFormDataModelBinderService`_)

- ~~In version `2.0.0`, this plugin uses `IMultiPartFormDataModelBinderService` to help developer override `multpart-form/data` parameter serialization.~~
- From version `3.0.0`, this plugin uses `IModelBinderService` to serialize data read from **MultipartFormDataContent** stream.

- This helps developers to initialize **Data type** that has not been recoginized by ASP.NET Web API (Mostly about custom classes or data type).

- For data that should be skipped analyzing, please throw `UnhandledParameterException` in `BuildModelAsync` function.

- Below is an example of `IModelBinderService` implementation that can be found in project [source code](https://github.com/redplane/ApiMultipartFormFormatter/tree/streaming-support/lib/ApiMultipartFormDataFormatter/Services/Implementations).
```
public class GuidModelBinderService : IModelBinderService
{
    #region Methods

    public Task<object> BuildModelAsync(Type propertyType, object value,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        // Get property type.
        var underlyingType = Nullable.GetUnderlyingType(propertyType);

        // Property is GUID.
        if (propertyType == typeof(Guid) && Guid.TryParse(value.ToString(), out var guid))
            return Task.FromResult((object) guid);

        if (underlyingType == typeof(Guid))
        {
            if (Guid.TryParse(value?.ToString(), out guid))
                return Task.FromResult((object)guid);

            return Task.FromResult(default(object));
        }

        throw new UnhandledParameterException();
    }


    #endregion
}
```

- `IModelBinderService` can be registered in **DI Frameworks** such as ([AutoFac](https://autofac.org/), [Ninject](http://www.ninject.org/), ...). This requires `MultipartFormDataFormatter` to be registered in **DI Frameworks** also. Example:

    ```
        // ...
        containerBuilder.RegisterType<GuidModelBinderService>()
                        .AsImplementedInterfaces()
                        .SingleInstance();
        // ...
    ```

- `IModelBinderService` can be registered **DIRECTLY** while initializing `MultipartFormDataFormatter` object.
    
    ```
        config.Formatters.Add(new MultipartFormDataFormatter(
                        new IModelBinderService[]
                    {
                        new GuidModelBinderService()
                    }));

    ```
  
- **BY DEFAULT**, `MultipartFormDataFormatter` is registered with a list of default services **WHEN NO ModelBinderService** is specified: 

```
        public MultipartFormDataFormatter(IEnumerable<IModelBinderService> modelBinderServices = null)
        {
            _modelBinderServices = modelBinderServices?.ToArray();

            if (_modelBinderServices == null || _modelBinderServices.Length < 1)
            {
                _modelBinderServices = new IModelBinderService[]
                {
                    new DefaultMultiPartFormDataModelBinderService(),
                    new GuidModelBinderService(),
                    new EnumModelBinderService(),
                    new HttpFileModelBinderService()
                };
            }


            // Register multipart/form-data as the supported media type.
            SupportedMediaTypes.Add(new MediaTypeHeaderValue(SupportedMediaType));
        }
```

## VI. Examples:
   - You can refer to [this project](https://github.com/redplane/ApiMultipartFormFormatter/tree/streaming-support/example/back-end) for further information of how to implement **ASP Web API 2** project with this plugin.

## VII. Limitation:
    
- Currently, this formatter cannot deal with interfaces such as: `IAccount`, `IList`, `IEnumerable`, ... To use it with a collection, please specify : `List`, `Enumerable`, ....
    
- For a collection, please use `List`, `Enumerable`, ... instead of `[] (array)`. This feature will be updated later.

## VIII. Change logs:

* 3.0.0:
    * Provided `HttpFile` and `HttpFileBase` support (previously `HttpFile` only). [HttpFileBase]() is recommended since it reads data using stream instead of convert data into bytes array. Please refer [discussion](https://github.com/redplane/ApiMultipartFormFormatter/pull/7) for further information. 
        - (Thank [Driedas](https://github.com/Driedas) for his pull request)
    
    * Replaced ~~IMultiPartFormDataModelBinderService~~ with `IModelBinderService` for custom data serialization. Therefore, `IModelBinderService` classes can be registered to handle custom data that plugin cannot handled.

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
