using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace licX.LicenseInstaller
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class LicenseInstallerForm : System.Windows.Forms.Form
	{
        private System.Windows.Forms.Label instructionLabel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button installButton;
        private System.Windows.Forms.Label exampleLicenseKeyLabel;
        private System.Windows.Forms.LinkLabel wanderlustLinkLabel;
        private System.Windows.Forms.TextBox licenseKeyTextBox;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

        private string GetConfigValue(string valueName)
        {
            string s;

            try                 { s = (string) reader.GetValue(valueName, typeof(string)); }
            catch (Exception)   { s = null; }

            return s;
        }

            
		public LicenseInstallerForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//

            // Darren, why is this here?  debugging turd?
            licenseKeyFilePath = @"C:\tmp\myfile.lic";
            

            
            licenseKeyFilePath  = GetConfigValue( "licenseKeyFilePath"  );
            vendorName          = GetConfigValue( "vendorName"          );
            productName         = GetConfigValue( "productName"         );
            contactUrl          = GetConfigValue( "contactUrl"          );
            caption             = GetConfigValue( "caption"             );
            instructions        = GetConfigValue( "instructions"        );            


            //
            // Make sure we were given a license key path.  We can't really
            // check the validity, because it might not exist yet.
            //
            if ( licenseKeyFilePath == null )
            {
                MessageBox.Show( "No licenseKeyFilePath property was found in the application configuration.", "License Installer Error" );
                Application.Exit();
            }
            

            //
            // Set up the application caption.
            //
            if ( caption != null )  { this.Text = caption; }
            else if ( productName != null ) { this.Text = productName + " License Key Installer"; }
            else { /* use default */ }


            //
            // Set up the instruction text.
            //
            if ( instructions != null ) { this.instructionLabel.Text = instructions; }
            else { /* use default */ }


            try
            {
                if ( System.IO.File.Exists( licenseKeyFilePath ) )
                {
                    System.IO.StreamReader reader = System.IO.File.OpenText( licenseKeyFilePath );
                    if ( reader != null )
                    {
                        licenseKeyTextBox.Text = reader.ReadLine();
                    }
                    reader.Close();
                }
            } 
            catch (Exception)
            {
                // nothing to do; problem reading an existing .lic file.
            }
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(LicenseInstallerForm));
            this.instructionLabel = new System.Windows.Forms.Label();
            this.licenseKeyTextBox = new System.Windows.Forms.TextBox();
            this.installButton = new System.Windows.Forms.Button();
            this.exampleLicenseKeyLabel = new System.Windows.Forms.Label();
            this.wanderlustLinkLabel = new System.Windows.Forms.LinkLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // instructionLabel
            // 
            this.instructionLabel.Location = new System.Drawing.Point(26, 23);
            this.instructionLabel.Name = "instructionLabel";
            this.instructionLabel.Size = new System.Drawing.Size(640, 59);
            this.instructionLabel.TabIndex = 0;
            this.instructionLabel.Text = "Enter your product license key in the text box below, and click Install.  If a li" +
                "cense key already exists, it will be shown in the text box.";
            // 
            // licenseKeyTextBox
            // 
            this.licenseKeyTextBox.Location = new System.Drawing.Point(26, 35);
            this.licenseKeyTextBox.Name = "licenseKeyTextBox";
            this.licenseKeyTextBox.Size = new System.Drawing.Size(358, 26);
            this.licenseKeyTextBox.TabIndex = 1;
            this.licenseKeyTextBox.Text = "";
            // 
            // installButton
            // 
            this.installButton.Location = new System.Drawing.Point(486, 129);
            this.installButton.Name = "installButton";
            this.installButton.Size = new System.Drawing.Size(120, 33);
            this.installButton.TabIndex = 2;
            this.installButton.Text = "Install";
            this.installButton.Click += new System.EventHandler(this.installButton_Click);
            // 
            // exampleLicenseKeyLabel
            // 
            this.exampleLicenseKeyLabel.Location = new System.Drawing.Point(26, 82);
            this.exampleLicenseKeyLabel.Name = "exampleLicenseKeyLabel";
            this.exampleLicenseKeyLabel.Size = new System.Drawing.Size(358, 29);
            this.exampleLicenseKeyLabel.TabIndex = 5;
            this.exampleLicenseKeyLabel.Text = "e.g., 01-23-456789abcdef0123-456789ab";
            // 
            // wanderlustLinkLabel
            // 
            this.wanderlustLinkLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.wanderlustLinkLabel.LinkArea = new System.Windows.Forms.LinkArea(66, 24);
            this.wanderlustLinkLabel.Location = new System.Drawing.Point(13, 234);
            this.wanderlustLinkLabel.Name = "wanderlustLinkLabel";
            this.wanderlustLinkLabel.Size = new System.Drawing.Size(653, 23);
            this.wanderlustLinkLabel.TabIndex = 7;
            this.wanderlustLinkLabel.TabStop = true;
            this.wanderlustLinkLabel.Text = "Integrated licensing support provided by License Express(tm) from Wanderlust Soft" +
                "ware, LLC.";
            this.wanderlustLinkLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.wanderlustLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.licenseKeyTextBox);
            this.groupBox1.Controls.Add(this.exampleLicenseKeyLabel);
            this.groupBox1.Location = new System.Drawing.Point(70, 94);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(410, 116);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "License Key";
            // 
            // LicenseInstallerForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(8, 19);
            this.ClientSize = new System.Drawing.Size(681, 268);
            this.Controls.Add(this.wanderlustLinkLabel);
            this.Controls.Add(this.instructionLabel);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.installButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "LicenseInstallerForm";
            this.Text = "License Key Installation";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new LicenseInstallerForm());
		}

        private void linkLabel1_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            wanderlustLinkLabel.Links[wanderlustLinkLabel.Links.IndexOf(e.Link)].Visited = true;         
            System.Diagnostics.Process.Start( "http://www.wanderlust-software.com" );        
        }

        private bool ValidateLicenseKeyString(string licenseKeyString)
        {
            //Darren, write this function
            return true;
        }

        private void installButton_Click(object sender, System.EventArgs e)
        {
            string licenseKeyString = licenseKeyTextBox.Text;

            //
            // Validate the license key string.  This doesn't validate that it's
            // actually valid for this product, but it checks for proper syntax
            // anyhow.
            //
            if ( !ValidateLicenseKeyString(licenseKeyString) )
            {
                MessageBox.Show(
                    "The license key you entered does not appear to be formatted correctly.  " +
                    "Please verify that you have entered all of the digits, and included the hyphens " +
                    "as shown in the example below the text box.",
                    "License Key Format Error"
                    );
                return;
            }

            //
            // Open the license key file.
            //
            System.IO.StreamWriter writer;
            try                 { writer = System.IO.File.CreateText( licenseKeyFilePath ); }
            catch(Exception)    { writer = null; }

            if (writer == null)
            {
                MessageBox.Show(
                    "The license installer could not open the license key file " +
                    "at " + licenseKeyFilePath + ".\n",
                    "Installation Error"
                    );
                return;
            }

            try { writer.WriteLine( licenseKeyString ); }
            catch(Exception)
            {
                MessageBox.Show( "The license key could not be written.", "Installation Error" );
                return;
            }

            writer.Close();

            MessageBox.Show( "The specified license key was installed successfully.", "License Installed" );

            return;
                
        }

        /// <summary>
        /// Complete path of the license key file (i.e., "c:\program files\MyApp\xyz.lic")
        /// </summary>
        private string licenseKeyFilePath;
        private string vendorName;
        private string productName;
        private string contactUrl;
        private string caption;
        private string instructions;

        System.Configuration.AppSettingsReader reader = new System.Configuration.AppSettingsReader();
	}
}
