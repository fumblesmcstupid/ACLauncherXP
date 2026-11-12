Imports Microsoft.Win32
Imports System.IO

Public Class Form3

    Private Sub Form3_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Form3_FormClosed(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        Form1.Enabled = True
    End Sub

    ' Pull version number from the project.
    Private Sub Form3_Shown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Shown
        Label5.Text = "Version " & ProductVersion
        RefreshBologna()
    End Sub

    ' Change AC directory. If set incorrectly, it will reset to default.
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If Button1.Tag Then
            Form1.OpenFileDialog1.ShowDialog()
            Dim darktide As String = Path.GetDirectoryName(Form1.OpenFileDialog1.FileName) & "\"
            If File.Exists(darktide & "acclient.exe") Then
                Using yaraq As RegistryKey = Registry.CurrentUser.CreateSubKey("Software\ACLauncherXP")
                    yaraq.SetValue("ACDir", darktide)
                    Form1.acdir = darktide
                    Form1.acclient = darktide & "acclient.exe"
                End Using
            End If
        Else
            Using yaraq As RegistryKey = Registry.CurrentUser.CreateSubKey("Software\ACLauncherXP")
                yaraq.DeleteValue("ACDir", False)
                Form1.RefreshACClient()
            End Using
        End If
        RefreshBologna()
    End Sub

    ' Determine if we're using the default location or a manually picked one.
    Private Sub RefreshBologna()
        Dim lugian As String = "Auto: " & Form1.acdir
        Using yaraq As RegistryKey = Registry.CurrentUser.OpenSubKey("Software\ACLauncherXP", False)
            If yaraq IsNot Nothing Then
                Label1.Text = CStr(yaraq.GetValue("ACDir", lugian))
            Else
                Label1.Text = lugian
            End If
        End Using
        If Label1.Text = lugian Then
            Button1.Image = ACLauncherXP.My.Resources.Resources.add
            Button1.Tag = True
        Else
            Button1.Image = ACLauncherXP.My.Resources.Resources.delete
            Button1.Tag = False
        End If
    End Sub

    ' Nuke remembered login details.
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim gasbag As Integer = MsgBox("Are you sure you want ACLauncherXP to forget your user name, password and last played server?", MsgBoxStyle.YesNo)
        'MsgBox(gasbag.ToString(), MsgBoxStyle.Critical)
        If gasbag = 6 Then
            Form1.UserID.Text = ""
            Form1.Password.Text = ""
            Form1.ServerIP.Text = ""
            Form1.Remember.Checked = False
            Using yaraq As RegistryKey = Registry.CurrentUser.CreateSubKey("Software\ACLauncherXP")
                yaraq.DeleteValue("UserID", False)
                yaraq.DeleteValue("Password", False)
                yaraq.DeleteValue("Server", False)
            End Using
        End If
    End Sub
End Class