# Overview
This sample project focuses on several key areas: Gulp, Admin Area Mapping, and seamlessly switch between multiple web themes under the NET Core solution. Additionally, the well-organized project structure serves as a scalable and flexible model, making it an exemplary foundation for future extensions and modifications.


## Gulp Bundling and Minification
Check out this link on how the [Gulp](https://www.codeproject.com/Articles/1165004/How-to-Use-Gulp-with-ASP-NET-Core-MVC#:~:text=In%20the%20first%20step%2C%20we%20are%20going%20to%20load%20module.,-JavaScript&text=JavaScript-,var%20gulp%20%3D%20require('gulp')%3B%20%2F%2FUsing%20package,the%20path%20of%20%22%20wwwroot%20%22.&text=var%20paths%20%3D%20%7B%20webroot%3A%20%22.%2Fwwwroot%2F%22%20%7D%3B,-In%20the%20third) work.

The following Gulp commands are available:

- `gulp fonts` - copy fonts to the dist folder
- `gulp styles` - minify CSS, compile SASS to CSS
- `gulp scripts` - bundle and minify JS
- `gulp clean` - remove the dist folder
- `gulp build` - run the styles and scripts tasks
- `gulp watch` - watch all changes in all sass files

## Themes
Ui can be customized using themes integrated from [bootswatch](https://bootswatch.com/).

By default, configuration value is null to use default theme. if you want to use a theme, just fill the lowercase theme name

## Admin Area
The implementation of Admin Area mapping is to demonstrate on how streamlined and organized management of different sections within the user interface.