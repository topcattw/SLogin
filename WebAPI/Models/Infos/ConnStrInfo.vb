Public Class ConnStrInfo
    Private m_ConnStr As String = "Data Source=.\SqlExpress;Initial Catalog=DCAT;Integrated Security=True"

    Public ReadOnly Property ConnStr As String
        Get
            Return m_ConnStr
        End Get
    End Property
End Class
