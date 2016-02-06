using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Diagnostics;

namespace LicensedComponent
{
	/// <summary>
	/// Summary description for UserControl1.
	/// </summary>
	[LicenseProvider(typeof(licX.LicXLicenseProvider))]
	public class TestLicensedComponent : System.Windows.Forms.UserControl
	{
        private System.Windows.Forms.Label label1;
        private licX.LicXLicense licXLicense;
        private System.Windows.Forms.Label evaluationLabel;
        private licX.LicXLicenseComponent licXLicenseComponent;
        
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public TestLicensedComponent()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

            licXLicense = (licX.LicXLicense) LicenseManager.Validate(typeof(TestLicensedComponent), this);
            Trace.WriteLine( "License Information: " );
            Trace.WriteLine( "  LicenseKey: " + licXLicense.LicenseKey);
            Trace.WriteLine( "  SerialNumber: " + licXLicense.SerialNumber);
            Trace.WriteLine( "  IsEvaluation: " + licXLicense.IsEvaluation );
            Trace.WriteLine( "  EvaluationDaysRemaining: " + licXLicense.EvaluationDaysRemaining );

            if ( licXLicense.IsEvaluation )
            {
                evaluationLabel.Visible = true;
            }
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if( components != null )
					components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.label1 = new System.Windows.Forms.Label();
            this.evaluationLabel = new System.Windows.Forms.Label();
            this.licXLicenseComponent = new licX.LicXLicenseComponent();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(32, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 48);
            this.label1.TabIndex = 0;
            this.label1.Text = "I am a licensed component.";
            // 
            // evaluationLabel
            // 
            this.evaluationLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.evaluationLabel.Location = new System.Drawing.Point(16, 112);
            this.evaluationLabel.Name = "evaluationLabel";
            this.evaluationLabel.Size = new System.Drawing.Size(120, 23);
            this.evaluationLabel.TabIndex = 1;
            this.evaluationLabel.Text = "Evaluation Version";
            this.evaluationLabel.Visible = false;
            // 
            // licXLicenseComponent
            // 
            this.licXLicenseComponent.ContactUrl = "asdf";
            this.licXLicenseComponent.ExpirationDate = new System.DateTime(((long)(0)));
            this.licXLicenseComponent.MasterLicensePassword = "This is my password.";
            this.licXLicenseComponent.ProductName = "asdf";
            this.licXLicenseComponent.VendorName = "asdf";
            // 
            // TestLicensedComponent
            // 
            this.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                          this.evaluationLabel,
                                                                          this.label1});
            this.Name = "TestLicensedComponent";
            this.ResumeLayout(false);

        }
		#endregion

        private void licXLicenseComponent1_LicenseCheckFailed(object sender, System.EventArgs e)
        {
            MessageBox.Show( "This component is not licensed!" );
        }

	}
}
