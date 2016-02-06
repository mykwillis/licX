using System;
using System.Windows.Forms;
using System.ComponentModel;

namespace licX
{
    [LicenseProvider(typeof(licX.LicXLicenseProvider))]
    class KeyGen
    {
        int startingSerialNumber = 1;
        int numberOfLicenseStringsToGenerate = 1;
        string masterKey = "Default Master License Password";
        string[] licenseStrings;
        int evalDays=0;
        bool isEval=false;
		bool expires=false;
		DateTime expirationDate;
#if !INTERNAL
        licX.LicXLicense license;
#endif
        licX.LicXLicenseComponent licenseComponent;

        KeyGen()
        {
            licenseComponent = new licX.LicXLicenseComponent();
            licenseComponent.ContactUrl             = "http://www.wanderlust-software.com";
            licenseComponent.MasterLicensePassword  = "licXXcil";
            licenseComponent.ProductName            = "License Express(tm) for .NET";
            licenseComponent.VendorName             = "Wanderlust Software, LLC";
            licenseComponent.NagDuringEvaluation    = false;

            // Validate the licX license.
#if !INTERNAL
            try
            {
                license = (licX.LicXLicense) LicenseManager.Validate(this.GetType(), this);            
            }
            catch( /* LicenseException e */Exception )
            {
                // swallow the LicenseException if non valid key is used for KeyGen.
                
                //Console.WriteLine("Exception building license.");
                //Console.WriteLine(e);
                Application.Exit();
            }
#endif
        }

        void LoadDefaultParams()
        {
        }

        void ProcessCommandLine(string[] args)
        {
            if ( args.Length == 0 )
            {
                DisplayUsage();
                numberOfLicenseStringsToGenerate=0;
                return;
            }

            for(int i=0; i < args.Length;i++ )
            {
                switch(args[i].ToLower())
                {
                    case "-?": goto case "help";
                    case "/?": goto case "help";
                    case "-help": goto case "help";
                    case "/help": goto case "help";
                    case "help":
                        DisplayUsage();
                        numberOfLicenseStringsToGenerate = 0;
                        return;

                    case "-start": goto case "start";
                    case "/start": goto case "start";
                    case "start":
                        if ( ++i < args.Length )
                        {
							try
							{
								startingSerialNumber = Int32.Parse( args[i] );
							}
							catch(Exception e)
							{
								Console.WriteLine("start: bad evaluation period.");
								Console.WriteLine( e.Message );
								Application.Exit();
							}
                        }
                        break;

                    case "-count": goto case "count";
                    case "/count": goto case "count";
                    case "count":
                        if ( ++i < args.Length )
                        {
							try
							{
                                numberOfLicenseStringsToGenerate = Int32.Parse( args[i] );
							}
							catch(Exception e)
							{
								Console.WriteLine("count: bad evaluation period.");
								Console.WriteLine( e.Message );
								Application.Exit();
							}
						}
                        
                        break;

                    case "-password": goto case "password";
                    case "/password": goto case "password";
                    case "password":
                        if ( ++i < args.Length )
                        {
                            masterKey = args[i];
                        }
                        break;

                    case "-eval": goto case "eval";
                    case "/eval": goto case "eval";
                    case "eval":
                        isEval = true;
                        if ( ++i < args.Length )
                        {
							try
							{
								evalDays = Int32.Parse(args[i]);
							}
							catch (Exception e)
							{
								Console.WriteLine("eval: bad evaluation period.");
								Console.WriteLine( e.Message );
								Application.Exit();
							}
                        }
                        break;
					case "-expire": goto case "expire";
					case "/expire": goto case "expire";
					case "expire":
						expires = true;
						if ( ++i < args.Length )
						{
							try
							{
								expirationDate = DateTime.Parse(args[i]);
							}
							catch (Exception e)
							{
								Console.WriteLine("expire: bad expiration date.");
								Console.WriteLine( e.Message );
								Application.Exit();
							}
						}
						break;
                    default:
                        Console.Error.WriteLine("Unrecognized option: " + args[i]); 
                        DisplayUsage();
                        numberOfLicenseStringsToGenerate = 0;
                        return;
                        
                } // switch
            } // for
        }

