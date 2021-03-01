Imports System.Net
Imports System.IO
Imports System.Text.RegularExpressions

Public Class Form1
    Dim WC_File As New WebClient
    Dim WC_String As New WebClient

    Public StartupPath As String = Application.StartupPath
    Public FileToUpdate As String

    Public Download_Link As String
    Public NewVersion As String
    Public NewDownloadLink As String

    'Fichier d'installation : planned_shutdown_setup.exe

    Public Started_Number = 0

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'WC_String.Headers.Add("user-agent", "Only a test!")
        'WC_String.Headers.Add("Content-Type", "application/x-www-form-urlencoded")
        WC_String.Headers.Add("User-Agent", "Mozilla/4.0 (Compatible; Windows NT 5.1; MSIE 6.0)")
        WC_File.Headers.Add("User-Agent", "Mozilla/4.0 (Compatible; Windows NT 5.1; MSIE 6.0)")

        Download_Link = WC_String.DownloadString("https://api.github.com/repos/MisterEvans78/Planned-shutdown/releases/latest")
        Debug.WriteLine("tag_name"":""(\d.\d.\d)"",")
        'Debug.WriteLine(Download_Link)
        Debug.WriteLine("Version : " & Regex.Match(Download_Link, "tag_name"":""(\d.\d.\d)"",").Groups.Item(1).ToString)
        NewVersion = Regex.Match(Download_Link, "tag_name"":""(.*)"",""target_").Groups.Item(1).ToString
        NewDownloadLink = Regex.Match(Download_Link, "browser_download_url"":""(.*)""}],").Groups.Item(1).ToString

        Debug.WriteLine("azeazeazazeazezaeazezae")
        Debug.WriteLine("----")
        'MsgBox(Download_Link)
        Debug.WriteLine(NewVersion)
        Debug.WriteLine(NewDownloadLink)

        FileToUpdate = $"{Path.GetTempPath}\Planned.shutdown.{NewVersion}.exe"

        'MsgBox(NewVersion)
        Continue_Loading()
    End Sub

    Private Sub Continue_Loading()
        Try
            If NewDownloadLink = "/" Then
                Dim result = MessageBox.Show("It seems that the update file is not available, please contact the support to fix this problem.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                If result = DialogResult.OK Then
                    Close()
                Else
                    Close()
                End If
            Else
                If File.Exists(FileToUpdate) Then
                    File.Delete(FileToUpdate)
                End If
                TimerUpdate.Start()
            End If
        Catch ex As Exception
            MsgBox($"Try to launch the updater with admin rights{vbNewLine}Error:{ex.Message}", MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub WC_DownloadProgressChanged(sender As Object, e As DownloadProgressChangedEventArgs)
        ProgressBar1.Value = e.ProgressPercentage

        If e.ProgressPercentage = 100 Then
            'This verification will prevent double launch
            If Started_Number = 1 Then
                TimeLaunchTheUpdatedFile.Start()
            Else
                Started_Number = 1
            End If

        End If
    End Sub
    Private Sub TimerUpdate_Tick(sender As Object, e As EventArgs) Handles TimerUpdate.Tick
        TimerUpdate.Stop()
        'MsgBox(Download_Link)
        AddHandler WC_File.DownloadProgressChanged, AddressOf WC_DownloadProgressChanged
        WC_File.DownloadFileAsync(New Uri(NewDownloadLink), FileToUpdate)
    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        Close()
    End Sub

    Private Sub TimeLaunchTheUpdatedFile_Tick(sender As Object, e As EventArgs) Handles TimeLaunchTheUpdatedFile.Tick
        TimeLaunchTheUpdatedFile.Stop()
        Process.Start(FileToUpdate)
        Close()
    End Sub
End Class
