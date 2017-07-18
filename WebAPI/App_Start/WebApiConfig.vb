Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web.Http
Imports System.Web.Http.Cors '增加這個Import

Public Module WebApiConfig
    Public Sub Register(ByVal config As HttpConfiguration)
        ' Web API 設定和服務
        config.EnableCors() '加上這個設定

        ' Web API 路由
        config.MapHttpAttributeRoutes()

        config.Routes.MapHttpRoute(
            name:="DefaultApi",
            routeTemplate:="api/{controller}/{id}",
            defaults:=New With {.id = RouteParameter.Optional}
        )
    End Sub
End Module
