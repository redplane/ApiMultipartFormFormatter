# ngx-structure

Description:

 * This is a simple structure of an `Angular 5` application. This project is integrated with Webpack for files bundling & live reload.

 * Language supported : 
    * Typescript.

* Angular CLI:
    * 1.7.4
    
Online demo can be found [here](http://localhost:4200/#!/):

#### Project structure.
```
.
+-- src
|   +-- assets
|   +-- constants
|   +-- environments
|   +-- factories
|   +-- guards
|   +-- interceptors
|   +-- interfaces
|   +-- models
|   +-- modules
|       +-- <module a>
|           +-- <module a>.component.html
|           +-- <module a>.component.ts
|       +-- <module b>
|           +-- <module b>.component.html
|           +-- <module b>.component.ts
|       <parent module>.module.ts
|       <parent module>.module.ts
+-- index.html (Index file)
```

#### Structure description.
- ```assets``` : Static files (such as: ```*.css```, ```*.json```, ...) should be stored in this folder.
- ```constants```: Appliation constant files should be stored in this folder (such as: ```app-settings.constant.ts```, ```urlStates.constant.ts```, ...). Naming convention: ```*.constants.ts```.
- ```environments```: Contains classes or variables that depend on build environment.
- ```factories```: Contains application factory functions.
- ```interceptors```: Contains application interceptor.
- ```modules```: Application main modules should be stored in this folder
- ```<module a>```: Folder contains module a definition. Such as : ```account```, ```category```, ...
- ```<module a>.component.html```: Template file for ```module a```.
- ```<module a>.component.ts```: Logic definition file of ```module a```.
- ```<parent module>.module.ts```: Importing/exporting children modules. For example: ```user-management```, ```category-management```.
- ```<parent module>.route.ts```: Child modules' routes definition.
- ```interfaces```: Contains application interfaces which is for classes to implement.
- ```models```: Contains application model files, mostly about database entities.
- ```modules```: Contains application modules files.
- ```services```: Contains application services to inject to components.
- ```view-models```: Contains view-model files that are bridge between api end-point and application.
- ```index.html```: Application entry html file.

### Project commnands.
- `ng serve`: Bundle and publish project files. Files which are generated will be placed inside `dist` folder.
- `npm build --prod`: Bundle and start watching project. This is for development stage. Browser will be reloaded automatically when changes are detected.

### Added plugins:
- `ngx-translate`: For i18n translation in app. Please refer [its document](https://github.com/ngx-translate/core).
- `ngx-moment`: Provides time calculation pipeline. Please refer [its document](https://github.com/urish/ngx-moment)
- `font-awesome`: [Fonts system](https://fontawesome.com/?from=io).
- `jquery-slimscroll`: [Slimscroll for website](https://github.com/rochal/jQuery-slimScroll).
- `ngx-bootstrap`: [Bootstrap components for angular x](https://github.com/valor-software/ngx-bootstrap)
- `admin-lte`: Free admin website template. Source: [adminlte.io](https://adminlte.io/themes/AdminLTE/index2.html)
- `bootstrap 3`: [Responsive web design framework](http://getbootstrap.com/docs/3.3/).


### Bugs report.
- While using this plugin, if you find any errors, please create issues at [project page](https://github.com/redplane/ngx-structure)


