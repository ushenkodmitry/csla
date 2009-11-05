'INSTANT C# NOTE: Formerly VB.NET project-level imports:

Imports Microsoft.VisualBasic
Imports System
Imports System.Linq
Imports System.Collections.Generic
Imports System.Xml
Imports System.Net
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Documents
Imports System.Windows.Ink
Imports System.Windows.Input
Imports System.Windows.Media
Imports System.Windows.Media.Animation
Imports System.Windows.Shapes

Imports Sample.Business

Namespace Sample
  Partial Public Class App
    Inherits Application

    Public Sub New()
      AddHandler App.Startup, AddressOf Application_Startup
      AddHandler App.Exit, AddressOf Application_Exit
      AddHandler App.UnhandledException, AddressOf Application_UnhandledException
      InitializeComponent()
    End Sub

    Private Sub Application_Startup(ByVal o As Object, ByVal e As StartupEventArgs)
      Csla.DataPortal.ProxyTypeName = GetType(Csla.DataPortalClient.WcfProxy(Of )).AssemblyQualifiedName
      Me.RootVisual = New Page()
    End Sub

    Private Sub Application_Exit(ByVal o As Object, ByVal e As EventArgs)

    End Sub

    Private Sub Application_UnhandledException(ByVal sender As Object, ByVal e As ApplicationUnhandledExceptionEventArgs)

      ' If the app is running outside of the debugger then report the exception using
      ' the browser's exception mechanism. On IE this will display it a yellow alert 
      ' icon in the status bar and Firefox will display a script error.
      If (Not System.Diagnostics.Debugger.IsAttached) Then

        ' NOTE: This will allow the application to continue running after an exception has been thrown
        ' but not handled. 
        ' For production applications this error handling should be replaced with something that will 
        ' report the error to the website and stop the application.
        e.Handled = True

        Try
          Dim errorMsg As String = e.ExceptionObject.Message + e.ExceptionObject.StackTrace
          errorMsg = errorMsg.Replace(""""c, "\"c).Replace("\r\n", "\n")

          System.Windows.Browser.HtmlPage.Window.Eval("throw New Error(""Unhandled Error in Silverlight 2 Application " & errorMsg & """);")
        Catch

        End Try
      End If
    End Sub

  End Class

End Namespace 'end of root namespace