' ***** 1st of two lines that are need to License your class library *****
<System.ComponentModel.LicenseProvider(GetType(licX.LicXLicenseProvider))> _
Public Class LicensedVBClass

    Dim EvaluationLableVisable As Boolean

    Private licControl As licX.LicXLicenseComponent
    Private license As licX.LicXLicense

    Public Sub New()

        licControl = New licX.LicXLicenseComponent()

        licControl.VendorName = "Development, Inc."
        licControl.ProductName = "Product v3.1.1"
        licControl.ContactUrl = "http://www.website.com"

        ' ***** 2nd of two lines that are need to License your class library *****
        license = System.ComponentModel.LicenseManager.Validate(Me.GetType(), Me)

        If license.IsEvaluation Then
            'You have detected that it is an Evaluation License. Do Something.  
            EvaluationLableVisable = True
        Else
            'You have detected that it is a normal License. Do nothing.
            EvaluationLableVisable = False
        End If

    End Sub
End Class
