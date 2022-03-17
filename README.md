# SitioPersona
Sitio Web, permite el registro de Entidades de tipo personas y Usuarios a través de un formulario.

# Instalación y prueba del Sitio Web 
1. Para poder desplegar aplicaciones .Net core es necesario .NET Core Hosting Bundle el cual prepara IIS para .net core.
   descargarlo desde el siguiente link e instalarlo
   https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/runtime-aspnetcore-6.0.3-windows-hosting-bundle-installer

2. Descargar y descomprimir el archivo SitioPersona.rar, el cual se encuentra en el repositorio

3. Verificar la URL del API en el archivo "appsettings.json", esta es la URL que el sitio consume para consumir el API y poder ir hasta la base de datos
   URL => https://localhost:44386/Api/ en este caso esta es la URL que se tenía para cuestiones de pruebas, pero para poder probar deben colocar la que se genere cuando ya configuren su API
   ![image](https://user-images.githubusercontent.com/39510736/158774062-8c2ddc8f-0c57-44b5-a7d2-1b7bc9d1898c.png)

4. Ingresar al IIS y dar clic derecho en la carpeta sitios y escoger la opción agregar un nuevo sitio
   Colocar el nombre de su preferencia para el sitio y colocar la ruta donde se encuentra la aplicación, que en ese caso será donde descomprima el archivo "SitioPersona.rar"
   ![image](https://user-images.githubusercontent.com/39510736/158774632-2d62c0ac-ab1e-46ee-a46e-1f880e2e501f.png)

# Funcionamiento

5. Luego de configurado, podemos examinar el sitio y va tener el siguiente aspecto
   ![image](https://user-images.githubusercontent.com/39510736/158775065-c107d774-57d7-4c39-9f8e-bc7984375b1d.png)
   * El index del sitio Web nos muestra la tabla con las personas que se encuentren registradas
   * La opción inicio del menú de navegación muestra una tabla con todas las personas registradas y con su respectivo usuario
   * La opción login del menú de navegación nos lleva a un formulario que pide usuario y contraseña y tiene un botón de iniciar sesión, si el usuario existe en la base        de datos, le mostrará el detalle de la persona
     ![image](https://user-images.githubusercontent.com/39510736/158776399-3a8a12bb-8d33-43c9-89f9-d25d341e135d.png)
   * El botón registrar nos lleva al formulario de registro de persona, el cual contiene los campos para el posterior registro incluyendo los del usuario
     ![image](https://user-images.githubusercontent.com/39510736/158776529-1a764761-5a7d-4776-a009-b8e22ea5049d.png)


