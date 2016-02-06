using System;
using System.ComponentModel;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.ComponentModel.Design;
using System.Reflection;
using System.Diagnostics;

namespace licX
{
    /// <summary>
    /// Represents the licensing policy to use.
    /// </summary>
    /// <remarks>
    /// A LicXLicensePolicy is added to the component that is
    /// to be licensed.  The properties of the LicensePolicyComponent
    /// control the behavior of LicX licensing, such as whether or not
    /// an evaluation license is supported, what the licensing grace
    /// periods are, and so forth.
    /// </remarks>
//    [LicenseProvider(typeof(LicXLicenseProvider))]
    public class LicXLicenseComponent : Component
    {
        //
        //
        // Evaluation
        //
        //
        [Category("Evaluation")]
        [Description("Specifies whether or not \"nag\" dialog boxes should be displayed at runtime when an evaluation license key is being used.")]
        [DefaultValue(true)]
        public bool NagDuringEvaluation
        {
            get { return nagDuringEvaluation; }
            set { nagDuringEvaluation = value; }
        }
        bool nagDuringEvaluation=true;

#if NOTDEF
        [Category("Evaluation")]
        [Description("Specifies whether or not evaluation of the component should be allowed before providing a valid serial number.")]
        [DefaultValue(true)]
        public bool AllowEvaluation
        {
            get { return allowEvaluation; }
            set { allowEvaluation = value; }
        }
        bool allowEvaluation=true;


        [Category("Evaluation")]
        [Description("Length of evaluation period in days.")]
        [DefaultValue(14)]
        public int EvaluationPeriod
        {
            get { return evaluationPeriod; }
            set { evaluationPeriod = value; }
        }
        int evaluationPeriod=14;


        [Category("Evaluation")]
        [Description("Message to display to user during evaluation period.")]
        public string EvaluationMessage
        {
            get { return evaluationMessage; }
            set { evaluationMessage = value; }
        }
        string evaluationMessage;


        [Category("Evaluation")]
        [Description("Message to display when evaluation period has expired.")]
        public string EvaluationExpiredMessage
        {
            get { return evaluationExpiredMessage; }
            set { evaluationExpiredMessage = value; }
        }
        string evaluationExpiredMessage;



        
        [Category("Expiration")]
        [Description("Message to display to user when license has expired.")]
        public string ExpirationMessage
        {
            get { return expirationMessage; }
            set { expirationMessage = value; }
        }
        string expirationMessage;

#endif

        [Category("Expiration")]
        [Description("Expiration date of this component.")]
        public DateTime ExpirationDate
        {
            get { return expirationDate; }
            set { expirationDate = value; }
        }
        DateTime expirationDate;


        [Category("General")]
        [Description("The name of the licensed product, component, or class.")]
        public string ProductName
        {
            get { return productName; }
            set { productName = value; }
        }
        string productName;


        [Category("General")]
        [Description("The name of the vendor of the product being licensed.  Typically your company name.")]
        public string VendorName
        {
            get { return vendorName; }
            set { vendorName = value; }
        }
        string vendorName;


        [Category("General")]
        [Description("A URL to which users should be pointed to purchase or otherwise obtain a valid license.  Your company's home page, for example.")]
        public string ContactUrl
        {
            get { return contactUrl; }
            set { contactUrl = value; }
        }
        string contactUrl;

        [Category("General")]
        [Description("Password used to generate serial numbers.")]
        [DefaultValue("Default Master License Password")]
        public string MasterLicensePassword
        {
            get { return password; }
            set { password = value; }
        }
        string password = "Default Master License Password";

        [Category("General")]
        [Description("Event raised when a license check has failed.")]
        public event LicenseCheckFailedEventHandler LicenseCheckFailed;

        public void NotifyLicenseCheckFailed(LicenseCheckFailureReason reason, Type licensedComponentType, object instance, string licenseKey)
        {
            //Trace.WriteLine( "NotifyLicenseCheckFailed" );
            if ( LicenseCheckFailed != null )
            {
                LicenseCheckFailedEventArgs e = new LicenseCheckFailedEventArgs(reason, licensedComponentType, instance);
                LicenseCheckFailed(this, e);
            }
            else
            {
                StringBuilder sb = new StringBuilder();

                string caption = "License Check Failed";

                sb.AppendFormat( "The product {0} requires a valid license, but one could not be obtained.\n", this.ProductName);
                sb.AppendFormat( "[licensed class: {0}.  Failure reason: {1}]\n", licensedComponentType, reason );
                sb.AppendFormat( "\n" );
                sb.AppendFormat( "Use the contact information below to obtain a valid license from the vendor.\n" );
                sb.AppendFormat( "    Vendor : {0}\n", this.VendorName );
                sb.AppendFormat( "    Product: {0}\n", this.ProductName );
                sb.AppendFormat( "    Contact: {0}\n", this.ContactUrl );
                if ( licenseKey != null && licenseKey.Length != 0 )
                {
                    sb.AppendFormat( "    License Key: {0}\n", licenseKey );
                }
                sb.AppendFormat( "\n" );
                sb.AppendFormat( "[ Integrated licensing support by License Express (tm) from Wanderlust Software.  http://www.wanderlust-software.com ]");
                
                System.Windows.Forms.MessageBox.Show( sb.ToString(), caption );
            }
        
            //Trace.WriteLine( "End NotifyLicenseCheckFailed" );
        }

