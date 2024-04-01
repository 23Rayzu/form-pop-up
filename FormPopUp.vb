Imports DotSpatial.Controls
Imports DotSpatial.Data
Imports DotSpatial.Symbology
Imports System.IO

Public Class FormPopUp
    Public AppPath As String = Application.ExecutablePath
    Public ResourcesPath As String = AppPath.ToUpper.Replace("\Creating_Geospatial_Information_System.exe", "\Resources")
    Private lyrAdmin As MapPolygonLayer
    Private Sub cmdEdit_Click(sender As Object, e As EventArgs) Handles cmd_Edit.Click
        Try

            Form1.sedangload = True

            If cmd_Edit.Text = "Edit" Then

                Dim input As String = Microsoft.VisualBasic.Interaction.InputBox(
                            "Masukkan password", "Password", "", -1, -1)

                If input = "iniMukhlish" Then
                    txtKode.ReadOnly = False
                    txtNamaAset.ReadOnly = False
                    txtJenisAset.ReadOnly = False
                    txtAtasNama.ReadOnly = False
                    cmd_Browse.Enabled = True
                    cmd_Delete.Visible = True
                    cmd_Edit.Text = "Edit"
                Else
                    txtKode.ReadOnly = True
                    txtNamaAset.ReadOnly = True
                    txtJenisAset.ReadOnly = True
                    txtAtasNama.ReadOnly = True
                    cmd_Browse.Enabled = False
                    cmd_Delete.Visible = False
                    cmd_Edit.Text = "Edit"
                    MessageBox.Show("Password Salah")
                End If

            ElseIf cmd_Edit.Text = "Edit" Then
                Dim featureEdited As IFeature = Form1.lyrPemerintah.FeatureSet.GetFeature(CInt(txtFoto.Text))
                featureEdited.DataRow.BeginEdit()
                featureEdited.DataRow("kode") = txtKode.Text
                featureEdited.DataRow("nama_aset") = txtNamaAset.Text
                featureEdited.DataRow("jenis_aset") = txtJenisAset.Text
                featureEdited.DataRow("atas_nama") = txtAtasNama.Text
                featureEdited.DataRow("foto") = txtFoto.Text
                featureEdited.DataRow.EndEdit()

                cmd_Edit.Text = "Edit"
                txtNamaAset.ReadOnly = True
                txtJenisAset.ReadOnly = True
                cmd_Browse.Enabled = False
                Map1.Refresh()
                Me.Hide()
                MessageBox.Show("Data Saved")
            End If

            Form1.sedangload = False

        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub

    Private Sub cmdBrowse_Click(sender As Object, e As EventArgs) Handles cmd_Browse.Click
        Dim ofd As OpenFileDialog = New OpenFileDialog()
        ofd.InitialDirectory = "C:\"
        ofd.Title = "Browse Photo"
        ofd.Filter = "JPG (*.jpg)|*.jpg|JPEG (*.jpeg)|*.jpeg|PNG (*.png)|*.png|All files (*.*)|*.*"
        ofd.FilterIndex = 1
        ofd.RestoreDirectory = True

        If (ofd.ShowDialog() = DialogResult.OK) Then
            Dim fileName As String = Path.GetFileName(ofd.FileName)
            Dim sourcePath As String = Path.GetDirectoryName(ofd.FileName)
            Dim targetPath As String = Path.Combine(Form1.ResourcesPath, "\Resources\Database\Non-Spatial\")
            Dim sourceFile As String = Path.Combine(sourcePath, fileName)
            Dim destFile As String = Path.Combine(targetPath, fileName)
            File.Copy(sourceFile, destFile, True)
            txtFoto.Text = fileName
            Map1.ClearLayers()
            Map1.AddLayer(destFile)

        Else
            MessageBox.Show("Silahkan Pilih Foto!!!", "Report", MessageBoxButtons.OK)
        End If
    End Sub



    Private Sub cmdDelete_Click(sender As Object, e As EventArgs) Handles cmd_Delete.Click
        Form1.sedangload = True
        Form1.lyrPemerintah.ClearSelection()
        Form1.lyrPemerintah.Select(CInt(txtShapeIndex.Text))
        Dim strValue As String = txtShapeIndex.Text
        Form1.lyrPemerintah.RemoveSelectedFeatures()
        Form1.sedangload = False
        Form1.Map1.Refresh()
        Me.Close()
        MessageBox.Show("Alhamdulillah, data terhapus!")
    End Sub

    Private Sub cmdZoomIn_Click(sender As Object, e As EventArgs) Handles cmd_ZoomIn.Click
        Map1.FunctionMode = DotSpatial.Controls.FunctionMode.ZoomIn
    End Sub

    Private Sub cmdZoomOut_Click(sender As Object, e As EventArgs) Handles cmd_ZoomOut.Click
        Map1.FunctionMode = DotSpatial.Controls.FunctionMode.ZoomOut
    End Sub

    Private Sub Pan_Click(sender As Object, e As EventArgs) Handles cmd_Pan.Click
        Map1.FunctionMode = DotSpatial.Controls.FunctionMode.Pan
    End Sub

    Private Sub cmdFullExtent_Click(sender As Object, e As EventArgs) Handles cmd_FullExtent.Click
        Map1.ZoomToMaxExtent()
    End Sub


    Private Sub cmdCancel_Click(sender As Object, e As EventArgs) Handles cmd_Cancel.Click
        Me.Close()
    End Sub

    Private Sub Map1_Click(sender As Object, e As EventArgs) Handles Map1.Click
        lyrAdmin = Map1.Layers.Add(ResourcesPath & "\Resources\Database\Spatial\ADMINISTRASIKECAMATAN_AR_50K.shp")
    End Sub

    Private Sub FormPopUp_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class