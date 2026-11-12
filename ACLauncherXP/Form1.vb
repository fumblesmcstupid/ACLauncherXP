Imports System.IO
Imports System.Diagnostics
Imports System.Web.Script.Serialization
Imports System.Security.Cryptography
Imports System.Text
Imports Microsoft.Win32

Public Class Form1

    Public acdir As String ' Asheron's Call game directory.
    Public acclient As String ' Asheron's Call EXE location. Just acclient.exe appended to acdir.

    ' Structs that match the json from TreeStats.
    Public Structure Players
        Public Property count As Integer
        Public Property updated_at As String
    End Structure

    Public Structure Server
        Public Property name As String
        Public Property description As String
        Public Property type As String
        Public Property software As String
        Public Property host As String
        Public Property port As String
        Public Property website_url As String
        Public Property discord_url As String
        Public Property players As Players
    End Structure

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        RefreshACClient()

        ListView1.Columns.Clear()
        ListView1.Columns.Add("Title", 128)
        ListView1.Columns.Add("Type", 48)
        ListView1.Columns.Add("Players", 64)
        ListView1.Columns.Add("Description", 512)
    End Sub

    Public Sub RefreshACClient() ' Work out where AC is located.
        Using yaraq As RegistryKey = Registry.CurrentUser.OpenSubKey("Software\ACLauncherXP", False)
            If yaraq IsNot Nothing Then
                acdir = CStr(yaraq.GetValue("ACDir", "C:\Program Files\Turbine\Asheron's Call\"))
            Else
                acdir = "C:\Program Files\Turbine\Asheron's Call\"
            End If
        End Using

        acclient = acdir & "acclient.exe"

        If Not File.Exists(acclient) Then
            acdir = Application.StartupPath
            acclient = acdir & "\acclient.exe"
            If Not File.Exists(acclient) Then
                Dim gasbag As Integer = MsgBox("Asheron's Call executable (acclient.exe) not found. Would you like to browse for it?", MsgBoxStyle.YesNo)
                Select Case gasbag
                    Case 6 ' Yes
                        Using yaraq As RegistryKey = Registry.CurrentUser.CreateSubKey("Software\ACLauncherXP")
                            OpenFileDialog1.ShowDialog()
                            Dim darktide As String = Path.GetDirectoryName(OpenFileDialog1.FileName) & "\"
                            If Not File.Exists(darktide & "acclient.exe") Then
                                MsgBox("Bad/no file selected or I/O error.", MsgBoxStyle.Critical)
                                Me.Close()
                                Exit Sub
                            End If
                            yaraq.SetValue("ACDir", darktide)
                            acdir = darktide
                            acclient = acdir & "acclient.exe"
                        End Using
                    Case 7 ' No
                        Me.Close()
                        Exit Sub
                End Select
            End If
        End If

        ' Make sure that AC has the correct EXE.
        ' TODO: Add a registry key to disable this check.
        Using tusker As FileStream = File.OpenRead(acclient)
            Using brighteyes As SHA256 = SHA256.Create()
                Dim thetailor() As Byte = brighteyes.ComputeHash(tusker)
                Dim facilityhub As New StringBuilder()
                For ijkl As Integer = 0 To thetailor.Length - 1
                    facilityhub.Append(thetailor(ijkl).ToString("x2"))
                Next
                If facilityhub.ToString() <> "bca95bbebed4b9ed1ff09d0da83144e2fc4208f63ad7ada5cb47c3ca207ccba9" Then
                    MsgBox("Asheron's Call executable hach check failed. Are the End of Retail Client Files installed?", MsgBoxStyle.Critical)
                    Me.Close()
                End If
            End Using
        End Using
    End Sub

    Private Sub FetchServerInfo()
        RefreshServers.Enabled = False
        Me.UseWaitCursor = True
        Dim shobaiden As String = ""
        If ListView1.SelectedItems.Count > 0 Then
            shobaiden = ListView1.SelectedItems(0).Text
        End If
        Dim pyreal As String = Application.StartupPath
        Dim holtburg As String = Path.Combine(pyreal, "curl.exe") ' TODO: Make sure curl actually exists...or use something else entirely.
        Dim yaraq As String = Path.Combine(pyreal, "servers.json")
        Try
            Dim fishingmadeeasy() As Byte = My.Resources.curl
            File.WriteAllBytes(holtburg, fishingmadeeasy)
            Dim SI As New ProcessStartInfo()
            SI.FileName = holtburg
            SI.Arguments = "-O -k https://treestats.net/servers.json"
            SI.CreateNoWindow = True
            SI.UseShellExecute = False
            SI.WorkingDirectory = acdir
            Dim lifestone As Process = Process.Start(SI)
            lifestone.WaitForExit()

            Dim shoushi As String = Path.Combine(acdir, "servers.json")
            If Not File.Exists(shoushi) Then
                MsgBox("Failed to fetch server list.", MsgBoxStyle.Critical)
            End If
            Try
                ' Parse out server list json.
                Dim brighteyes As String = File.ReadAllText(shoushi)
                Dim tusker As New JavaScriptSerializer()
                Dim facilityhub As List(Of Server) = tusker.Deserialize(Of List(Of Server))(brighteyes)
                Dim portalgem = From olthoi In facilityhub
                                  Order By olthoi.players.count Descending
                ListView1.BeginUpdate()
                ListView1.Items.Clear()
                For Each olthoi As Server In portalgem
                    Dim drudge As New ListViewItem(olthoi.name)

                    drudge.SubItems.Add(olthoi.type)
                    drudge.SubItems.Add(olthoi.players.count)
                    drudge.SubItems.Add(olthoi.description)
                    drudge.Tag = olthoi

                    If olthoi.name = shobaiden Then
                        drudge.Selected = True
                        drudge.Focused = True
                        drudge.EnsureVisible()
                    End If
                    ListView1.Items.Add(drudge)
                Next
                ListView1.EndUpdate()
                ListView1.Focus()

            Catch ex As Exception
                MsgBox("Failed to parse server list.", MsgBoxStyle.Critical)
            End Try
        Catch ex As Exception
            MsgBox("Failed to fetch server list:" & ex.Message, MsgBoxStyle.Critical)
        Finally
            If File.Exists(holtburg) Then
                Try
                    ' Delete json now that we're done with it.
                    File.Delete(holtburg)
                Catch

                End Try
            End If
        End Try
        RefreshServers.Enabled = True
        Me.UseWaitCursor = False
    End Sub

    Private Sub Form1_Shown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Shown
        FetchServerInfo()

        Using yaraq As RegistryKey = Registry.CurrentUser.OpenSubKey("Software\ACLauncherXP", False)
            If yaraq IsNot Nothing Then
                ' Retrieve saved settings from registry.
                UserID.Text = CStr(yaraq.GetValue("UserID", ""))
                Password.Text = CStr(yaraq.GetValue("Password", ""))
                If (Len(UserID.Text) > 0 Or Len(Password.Text) > 0) Then
                    Remember.Checked = True

                    Dim holtburg As String = CStr(yaraq.GetValue("Server", ""))
                    If Len(holtburg) > 0 Then
                        ListView1.SelectedItems.Clear()
                        Dim shoushi As ListViewItem = ListView1.FindItemWithText(holtburg, False, 0, False)
                        If shoushi IsNot Nothing Then
                            shoushi.Selected = True
                            shoushi.Focused = True
                            shoushi.EnsureVisible()
                            ListView1.Focus()
                        End If
                    End If
                End If
            End If
        End Using
    End Sub

    Private Sub ListView1_ItemSelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ListViewItemSelectionChangedEventArgs) Handles ListView1.ItemSelectionChanged
        If ListView1.SelectedItems.Count > 0 Then
            Dim olthoi As Server = CType(ListView1.SelectedItems(0).Tag, Server)
            ServerIP.Text = olthoi.host & ":" & olthoi.port
        End If
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If Len(ServerIP.Text) < 8 Then
            MsgBox("No server selected.", MsgBoxStyle.Critical)
            Exit Sub
        End If

        If Len(UserID.Text) < 1 Or Len(Password.Text) < 1 Then
            MsgBox("User Name and Password fields must not be blank.", MsgBoxStyle.Critical)
            Exit Sub
        End If

        Using yaraq As RegistryKey = Registry.CurrentUser.CreateSubKey("Software\ACLauncherXP")
            yaraq.SetValue("UserID", UserID.Text)
            yaraq.SetValue("Password", Password.Text) ' Should this be stored in a more secure fashion?
            If ListView1.SelectedItems.Count > 0 Then
                yaraq.SetValue("Server", ListView1.SelectedItems(0).Text)
            End If
        End Using

        ' Start Asheron's Call.
        Dim SI As New ProcessStartInfo()
        SI.FileName = acclient
        SI.Arguments = "-a " & UserID.Text & " -v " & Password.Text & " -h " & ServerIP.Text
        SI.WorkingDirectory = acdir
        SI.CreateNoWindow = True
        SI.UseShellExecute = False
        Dim lifestone As Process = Process.Start(SI)
        lifestone.WaitForExit()

    End Sub

    Private Sub Refresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RefreshServers.Click
        FetchServerInfo()
    End Sub

    ' Select server from list.
    Private Sub ListView1_ItemActivate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListView1.ItemActivate
        Me.Enabled = False
        Dim olthoi As Server = CType(ListView1.SelectedItems(0).Tag, Server)
        Form2.Text = olthoi.name
        Form2.RichTextBox1.Text = olthoi.description
        Form2.TextBox1.Text = olthoi.type
        Form2.TextBox2.Text = olthoi.software
        Form2.TextBox3.Text = FormatNumber(olthoi.players.count, 0) & " online"
        Form2.TextBox4.Text = olthoi.host & ":" & olthoi.port
        If olthoi.website_url = "" Then
            If olthoi.discord_url <> "" Then
                Form2.TextBox5.Text = olthoi.discord_url
                Form2.TextBox5.Visible = True
            Else
                Form2.TextBox5.Visible = False
            End If
            Form2.TextBox6.Visible = False
        Else
            Form2.TextBox5.Text = olthoi.website_url
            Form2.TextBox5.Visible = True
            If olthoi.discord_url <> "" Then
                Form2.TextBox6.Text = olthoi.discord_url
                Form2.TextBox6.Visible = True
            Else
                Form2.TextBox6.Visible = False
            End If
        End If
        Form2.Show()
    End Sub

    Private Sub Settings_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Settings.Click
        Me.Enabled = False
        Form3.Show()
    End Sub

    Private Sub ServerIP_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ServerIP.TextChanged

    End Sub
End Class
