## Requirements

Visual Studio 2015 or greater
Typescript 1.8 extension for Visual Studio, available at: https://www.microsoft.com/en-us/download/details.aspx?id=48593

Angular JS Umbraco datatype and dashboard control, made up of typescript/js and html files, which are located at:
```
~\uCondition.Umbraco\App_Plugins\uCondition\
```

## Sandbox for development

To run, set uCondition.Umbraco as the startup project and press F5 or View in browser.

You can access the Umbraco CMS via http://localhost:11251/umbraco

The credentials for Umbraco are:
- Username: admin@admin.com
- Password: qwert12345

The uCondition datatype is used on the homepage in the backoffice.

If you have any build issues, which may give vague build errors, it will likely be the incorrect typescript extension being used. The Typescript extension must be for Typescript 1.8.

## Manual installation
Nuget coming shortly.

- Build
- Copy uCondition.*.dll to bin
- Copy app_plugins/uCondition folders to project
- Apply namespace to /views/web.config (uCondition.Core.Extensions)