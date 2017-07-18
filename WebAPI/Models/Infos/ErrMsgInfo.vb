Imports System.Net.Http
Imports System.Net

Public Class ErrMsgInfo
    Private m_ErrCode As String = ""
    Private m_ErrMsg As String = ""
    Private m_ErrTime As String = ""
    Private m_ErrJSON As String = ""

    Private m_Ex As Exception = Nothing
    Private m_RepMsg As HttpResponseMessage

    Public WriteOnly Property Ex As Exception
        Set(value As Exception)
            m_Ex = value
            If m_Ex IsNot Nothing Then
                m_ErrCode = m_Ex.HResult
                m_ErrMsg = m_Ex.Message
                GenErrJSON()
            End If
        End Set
    End Property

    Public ReadOnly Property RepMsg As HttpResponseMessage
        Get
            Return m_RepMsg
        End Get
    End Property


    Private Sub GenErrJSON()
        If m_ErrCode <> "" And m_ErrMsg <> "" Then
            '產生錯誤回傳的JSON 
            m_ErrJSON = "{" & Chr(34) & "ErrCode" & Chr(34) & ":" & Chr(34) & m_ErrCode & Chr(34) & "," & Chr(34) & "ErrMsg" & Chr(34) & ":" & Chr(34) & m_ErrMsg & Chr(34) & "}"
            '產生迴船的HttpResponseMessage 
            m_RepMsg = New HttpResponseMessage(HttpStatusCode.ExpectationFailed)
            m_RepMsg.Content = New StringContent(m_ErrJSON)
            m_RepMsg.Content.Headers.ContentType = New Headers.MediaTypeHeaderValue("application/json")
        End If
    End Sub
End Class
