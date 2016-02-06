'' ***** 1st of two lines to uncomment to enable license validation *****
<System.ComponentModel.LicenseProvider(GetType(licX.LicXLicenseProvider))> _
Public Class UserControl1
    Inherits System.Windows.Forms.UserControl

    '' ***** This line is needed in case we want to check for evaluation licenses
    Private license As licX.LicXLicense

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        '' ***** 2nd of two lines to uncomment to enable license validation *****
        'System.ComponentModel.LicenseManager.Validate(Me.GetType(), Me)

        ''
        ''       OR uncomment these lines to detect an evaluation license
        ''

        '' ***** 2nd of two lines to uncomment (Alternate) to enable license validation *****
        license = System.ComponentModel.LicenseManager.Validate(Me.GetType(), Me)

        '' The following code determines if an Evaluation License is being used
        '' so that you can modify the behavior of your software.
        If license.IsEvaluation Then
            ' Do something different
        Else
            ' Do Nothing.")
        End If


        'Add any initialization after the InitializeComponent() call

    End Sub

    'UserControl1 overrides dispose to clean up the component list.
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
    Friend WithEvents Label1 As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.LicXLicenseComponent1 = New licX.LicXLicenseComponent
        Me.Label1 = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'LicXLicenseComponent1
        '
        Me.LicXLicenseComponent1.ContactUrl = Nothing
        Me.LicXLicenseComponent1.ExpirationDate = New Date(CType(0, Long))
        Me.LicXLicenseComponent1.ProductName = Nothing
        Me.LicXLicenseComponent1.VendorName = Nothing
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(24, 32)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(104, 80)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "I am a control."
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'UserControl1
        '
        Me.Controls.Add(Me.Label1)
        Me.Name = "UserControl1"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub LicXLicenseComponent1_EvaluationLicenseUsed( _
                ByVal sender As Object, _
                ByVal e As licX.EvaluationLicenseUsedEventArgs) _
                Handles LicXLicenseComponent1.EvaluationLicenseUsed

        System.Windows.Forms.MessageBox.Show("Purchase My Control for full functionality!!\nThis is an example of a custom \nnagging message, which replaced the default\nnagging message.", _
                                             "Evaluation License Used")
    End Sub

End Class