        void GenerateLicenseStrings()
        {            
            int currentSerialNumber = startingSerialNumber;

            licenseStrings = new string[numberOfLicenseStringsToGenerate];
            for(int i=0;i<numberOfLicenseStringsToGenerate;i++)
            {
                LicenseKey licenseKey = new LicenseKey();
				
				if ( expires )
				{
					licenseKey.Version = 2;
					licenseKey.Options = LicenseKeyOptions.AbsoluteExpirationDate;
					licenseKey.ExpirationDate = expirationDate;
				}
				
                if ( isEval )
                {
                    licenseKey.Type = LicenseKeyType.DesignTimeEvaluation;
                }
                else
                {
                    licenseKey.Type = LicenseKeyType.DesignTimeRetail;
                }

                licenseKey.EvaluationDays = evalDays;
                licenseKey.SerialNumber = currentSerialNumber;
                licenseKey.Hash = licenseKey.CalculateHash( masterKey );

                licenseStrings[i] = licenseKey.ToString();

                currentSerialNumber++;
            }
        }

        void ValidateLicenseStrings()
        {
            bool allStringsAreValid = true;

            foreach(string s in licenseStrings)
            {
                LicenseKey licenseKey = new LicenseKey( s );
                if ( !licenseKey.IsHashValid(masterKey) )
                {
                    Console.WriteLine("License string {0} is not valid!", s);
                    allStringsAreValid = false;
                }
            }

            if ( allStringsAreValid )
            {
                Console.WriteLine("[All license strings are valid.]");
            }
        }

        void WriteStringsToStdout()
        {
            foreach(string s in licenseStrings)
            {
                Console.WriteLine( s );
            }
        }

        void WriteStringsToClipboard()
        {
            string ss="";

            foreach(string s in licenseStrings)
            {
                ss += s;
                ss += "\r\n";
            }

            Clipboard.SetDataObject( ss, true );            
        }


        void Run(string[] args)
        {
            // Load default params from configuration file.
            LoadDefaultParams();

            // Override default params with any specified on command line.            
            ProcessCommandLine(args);
            
#if !INTERNAL
            if ( license.IsEvaluation )
            {
                System.Console.WriteLine();
                System.Console.WriteLine("Evaluation Version Note:");
                System.Console.WriteLine(
                    "  Because you have an evaluation version of License Express (tm), this\n" +
                    "  utility will only generate evaluation license keys with a maximum\n" +
                    "  expiration time of 2 days.  Visit http://www.wanderlust-software.com\n" +
                    "  to purchase a retail license which provides full functionality.\n" 
                    );

                LicenseKey licenseKey = new LicenseKey( license.LicenseKey );
                if  (
                    ( numberOfLicenseStringsToGenerate != 0 ) &&
                    ((!this.isEval) || (this.isEval && this.evalDays > 2)) 
                    )
                {
                    System.Console.Error.WriteLine(
                        "Keys not generated: Specify an evaluation license of 2 days or less.\n"+
                        "For example:\n"+
                        "  KeyGen /eval 2 /password \"My Password\" /count 5"
                        );                    
                    return;
                }
            }
#endif

            // Generate license strings.
            GenerateLicenseStrings();

            //ValidateLicenseStrings();

            // Write license strings to stdout.
            WriteStringsToStdout();

            // Write license strings to clipboard
            WriteStringsToClipboard();
            
        }


        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {           
            try
            {
                new KeyGen().Run(args);
            }
            catch(LicenseException)
            {
            }
        }

        void DisplayUsage()
        {
            Console.WriteLine("Wanderlust (R) KeyGen License Key Generation Utility.");
            Console.WriteLine("Copyright (C) Wanderlust Software 2002-2004.  All Rights Reserved.");
#if !INTERNAL
            if ( license.IsEvaluation )
            {
                Console.WriteLine("[EVALUATION VERSION]");
            }
#endif
            Console.WriteLine();
            Console.WriteLine("usage: KeyGen [/start <sn>] [/count <count>] [/password <string>]");
            Console.WriteLine("              [/expire <date>] [/eval <days>]");
            Console.WriteLine();
            Console.WriteLine("options:");
            Console.WriteLine("  /start <sn>        - Specifies <sn> as the starting serial number.");
            Console.WriteLine("                       By default, 0 is the starting serial number.");
            Console.WriteLine("  /count <count>     - Specifies that <count> license keys should be");
            Console.WriteLine("                       generated.");
            Console.WriteLine("                       By default, 1 license key is generated.");
            Console.WriteLine("  /password <string> - Specifies <string> as the password for the");
            Console.WriteLine("                       license keys.");
            Console.WriteLine("  /expires <date>    - Specifies that licenses will expire on the");
            Console.WriteLine("                       specified <date>.");
            Console.WriteLine("  /eval <days>       - Specifies that evaluation licenses should be");
            Console.WriteLine("                       generated that expire <days> days after a");
            Console.WriteLine("                       component is compiled or an application is");
            Console.WriteLine("                       first run.  By default, retail (non-evaluation)");
            Console.WriteLine("                       licenses are generated.");
        }
    }
}
