using System;
using System.Globalization;

// DesignTime Evaluation
// Runtime Evaluation
namespace licX
{
    public enum LicenseKeyType
    {
        DesignTimeEvaluation,
        DesignTimeRetail,
		
        RuntimeEvaluation,
        RuntimeRetail,

		Version2
    };

	// LicenseOptions appear in the oooo license field, if the
	// first field is "12".
	public enum LicenseKeyOptions
	{
		AbsoluteExpirationDate=1,
	}

    internal class LicenseKey
    {
        //
        // Expected license key format is:
        //  tt[-tt-oooo]-dd-hhhhhhhh-ssssssss[-dddddddddddddddd]
        // where
        //  tt is the LicenseKeyType, or "12" for a version 2 key type.
		//  tt-oooo is the LicenseKeyType and LicenseOptions, if tt is "12";
        //  dd is the number of days in an evaluation license.
        //  hhhhhhhh is the hash code for the entire license key string.
        //  ssssssss is the serial number for this license
        //  dddddddddddddddd is an optional expiration date, present only if
        //      the license key type is RuntimeEvaluation.
        //

        public LicenseKey(string licenseKeyString)
        {
            string[] parts = licenseKeyString.Split( new char[] { '-' } );
			int i=0;	// part number being parsed

			//
			// Read the primary license type.
			//
            Type = (LicenseKeyType) Int32.Parse(parts[i++], NumberStyles.HexNumber);

			//
			// Version 2 licenses have the real license type next, followed by
			// the license options.
			//
			if ( Type == LicenseKeyType.Version2 )
			{
				Version = 2;
				Type = (LicenseKeyType) Int32.Parse(parts[i++], NumberStyles.HexNumber);
				Options = (LicenseKeyOptions) Int32.Parse(parts[i++], NumberStyles.HexNumber);
			}

            EvaluationDays  = Int32.Parse(parts[i++], NumberStyles.HexNumber);
            Hash            = Int64.Parse(parts[i++], NumberStyles.HexNumber);            
            SerialNumber    = Int32.Parse(parts[i++], NumberStyles.HexNumber);
            if  (
				( Type == LicenseKeyType.RuntimeEvaluation ) ||
				( AbsoluteExpirationDate )
				)
            {
                ExpirationDate = new DateTime( Int64.Parse(parts[4], NumberStyles.HexNumber) );
            }
        }
        
        public LicenseKey()
        {
        }

		public bool AbsoluteExpirationDate
		{
			get { return (Options & LicenseKeyOptions.AbsoluteExpirationDate) != 0; }
			set { if ( Version >= 2 ) Options |= LicenseKeyOptions.AbsoluteExpirationDate; }
		}

        public LicenseKeyType Type
        {
			get 
			{
				if ( licenseKeyType != LicenseKeyType.Version2 )
				{
					return licenseKeyType;
				}
				else
				{
					return licenseKeyType2;
				}
			}
			set 
			{
				if ( Version >= 2 )
				{
					licenseKeyType = LicenseKeyType.Version2;
					licenseKeyType2 = value;
				}
				else
				{
					licenseKeyType = value;
				}
			}
        }
        LicenseKeyType licenseKeyType;
		
		public LicenseKeyType Type2
		{
			set { licenseKeyType2 = value; }
		}
		LicenseKeyType licenseKeyType2;

		public LicenseKeyOptions Options
		{
			get { return licenseKeyOptions; }
			set { licenseKeyOptions = value; }
		}
		LicenseKeyOptions licenseKeyOptions;

        public int Version
        {
            get { return version; }
            set { version = value; }
        }
        int version = 1;	// always default to version 1

        public int EvaluationDays
        {
            get { return evaluationDays; }
            set { evaluationDays = value; }
        }
        int evaluationDays;

        public DateTime ExpirationDate
        {
            get { return expirationDate; }
            set { expirationDate = value; }
        }
        DateTime expirationDate;

        public int SerialNumber
        {
            get { return serialNumber; }
            set { serialNumber = value; }
        }
        int serialNumber;

        public Int64 Hash
        {
            get { return hash; }
            set { hash = value; }
        }
        Int64 hash;


        public bool IsHashValid(string checksumKey)
        {
            Int64 expectedHash = CalculateHash(checksumKey);
            return expectedHash == this.Hash;
        }


        public Int64 CalculateHash(string checksumKey)
        {
            Int64 newHash;

            // build a hash of the master key appended to the serial number.
            // This is an insecure hash.
            string s = serialNumber.ToString();
            s += evaluationDays.ToString();
            if  (
				( Type == LicenseKeyType.RuntimeEvaluation ) ||
				( AbsoluteExpirationDate )
				)
            {
                s += expirationDate.Ticks.ToString( "X16" );
            }
            s += checksumKey;            
            s += s.GetHashCode().ToString(); // mix up the hash values
            //s += s; //                        a touch

            newHash = ((Int64)s.GetHashCode()) << 32;
            s += s.GetHashCode().ToString();
            newHash += s.GetHashCode();

            return newHash;
        }

        
        public override string ToString()
        {
            string licenseString;

            licenseString = ((int)licenseKeyType).ToString("X2");
            licenseString += "-";
			if ( licenseKeyType == LicenseKeyType.Version2 )
			{
				licenseString += ((int)licenseKeyType2).ToString("X2");
				licenseString += "-";
				licenseString += ((int)licenseKeyOptions).ToString("X4");
				licenseString += "-";
			}
            licenseString += evaluationDays.ToString( "X2" );
            licenseString += "-";
            licenseString += hash.ToString( "X16" );
            licenseString += "-";
            licenseString += serialNumber.ToString( "X8" );
            if  (
				( Type == LicenseKeyType.RuntimeEvaluation ) ||
				( AbsoluteExpirationDate )
				)
            {
                licenseString += "-";
                licenseString += ExpirationDate.Ticks.ToString( "X16" );
            }

            return licenseString;
        }
    }

}