        [Category("General")]
        [Description("Event raised when an evaluation license is used at runtime.")]
        public event EvaluationLicenseUsedEventHandler EvaluationLicenseUsed;

        public void NotifyEvaluationLicenseUsed(Type licensedComponentType, object instance, string licenseKey)
        {
            if ( EvaluationLicenseUsed != null )
            {
                EvaluationLicenseUsedEventArgs e = new EvaluationLicenseUsedEventArgs(licensedComponentType, instance);
                EvaluationLicenseUsed(this, e);
            }
            else
            {
                StringBuilder sb = new StringBuilder();

                string caption = "Evaluation License Notice";

                sb.AppendFormat( "The product {0} is being used with an evaluation license, which ", this.ProductName );
                sb.AppendFormat( "does not grant you the rights of a full license.\n", this.ProductName);
                sb.AppendFormat( "[licensed class: {0}.]\n", licensedComponentType );
                sb.AppendFormat( "\n" );
                sb.AppendFormat( "Use the contact information below to obtain a fully licensed version " );
                sb.AppendFormat( "of this product from the vendor.\n" );
                sb.AppendFormat( "\n" );
                sb.AppendFormat( "    Vendor : {0}\n", this.VendorName );
                sb.AppendFormat( "    Product: {0}\n", this.ProductName );
                sb.AppendFormat( "    Contact: {0}\n", this.ContactUrl );
                if ( licenseKey != null && licenseKey.Length != 0 )
                {
                    sb.AppendFormat( "    License Key: {0}\n", licenseKey );
                }
                sb.AppendFormat( "\n" );
                sb.AppendFormat( "[ Integrated licensing support by License Express (tm) from Wanderlust Software.  http://www.wanderlust-software.com ]");
                
                System.Windows.Forms.MessageBox.Show( sb.ToString(), caption );
            }
        }


    }
    public delegate void LicenseCheckFailedEventHandler(object sender, LicenseCheckFailedEventArgs e);
    public class LicenseCheckFailedEventArgs : EventArgs
    {
        public LicenseCheckFailedEventArgs(LicenseCheckFailureReason reason, Type licensedComponentType, object instance)
        {
            this.Reason = reason;
            this.LicensedComponentType = licensedComponentType;
            this.Component = instance;
        }

        public LicenseCheckFailureReason Reason
        {
            get { return reason; }
            set { reason = value; }
        }
        LicenseCheckFailureReason reason;

        public Type LicensedComponentType
        {
            get { return licensedComponentType; }
            set { licensedComponentType = value; }
        }
        Type licensedComponentType;

        public object Component
        {
            get { return component; }
            set { component = value; }
        }
        object component;

#if NOTDEF
        string productName;
        string companyName;
        string contactUrl;
#endif
    }

    /// <summary>
    /// Specifies the reason for a license check failure.
    /// </summary>
    public enum LicenseCheckFailureReason
    {
        /// <summary>
        /// A license key could not be found, either in a .LIC file or embedded
        /// as a runtime resource.
        /// </summary>
        NoLicenseKeyStringFound,

        /// <summary>
        /// A license key was found, but it was not valid.
        /// </summary>
        InvalidLicenseKey,

        /// <summary>
        /// The license key used is an evaluation license, and the evaluation
        /// period has expired.
        /// </summary>
        EvaluationLicenseExpired,

        /// <summary>
        /// The licensed component has specified an expiration date which has
        /// passed.
        /// </summary>
        ComponentExpired
    };


    public delegate void EvaluationLicenseUsedEventHandler(object sender, EvaluationLicenseUsedEventArgs e);
    public class EvaluationLicenseUsedEventArgs : EventArgs
    {
        public EvaluationLicenseUsedEventArgs(Type licensedComponentType, object instance)
        {
            this.LicensedComponentType = licensedComponentType;
            this.Component = instance;
        }

        public Type LicensedComponentType
        {
            get { return licensedComponentType; }
            set { licensedComponentType = value; }
        }
        Type licensedComponentType;

        public object Component
        {
            get { return component; }
            set { component = value; }
        }
        object component;
    }

}
