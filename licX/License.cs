using System;
using System.ComponentModel;
using System.Diagnostics;

namespace licX
{
    public class LicXLicense : License
    {
        License internalLicense;
        LicenseKey licenseKey;

        public LicXLicense(string licenseKeyString)
        {
            //licenseKey = new LicenseKey( licenseKeyString );
            licenseKey = new LicenseKey();
            internalLicense = null;
        }

        public LicXLicense(License l)
        {
            this.internalLicense = l;
            licenseKey = new LicenseKey( l.LicenseKey );
        }
        public override string LicenseKey
        { 
            get { return licenseKey.ToString(); }
        }
        public override void Dispose()
        {
            this.internalLicense.Dispose();
        }

        public int SerialNumber
        {
            get { return licenseKey.SerialNumber; }
        }

        public bool IsEvaluation
        {
            get 
            { 
                return (
                    (licenseKey.Type == LicenseKeyType.DesignTimeEvaluation) ||
                    (licenseKey.Type == LicenseKeyType.RuntimeEvaluation)
                    );
            }
        }
        public int EvaluationDaysRemaining
        {
            get 
            {
                if ( licenseKey.Type != LicenseKeyType.RuntimeEvaluation )
                {
                    return 0;
                }
                TimeSpan timeRemaining = licenseKey.ExpirationDate - DateTime.Now;
                return timeRemaining.Days;
            }
        }
    }
}