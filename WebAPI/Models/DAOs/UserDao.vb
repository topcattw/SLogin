Imports System.Data
Imports System.Data.SqlClient
Imports Dapper

Public Class UserDao
    ''' <summary>
    ''' 登入驗證，並傳回Token
    ''' </summary>
    ''' <param name="UsrID">使用者代號</param>
    ''' <param name="PW">密碼</param>
    ''' <param name="IP">IP</param>
    ''' <param name="UToken">回傳UToen</param>
    ''' <returns>
    ''' 1.取得傳入資料
    ''' 2.密碼加密(SHA256)
    ''' 3.驗證使用者代號與加密後的密碼
    ''' 4.如果驗證失敗，傳回錯誤
    ''' 5.驗證成功：
    '''     5-1.產生新的UToken
    '''     5-2.取得UToken逾時時間設定
    '''     5-3.計算UToken逾時時間
    '''     5-4.刪除過期的UToken
    '''     5-5.維護本次UToken
    ''' 6.傳回
    ''' </returns>
    Public Function Login(ByVal UsrID As String, ByVal PW As String, ByVal IP As String, ByRef UToken As String) As String
        Try
            Dim Rc As String = ""
            Dim oConnStr As New ConnStrInfo
            Using Conn As New SqlConnection(oConnStr.ConnStr)
                Conn.Open()
                Dim SqlTxt As String = ""
                '**     1.取得傳入資料
                '**     2.密碼加密(SHA256)
                Dim oEnc As New PUtilEncrypt.CUtilEncrypt
                Dim PWEnc As String = oEnc.SHA256_Encrypt(PW)

                '**     3.驗證使用者代號與加密後的密碼
                SqlTxt &= " SELECT USRID "
                SqlTxt &= " FROM TCATUser (NOLOCK) "
                SqlTxt &= " WHERE USRID = @USRID AND PW=@PW "
                SqlTxt &= "  "
                Dim Cmmd As New SqlCommand(SqlTxt, Conn)
                Cmmd.Parameters.AddWithValue("@USRID", UsrID)
                Cmmd.Parameters.AddWithValue("@PW", PWEnc)

                Dim Dr As SqlDataReader = Cmmd.ExecuteReader
                '**     4.如果驗證失敗，傳回錯誤
                If Not Dr.HasRows Then
                    Throw New Exception("帳號或密碼錯誤，請確認您的帳號密碼後重新登入，或者使用忘記密碼重新設定密碼～")
                End If
                Dr.Close()
                '**     5.驗證成功：
                '**         5-1.產生新的UToken
                UToken = Guid.NewGuid.ToString.ToUpper
                '**         5-2.取得UToken逾時時間設定

                Dim TimeoutMinute As Integer = 20   '預設20分鐘

                '**         5-3.計算UToken逾時時間
                Dim UTokenTime As String = Format(DateAdd(DateInterval.Minute, TimeoutMinute, Now), "yyyyMMddHHmmss")
                Dim NowTime As String = Format(Now, "yyyyMMddHHmmss")

                '**         5-4.刪除過期的UToken
                SqlTxt = ""
                SqlTxt &= " DELETE [dbo].[TCATUToken] "
                SqlTxt &= " WHERE UTokenTimeOut < @NowTime "
                SqlTxt &= "  "

                Conn.Execute(SqlTxt, New With {.NowTime = NowTime})

                '**         5-5.維護本次UToken
                SqlTxt = ""
                SqlTxt &= " INSERT INTO TCATUToken "
                SqlTxt &= " 	(UToken, USRID, UTokenTimeOut, IP, LastInTime) "
                SqlTxt &= " VALUES (@UToken, @USRID, @UTokenTimeOut, @IP, @LastInTime) "
                SqlTxt &= "  "

                Dim oUToken As New UTokenInfo
                oUToken.USRID = UsrID
                oUToken.UToken = UToken
                oUToken.UTokenTimeOut = UTokenTime
                oUToken.IP = IP
                oUToken.LastInTime = NowTime

                Conn.Execute(SqlTxt, oUToken)

                '**     6.傳回
                Return "Success"
            End Using

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Function getUserByUToken(ByVal UToken As String) As UserInfo
        Try
            Dim oUser As New UserInfo
            Dim oConnStr As New ConnStrInfo
            Using Conn As New SqlConnection(oConnStr.ConnStr)
                Conn.Open()
                Dim SqlTxt As String = ""
                Dim NowTime As String = Format(Now, "yyyyMMddHHmmss")

                '依據傳入的UToken，取得User相關資料
                SqlTxt = ""
                SqlTxt &= " SELECT U.* "
                SqlTxt &= " FROM TCATUser U (NOLOCK) "
                SqlTxt &= " INNER JOIN TCATUToken UT(NOLOCK) "
                SqlTxt &= " 	ON U.USRID = UT.USRID "
                SqlTxt &= " WHERE UT.UToken = @UToken  "
                SqlTxt &= "     AND UT.UTokenTimeOut >= @NowTime "
                SqlTxt &= "  "

                oUser = Conn.QueryFirst(Of UserInfo)(SqlTxt, New With {.UToken = UToken, .NowTime = NowTime})

                '判斷是否有內容
                If oUser Is Nothing Or oUser.USRID = "" Then
                    Throw New Exception("無相關資料，或者資料已經逾時")
                End If

                '取得Token逾時分鐘數
                Dim TimeoutMinute As Integer = 20

                '計算UToken Timeout時間
                Dim UTokenTime As String = Format(DateAdd(DateInterval.Minute, TimeoutMinute, Now), "yyyyMMddHHmmss")

                '維護UToken Timeout時間
                SqlTxt = ""
                SqlTxt &= " UPDATE TCATUToken "
                SqlTxt &= " SET UTokenTimeOut = @TokenTimeOut "
                SqlTxt &= "     , [LastInTime] = @NowTime "
                SqlTxt &= " WHERE UToken = @UToken "
                Conn.Execute(SqlTxt, New With {.TokenTimeOut = UTokenTime, .NowTime = NowTime, .UToken = UToken})

            End Using
            Return oUser

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function
End Class
