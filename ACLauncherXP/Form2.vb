Imports System.Runtime.InteropServices

Public Class Form2

    ' This is all here just to remove the blinking cursor from static text boxes. Maybe there's a better solution?
    <DllImport("user32.dll")> _
    Private Shared Function HideCaret(ByVal hWnd As IntPtr) As Boolean
    End Function

    Private Sub Form2_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Form2_FormClosed(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        Form1.Enabled = True
    End Sub

    Private Sub Form2_Shown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Shown
        HideCaret(RichTextBox1.Handle)
    End Sub

    Private Sub RichTextBox1_MouseHover(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RichTextBox1.MouseHover
        HideCaret(RichTextBox1.Handle)
    End Sub

    Private Sub RichTextBox1_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RichTextBox1.Enter
        HideCaret(RichTextBox1.Handle)
    End Sub

    Private Sub Form2_MouseEnter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.MouseEnter
        HideCaret(RichTextBox1.Handle)
    End Sub

    Private Sub Form2_Activated(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Activated
        HideCaret(RichTextBox1.Handle)
    End Sub
End Class