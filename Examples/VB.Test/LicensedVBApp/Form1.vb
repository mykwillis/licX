Public Class Form1
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents LicXLicenseComponent1 As licX.LicXLicenseComponent
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.LicXLicenseComponent1 = New licX.LicXLicenseComponent()
        '
        'LicXLicenseComponent1
        '
        Me.LicXLicenseComponent1.ContactUrl = "http://www.mycompany.com"
        Me.LicXLicenseComponent1.ExpirationDate = New Date(CType(0, Long))
        Me.LicXLicenseComponent1.ProductName = "My Product Name"
        Me.LicXLicenseComponent1.VendorName = "CompanyName"
        '
        'Form1
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(292, 266)
        Me.Name = "Form1"
        Me.Text = "Form1"

    End Sub

#End Region

    Private Sub LicXLicenseComponent1_EvaluationLicenseUsed( _
                ByVal sender As Object, _
                ByVal e As licX.EvaluationLicenseUsedEventArgs) _
                Handles LicXLicenseComponent1.EvaluationLicenseUsed
        System.Windows.Forms.MessageBox.Show("Hello", "Evaluation License Used")
    End Sub
End Class
