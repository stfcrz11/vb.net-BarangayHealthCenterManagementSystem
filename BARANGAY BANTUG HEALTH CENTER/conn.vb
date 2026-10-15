Imports MySql.Data.MySqlClient
Module conn

    Public adding As Boolean = False
    Public updating As Boolean = False
    Public id As Integer = Nothing
    Public cid As Integer = Nothing

    Public conn As New MySqlConnection
    Public cmd As New MySqlCommand
    Public da As New MySqlDataAdapter
    Public ds As New DataSet
    Public desc As String = Nothing

    Public Class CurrentUser
        Public Shared ID As Integer
        Public Shared Name As String
        Public Shared Role As String
        Public Shared Status As String
    End Class

    Public Sub LogAction(tableName As String, action As String, details As String)
        Dim log As String = "INSERT INTO systemlog (userid, tablename, action, details) VALUES (" & _
                            CurrentUser.ID & ", '" & tableName & "', '" & action & "', '" & details & "')"
        SetQuery(log)
    End Sub

    Public Function Connect(Server As String, Database As String, Port As String, Username As String, Password As String) As Boolean
        Try
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If

            conn.ConnectionString = "Server=" & Server & ";Port=" & Port & ";Database=" & Database & ";Username=" & Username & ";Password=" & Password
            conn.Open()
            Return True
        Catch ex As Exception
            MsgBox("Error connecting to database: " & ex.Message, MsgBoxStyle.Critical)
            Return False
        End Try
    End Function

    Public Sub GetQuery(Query As String, Table As String)
        Try

            If conn.State <> ConnectionState.Open Then
                If Not Connect("localhost", "bhcims", "3306", "root", "") Then
                    MsgBox("Could not establish database connection", MsgBoxStyle.Critical)
                    Return
                End If
            End If

            cmd = New MySqlCommand(Query, conn)
            da = New MySqlDataAdapter(cmd)
            ds.Clear()
            da.Fill(ds, Table)

        Catch ex As Exception
            MsgBox("Database Error: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub


    Public Sub SetQuery(Query As String)
        Try

            If conn.State <> ConnectionState.Open Then
                If Not Connect("localhost", "bhcims", "3306", "root", "") Then
                    MsgBox("Could not establish database connection", MsgBoxStyle.Critical)
                    Return
                End If
            End If

            cmd = New MySqlCommand(Query, conn)
            cmd.ExecuteNonQuery()

        Catch ex As Exception
            MsgBox("Database Error: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Public Function GetScalarValue(query As String) As Object
        Dim result As Object = Nothing
        Try
            If conn.State = ConnectionState.Closed Then
                conn.Open()
            End If

            Using cmd As New MySqlCommand(query, conn)
                result = cmd.ExecuteScalar()
            End Using
        Catch ex As Exception
            Throw ex
        End Try
        Return If(result Is Nothing OrElse Convert.IsDBNull(result), 0, result)
    End Function

    Public Sub AllowOnlyNumericContact(txt As TextBox, e As KeyPressEventArgs)
        Try
            If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) Then
                e.Handled = True
            End If

            If txt.Text.Length >= 11 AndAlso Not Char.IsControl(e.KeyChar) Then
                e.Handled = True
            End If
        Catch ex As Exception
            MsgBox("Error validating contact input: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Public Function GenerateSecureHash(input As String) As String
        Using sha256 As Security.Cryptography.SHA256 = Security.Cryptography.SHA256.Create()
            Dim bytes As Byte() = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(input))
            Dim builder As New System.Text.StringBuilder()

            For i As Integer = 0 To bytes.Length - 1
                builder.Append(bytes(i).ToString("x2"))
            Next

            Return builder.ToString()
        End Using
    End Function

    Public dtResidents As DataTable = Nothing
    Public dtPregnantMasterlist As DataTable = Nothing
    Public dtPregnantDetails As DataTable = Nothing
    Public dtMUAC As DataTable = Nothing
    Public dtSenior As DataTable = Nothing

End Module