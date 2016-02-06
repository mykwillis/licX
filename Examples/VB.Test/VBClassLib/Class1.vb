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
        licControl.MasterLicensePassword = "Development_PW"

        license = System.ComponentModel.LicenseManager.Validate(Me.GetType(), Me)

        If license.IsEvaluation Then
            'Something
            EvaluationLableVisable = True
        Else
            'Something
            EvaluationLableVisable = False
        End If


    End Sub
End Class
