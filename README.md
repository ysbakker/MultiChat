![](Design.png)
# MultiChat

This project was build as an assignment for the HAN University of Applied
Sciences. It features a client and server application built using the .NET
framework. Both applications run natively on macOS thanks to the `Xamarin.Mac`
library.

## Things to note

There are some quirks to this project that are far from best practice. It should therefore probably not be copied. For example, it uses low-level networking classes like `System.Net.Sockets.TcpClient` because this was a specific requirement for the assignment. Also, most of this application's code is placed directly in the `ViewController` file. This is definitely not a good idea, but is fine according to the assignment's requirements. The application should be _dummy-proof_ though, as relevant exceptions are handled properly everywhere.