using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace licX
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	[LicenseProvider(typeof(licX.LicXLicenseProvider))]
	public class KeyGenWin : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.RadioButton retailButton;
		private System.Windows.Forms.RadioButton evaluationButton;
		private System.Windows.Forms.NumericUpDown evaluationDays;
		private System.Windows.Forms.NumericUpDown serialNumberCount;
		private System.Windows.Forms.TextBox licenseKeyTextBox;
		private System.Windows.Forms.ComboBox passwordTextBox;
		private System.Windows.Forms.Button copyToClipboardButton;
		private System.Windows.Forms.Label evaluationDaysText;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
        
        private licX.LicXLicense license;
        private System.Windows.Forms.NumericUpDown startingSerialNumber;
        private System.Windows.Forms.DateTimePicker dateTimePicker;
        private System.Windows.Forms.CheckBox expiresCheckBox;
        private licX.LicXLicenseComponent licXLicenseComponent2;
        private licX.LicXLicenseComponent licenseComponent;

		public KeyGenWin()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

            licenseComponent = new licX.LicXLicenseComponent();
            licenseComponent.ContactUrl             = "http://www.wanderlust-software.com";
            licenseComponent.MasterLicensePassword  = "licXXcil";
            licenseComponent.ProductName            = "License Express(tm) for .NET";
            licenseComponent.VendorName             = "Wanderlust Software, LLC";
            licenseComponent.NagDuringEvaluation    = false;

            // Validate the licX license.
           
            license = (licX.LicXLicense) LicenseManager.Validate(this.GetType(), this);            
            

//#if TempTakeoutToDebug
            if ( license.IsEvaluation )
            {   
                MessageBox.Show( 
                    "Because you have an evaluation version of License Express (tm), this utility " +
                    "will only generate evaluation license keys with a maximum " +
                    "expiration time of 2 days.  Visit http://www.wanderlust-software.com " +
                    "to purchase a retail license which provides full functionality.",
                    "Evaluation License Notice"
                    );

                evaluationButton.Checked = true;
                evaluationDays.Enabled = true;
                evaluationDays.Value = 2;
                evaluationDays.Maximum = 2;

                retailButton.Checked = false;
                retailButton.Enabled = false;                
                dateTimePicker.Enabled = false;
                expiresCheckBox.Enabled = false;
            }
