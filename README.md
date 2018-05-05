# ApiMultipartFormDataFormatter

### Features:

 * Receives multipart/form-data request from client, parses information and bind to model.

### Usages:
To use this custom media format in your WEB API 2 project. Please following these steps:

 * Install the lastest nuget package by using command `Install-Package ApiMultipartFormDataFormatter`.
 * Open WebApiConfig.cs file and add the following command: `config.Formatters.Add(new ApiMultipartFormDataFormatter());`
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

        config.Formatters.Add(new ApiMultipartFormDataFormatter());
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

### Limitation:
* Currently, this formatter cannot deal with interfaces such as: IAccount, IList, IEnumerable, ... To use it with a collection, please specify : List, Enumerable, ....
* For a collection, please use List, Enumerable, ... instead of [] (array). This feature will be updated later.

### NOTE:
* While sending the request, please make sure not to attach `Content-Type` in header or make `Content-Type` be `NULL` 
### Images:

[Postman request](http://i.imgur.com/q8Elrwv.png)

[Parameter analyzation](http://i.imgur.com/PttfICl.png)