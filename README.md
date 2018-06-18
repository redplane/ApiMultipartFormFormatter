# ApiMultipartFormDataFormatter [![Build status](https://ci.appveyor.com/api/projects/status/cwyutunb3xh8clik?svg=true)](https://ci.appveyor.com/project/redplane/apimultipartformformatter)

## Features:

 * Receives multipart/form-data request from client, parses information and bind to model.

## Usages:
To use this custom media format in your WEB API 2 project. Please following these steps:

 * Install the lastest nuget package by using command `Install-Package ApiMultipartFormDataFormatter`.
 * Open WebApiConfig.cs file and add the following command: `config.Formatters.Add(new MultipartFormDataFormatter());`
 	* For example:
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
    
    In controller file:
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

## Limitation:
* Currently, this formatter cannot deal with interfaces such as: IAccount, IList, IEnumerable, ... To use it with a collection, please specify : List, Enumerable, ....
* For a collection, please use List, Enumerable, ... instead of [] (array). This feature will be updated later.

## Change log:
* 1.0.0:
    * Initial release.

* 1.0.1:
    * Fixed issue about list serialization, mentioned [here](https://github.com/redplane/ApiMultipartFormFormatter/issues/2)

* 1.0.2:
    * Incorrect release version. Please skip this.

* 1.0.3:
    * Prevent dependencies such as `NewtonSoft.Json`, ... from being compiled and included in release nuget package. Therefore, the package size is smaller.
    * Prevent dependencies from being removed when `ApiMultipartFormDataFormatter` nuget is uninstalled.

## IMPORTANT NOTE:
* While sending the request, please make sure not to attach `Content-Type` in header or make `Content-Type` be `NULL` 
* `ApiMultipartFormDataFormatter` is obsolete and will be removed in version after 1.0.3. Please use `MultipartFormDataFormatter` instead.


### Images:

[Postman request](http://i.imgur.com/q8Elrwv.png)

[Parameter analyzation](http://i.imgur.com/PttfICl.png)