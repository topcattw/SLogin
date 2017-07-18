Imports System.Net
Imports System.Net.Http
Imports System.Web.Http
Imports System.Web.Http.Cors

Namespace Controllers
    <EnableCors("*", "*", "*")>
    Public Class UserController
        Inherits ApiController

        ' GET: api/User
        <HttpGet>
        <Route("api/User/UToken")>
        Public Function GetUserByUToken() As UserInfo
            Try
                Dim oUser As New UserInfo

                '從Request.Header取得UToken
                Dim UToken As String = ""
                If Request.Headers.Contains("UToken") Then
                    UToken = Request.Headers.GetValues("UToken").First()
                End If

                If UToken <> "" Then
                    Dim dao As New UserDao
                    oUser = dao.getUserByUToken(UToken)
                Else
                    Throw New Exception("無UToken傳入")
                End If

                Return oUser
            Catch ex As Exception
                Dim oErr As New ErrMsgInfo
                oErr.Ex = ex

                Dim RepMsg As HttpResponseMessage = oErr.RepMsg
                Throw New HttpResponseException(RepMsg)
            End Try
        End Function

        '' GET: api/User
        'Public Function GetValues() As IEnumerable(Of String)
        '    Return New String() {"value1", "value2"}
        'End Function

        '' GET: api/User/5
        'Public Function GetValue(ByVal id As Integer) As String
        '    Return "value"
        'End Function

        ' POST: api/User/Login
        <HttpPost>
        <Route("api/User/Login")>
        Public Function PostLogin(<FromBody()> ByVal oLoginIn As LoginInInfo) As LoginOutInfo
            Try
                '取得ClientIP
                Dim ClientIP As String = HttpContext.Current.Request.UserHostAddress

                Dim oLoginOut As New LoginOutInfo
                Dim dao As New UserDao
                Dim UToken As String = ""
                Dim Rc As String = dao.Login(oLoginIn.USRID, oLoginIn.PW, ClientIP, UToken)
                oLoginOut.Rc = Rc
                oLoginOut.UToken = UToken

                Return oLoginOut

            Catch ex As Exception
                Dim oErr As New ErrMsgInfo
                oErr.Ex = ex

                Dim RepMsg As HttpResponseMessage = oErr.RepMsg
                Throw New HttpResponseException(RepMsg)

            End Try
        End Function

        '' POST: api/User
        'Public Sub PostValue(<FromBody()> ByVal value As String)

        'End Sub

        '' PUT: api/User/5
        'Public Sub PutValue(ByVal id As Integer, <FromBody()> ByVal value As String)

        'End Sub

        '' DELETE: api/User/5
        'Public Sub DeleteValue(ByVal id As Integer)

        'End Sub
    End Class
End Namespace