// #endif

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
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(KeyGenWin));
            this.retailButton = new System.Windows.Forms.RadioButton();
            this.evaluationButton = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.expiresCheckBox = new System.Windows.Forms.CheckBox();
            this.dateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.evaluationDaysText = new System.Windows.Forms.Label();
            this.evaluationDays = new System.Windows.Forms.NumericUpDown();
            this.serialNumberCount = new System.Windows.Forms.NumericUpDown();
            this.licenseKeyTextBox = new System.Windows.Forms.TextBox();
            this.passwordTextBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.copyToClipboardButton = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.startingSerialNumber = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.licXLicenseComponent2 = new licX.LicXLicenseComponent();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.evaluationDays)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.serialNumberCount)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.startingSerialNumber)).BeginInit();
            this.SuspendLayout();
            // 
            // retailButton
            // 
            this.retailButton.Location = new System.Drawing.Point(26, 32);
            this.retailButton.Name = "retailButton";
            this.retailButton.Size = new System.Drawing.Size(128, 23);
            this.retailButton.TabIndex = 0;
            this.retailButton.TabStop = true;
            this.retailButton.Text = "Re&tail";
            this.retailButton.CheckedChanged += new System.EventHandler(this.retailButton_CheckedChanged);
            // 
            // evaluationButton
            // 
            this.evaluationButton.Checked = true;
            this.evaluationButton.Location = new System.Drawing.Point(26, 117);
            this.evaluationButton.Name = "evaluationButton";
            this.evaluationButton.Size = new System.Drawing.Size(128, 23);
            this.evaluationButton.TabIndex = 1;
            this.evaluationButton.TabStop = true;
            this.evaluationButton.Text = "&Evaluation";
            this.evaluationButton.CheckedChanged += new System.EventHandler(this.evaluationButton_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.expiresCheckBox);
            this.groupBox1.Controls.Add(this.dateTimePicker);
            this.groupBox1.Controls.Add(this.evaluationDaysText);
            this.groupBox1.Controls.Add(this.evaluationDays);
            this.groupBox1.Controls.Add(this.retailButton);
            this.groupBox1.Controls.Add(this.evaluationButton);
            this.groupBox1.Location = new System.Drawing.Point(26, 58);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(422, 164);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "License Type";
            // 
            // expiresCheckBox
            // 
            this.expiresCheckBox.Location = new System.Drawing.Point(51, 58);
            this.expiresCheckBox.Name = "expiresCheckBox";
            this.expiresCheckBox.Size = new System.Drawing.Size(103, 36);
            this.expiresCheckBox.TabIndex = 2;
            this.expiresCheckBox.Text = "Expires";
            this.expiresCheckBox.CheckedChanged += new System.EventHandler(this.expiresCheckBox_CheckedChanged);
            // 
            // dateTimePicker
            // 
            this.dateTimePicker.CustomFormat = "MMM d, yyyy";
            this.dateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker.Location = new System.Drawing.Point(154, 58);
            this.dateTimePicker.MinDate = new System.DateTime(2003, 10, 2, 0, 0, 0, 0);
            this.dateTimePicker.Name = "dateTimePicker";
            this.dateTimePicker.Size = new System.Drawing.Size(192, 26);
            this.dateTimePicker.TabIndex = 3;
            this.dateTimePicker.ValueChanged += new System.EventHandler(this.SomethingChanged);
            // 
            // evaluationDaysText
            // 
            this.evaluationDaysText.Enabled = false;
            this.evaluationDaysText.Location = new System.Drawing.Point(230, 117);
            this.evaluationDaysText.Name = "evaluationDaysText";
            this.evaluationDaysText.Size = new System.Drawing.Size(52, 23);
            this.evaluationDaysText.TabIndex = 5;
            this.evaluationDaysText.Text = "days";
            // 
            // evaluationDays
            // 
            this.evaluationDays.Location = new System.Drawing.Point(154, 117);
            this.evaluationDays.Maximum = new System.Decimal(new int[] {
                                                                           365,
                                                                           0,
                                                                           0,
                                                                           0});
            this.evaluationDays.Name = "evaluationDays";
            this.evaluationDays.Size = new System.Drawing.Size(64, 26);
            this.evaluationDays.TabIndex = 4;
            this.evaluationDays.Value = new System.Decimal(new int[] {
                                                                         2,
                                                                         0,
                                                                         0,
                                                                         0});
            this.evaluationDays.Validating += new System.ComponentModel.CancelEventHandler(this.evaluationDays_Validating);
            this.evaluationDays.ValueChanged += new System.EventHandler(this.SomethingChanged);
            // 
            // serialNumberCount
            // 
            this.serialNumberCount.Location = new System.Drawing.Point(90, 94);
            this.serialNumberCount.Maximum = new System.Decimal(new int[] {
                                                                              1000,
                                                                              0,
                                                                              0,
                                                                              0});
            this.serialNumberCount.Name = "serialNumberCount";
            this.serialNumberCount.Size = new System.Drawing.Size(89, 26);
            this.serialNumberCount.TabIndex = 3;
            this.serialNumberCount.Value = new System.Decimal(new int[] {
                                                                            1,
                                                                            0,
                                                                            0,
                                                                            0});
            this.serialNumberCount.Validating += new System.ComponentModel.CancelEventHandler(this.serialNumberCount_Validating);
            this.serialNumberCount.ValueChanged += new System.EventHandler(this.SomethingChanged);
            // 
            // licenseKeyTextBox
            // 
            this.licenseKeyTextBox.AllowDrop = true;
            this.licenseKeyTextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.licenseKeyTextBox.Font = new System.Drawing.Font("Courier New", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.licenseKeyTextBox.Location = new System.Drawing.Point(26, 281);
            this.licenseKeyTextBox.Multiline = true;
            this.licenseKeyTextBox.Name = "licenseKeyTextBox";
            this.licenseKeyTextBox.ReadOnly = true;
            this.licenseKeyTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.licenseKeyTextBox.Size = new System.Drawing.Size(665, 81);
            this.licenseKeyTextBox.TabIndex = 5;
            this.licenseKeyTextBox.Text = "";
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.Location = new System.Drawing.Point(154, 12);
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.Size = new System.Drawing.Size(332, 28);
            this.passwordTextBox.TabIndex = 1;
            this.passwordTextBox.Text = "Default Master License Password";
            this.passwordTextBox.TextChanged += new System.EventHandler(this.SomethingChanged);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(26, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 23);
            this.label2.TabIndex = 0;
            this.label2.Text = "&Password:";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(13, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 34);
            this.label3.TabIndex = 0;
            this.label3.Text = "&Start:";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(13, 94);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 33);
            this.label4.TabIndex = 2;
            this.label4.Text = "C&ount:";
            // 
            // copyToClipboardButton
            // 
            this.copyToClipboardButton.Location = new System.Drawing.Point(486, 374);
            this.copyToClipboardButton.Name = "copyToClipboardButton";
            this.copyToClipboardButton.Size = new System.Drawing.Size(192, 34);
            this.copyToClipboardButton.TabIndex = 6;
            this.copyToClipboardButton.Text = "&Copy to clipboard";
            this.copyToClipboardButton.Click += new System.EventHandler(this.copyToClipboardButton_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.startingSerialNumber);
            this.groupBox2.Controls.Add(this.serialNumberCount);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Location = new System.Drawing.Point(461, 58);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(230, 164);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Serial Numbers";
            // 
            // startingSerialNumber
            // 
            this.startingSerialNumber.Location = new System.Drawing.Point(90, 35);
            this.startingSerialNumber.Maximum = new System.Decimal(new int[] {
                                                                                 99999999,
                                                                                 0,
                                                                                 0,
                                                                                 0});
            this.startingSerialNumber.Name = "startingSerialNumber";
            this.startingSerialNumber.Size = new System.Drawing.Size(89, 26);
            this.startingSerialNumber.TabIndex = 1;
            this.startingSerialNumber.Value = new System.Decimal(new int[] {
                                                                               1000,
                                                                               0,
                                                                               0,
                                                                               0});
            this.startingSerialNumber.Validating += new System.ComponentModel.CancelEventHandler(this.startingSerialNumber_Validating);
            this.startingSerialNumber.ValueChanged += new System.EventHandler(this.SomethingChanged);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(26, 246);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(230, 23);
            this.label5.TabIndex = 3;
            this.label5.Text = "Generated license keys:";
            // 
            // licXLicenseComponent2
            // 
            this.licXLicenseComponent2.ContactUrl = "www.Wanderlust-Software.com";
            this.licXLicenseComponent2.ExpirationDate = new System.DateTime(((long)(0)));
            this.licXLicenseComponent2.MasterLicensePassword = "licXXcil";
            this.licXLicenseComponent2.ProductName = "KeyGenWin";
            this.licXLicenseComponent2.VendorName = "Wanderlust Software, LLC";
            // 
            // KeyGenWin
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(8, 19);
            this.ClientSize = new System.Drawing.Size(714, 444);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.copyToClipboardButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.passwordTextBox);
            this.Controls.Add(this.licenseKeyTextBox);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(720, 488);
            this.MinimumSize = new System.Drawing.Size(720, 488);
            this.Name = "KeyGenWin";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "License Express(tm) Key Generator";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.KeyGenWin_Closing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.evaluationDays)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.serialNumberCount)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.startingSerialNumber)).EndInit();
            this.ResumeLayout(false);

        }
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
            try
            {
                Application.Run(new KeyGenWin());
            }
            catch( LicenseException)
            {               
                Application.Exit();
            }
                       
            
		}

		string[] licenseStrings;

		void GenerateLicenseStrings()
		{   
			int currentSerialNumber = (int)startingSerialNumber.Value;

			licenseStrings = new string[(int)serialNumberCount.Value];
			for(int i=0;i<serialNumberCount.Value;i++)
			{
				licX.LicenseKey licenseKey = new licX.LicenseKey();
				if ( evaluationButton.Checked )
				{
					licenseKey.Type = licX.LicenseKeyType.DesignTimeEvaluation;
					licenseKey.EvaluationDays = (int)evaluationDays.Value;
                    licenseKey.AbsoluteExpirationDate = false;
				}
				else
				{
					licenseKey.Type = licX.LicenseKeyType.DesignTimeRetail;
					licenseKey.EvaluationDays = 0;
                    if ( expiresCheckBox.Checked == true )
                    {
                        licenseKey.Type2 = licX.LicenseKeyType.DesignTimeRetail;
                        licenseKey.Type = licX.LicenseKeyType.Version2;
                        licenseKey.Version = 2;
                        licenseKey.ExpirationDate = dateTimePicker.Value;
                        licenseKey.AbsoluteExpirationDate = true;
                    }
				}
				
				licenseKey.SerialNumber = currentSerialNumber;
				licenseKey.Hash = licenseKey.CalculateHash( passwordTextBox.Text );

				licenseStrings[i] = licenseKey.ToString();

				currentSerialNumber++;
			}
		}

		private void Form1_Load(object sender, System.EventArgs e)
		{
            Microsoft.Win32.RegistryKey regKey = null;

            int evaluationChecked = 1;
            int expiresCheckBoxChecked = 0;
            string dateSaved = "";

            try
            {

                regKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey( "Software\\Wanderlust\\KeyGenWin", true );
                if ( regKey != null )
                {
                    passwordTextBox.Text        = (string)  regKey.GetValue( "MasterLicensePassword" );
                    startingSerialNumber.Value  = (int)     regKey.GetValue( "SerialNumberStart" );
                    serialNumberCount.Value     = (int)     regKey.GetValue( "SerialNumberCount" );
                    evaluationDays.Value        = (int)     regKey.GetValue( "EvaluationDays" );
                    evaluationChecked           = (int)     regKey.GetValue( "Evaluation" );
                    expiresCheckBoxChecked      = (int)     regKey.GetValue( "Expires" );
                    dateSaved                   = (string)  regKey.GetValue( "DateTimePicker" );                                
                }
            }
            catch(Exception)
            {
                // swallow any access denied or other exceptions.
            }
            
            //
            // If they used a non-eval licX license, enable them to use
            // full functionality.  If an licX eval licensed is used, then 
            // the defaults would be set in constructor.
            //

            if ( retailButton.Enabled )
            {
                evaluationButton.Checked = (evaluationChecked != 0);
                retailButton.Checked     = !evaluationButton.Checked;
                expiresCheckBox.Enabled  = !evaluationButton.Checked;
                expiresCheckBox.Checked  = (expiresCheckBoxChecked != 0);
                dateTimePicker.Enabled   = !evaluationButton.Checked && expiresCheckBox.Checked;
                
                //
                // Check if the date in the registry has allready passed.
                // If so, show the current date
                //

                int compareValue = -1;
                if ( dateSaved != "")
                {
                    dateTimePicker.Value = DateTime.Parse (dateSaved);
                    compareValue         = dateTimePicker.Value.CompareTo( System.DateTime.Now );
                    if ( compareValue < 0 ) 
                        dateTimePicker.Value = System.DateTime.Now;       
                }
                else dateTimePicker.Value = System.DateTime.Now;            
            }
                       
                        
            if ( regKey != null )
                regKey.Close(); 

			UpdateLicenseKeys();
		}

		private void UpdateLicenseKeys()
		{
			string licenseKeys="";

            GenerateLicenseStrings();
			
			foreach(string s in licenseStrings)
			{
				licenseKeys += s;
				licenseKeys += "\r\n";
			}

			licenseKeyTextBox.Text = licenseKeys;
		}

		private void copyToClipboardButton_Click(object sender, System.EventArgs e)
		{
			Clipboard.SetDataObject( licenseKeyTextBox.Text, true );
		}

		private void SomethingChanged(object sender, System.EventArgs e)
		{            
			UpdateLicenseKeys();
		}

		private void evaluationButton_CheckedChanged(object sender, System.EventArgs e)
		{		
			evaluationDays.Enabled = evaluationButton.Checked;
			evaluationDaysText.Enabled = evaluationButton.Checked;            
			UpdateLicenseKeys();
		}

        private void expiresCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            dateTimePicker.Enabled =  expiresCheckBox.Checked && expiresCheckBox.Enabled;            
            UpdateLicenseKeys();
        }

        private void dateTimePicker_ValueChanged(object sender, System.EventArgs e)
        {
            UpdateLicenseKeys();
        }

        private void retailButton_CheckedChanged(object sender, System.EventArgs e)
        {              
            expiresCheckBox.Enabled = retailButton.Checked; 
            dateTimePicker.Enabled = expiresCheckBox.Checked && expiresCheckBox.Enabled;
            UpdateLicenseKeys();
        }

        private void KeyGenWin_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Microsoft.Win32.RegistryKey regKey = null;

            try
            {
                regKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey( "Software\\Wanderlust\\KeyGenWin" );
                regKey.SetValue( "MasterLicensePassword", passwordTextBox.Text );
                regKey.SetValue( "SerialNumberStart", (int)startingSerialNumber.Value);
                regKey.SetValue( "SerialNumberCount", (int)serialNumberCount.Value );
                regKey.SetValue( "EvaluationDays", (int)evaluationDays.Value );
                regKey.SetValue( "Evaluation", (int) (evaluationButton.Checked ? 1 : 0));
                regKey.SetValue( "Expires", (int)(expiresCheckBox.Checked ? 1 : 0));
                regKey.SetValue( "DateTimePicker", dateTimePicker.Value.ToString() );
            }
            catch(Exception)
            {
                // swallow any access denied or other exceptions.
            }            
            if ( regKey != null ) regKey.Close();            
        }

        
        private void serialNumberCount_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {            
            // MessageBox.Show("Serial Number Validating Event " /* + serialNumberCount.Value */ );        
            
            //
            // The line below ensures that any change in the text field is registered by
            // forcing the NumericUpDown.Value to be read.
            //
            // It is a hack to fix the problem when a number is changed by typing (not using 
            // UpDown arrows) and the focus is changed without first pressing return.
            // When changing focus, the Validating event is automatically triggered, but 
            // will not read from the value, thinking it hasn't been changed. ( Maybe a dirty bit
            // is never set or local memory cache is referenced instead?) No change means
            // that the ValueChanged event is never triggered.  The change will not be noticed until
            // the value is explicitly read, which will trigger the ValueChanged event.  

            int i = (int) serialNumberCount.Value;
        }

        private void serialNumberCount_ValueChanged(object sender, System.EventArgs e)
        {            
            UpdateLicenseKeys();
        }                
        
        private void startingSerialNumber_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //
            // The line below ensures that any change in the text field is registered by
            // forcing the NumericUpDown.Value to be read.
            //
            // See serialNumberCount_Validating for explanation

            int i = (int)startingSerialNumber.Value;
        }

        private void evaluationDays_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //
            // The line below ensures that any change in the text field is registered by
            // forcing the NumericUpDown.Value to be read.
            //
            // See serialNumberCount_Validating for explanation
            //

            int i = (int) evaluationDays.Value;
        
        }
	}
}
