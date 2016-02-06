
using System;
using System.ComponentModel;
using System.Reflection;
using System.Diagnostics;


namespace licX
{
    //
    //  LicXLicenseProvider is our license provider, which is invoked by VS
    //  at design- and build-time, and by the .NET runtime at runtime.  The
    //  base license provider class is LicenseProvider, which specifies just
    //  one method, GetLicense, that needs to be implemented by the provider.
    //  We derive from LicFileLicense provider, which handles the details of
    //  locating .LIC files and so forth for us.  It specifies the GetKey and
    //  IsKeyValid methods in addition to GetLicense.
    //
    public class LicXLicenseProvider : LicFileLicenseProvider
    {
        //
        //  Instance variables - these variables are stored away in GetLicense
        //  and IsKeyValid so that they are available from any method.  Note 
        //  that they presume we will never get more than one concurrent 
        //  GetLicense call, which appears to be a valid assumption.
        //

        LicenseContext context;
        object instance;       
        string licenseKeyString;
        LicXLicenseComponent licenseComponent;
        const string registryCacheName = @"Software\Wanderlust\licX\KeyCache";
        LicenseCheckFailureReason licenseCheckFailureReason = LicenseCheckFailureReason.NoLicenseKeyStringFound;
        TraceSwitch traceSwitch;
               
        
         

        public LicXLicenseProvider()
        {
            traceSwitch = new TraceSwitch( "licX", "Trace switches for licX licensing." );
            if ( traceSwitch.TraceVerbose )
            {
                Trace.WriteLine( "----- licX Assembly Details -----" );
                Trace.WriteLine( "Assembly:               " + Assembly.GetExecutingAssembly() );
                Trace.WriteLine( "License Component Type: " + typeof(LicXLicenseComponent).FullName );
                Trace.WriteLine( "License Component Guid: " + typeof(LicXLicenseComponent).GUID );
                Trace.WriteLine( "-----  end Assembly Details -----" );
            }
        }

        
        //
        //  GetLicense is the entry point called by the LicenseManager at design-time,
        //  build-time, and runtime.  It is the only method defined by the 
        //  LicenseProvider base class (though GetKey and IsKeyValid are defined by
        //  LicFileLicenseProvider, which we derive from).
        //

        public override License GetLicense(
            LicenseContext context, 
            Type type, 
            object instance, 
            bool allowExceptions
            )
        {
            License license;      

            if ( traceSwitch.TraceVerbose )
            {
                Trace.WriteLine( String.Format("GetLicense({0}, {1})", context.UsageMode, type ) );
                Trace.Indent();
            }


            //
            // Save the instance and context parameters in our fields, since we
            // will need their values again in IsKeyValid() and GetKey().
            //        
    
            this.instance = instance;
            this.context = context;
            this.licenseComponent = GetLicenseComponent(type, instance);
            this.licenseKeyString = null;

            
            if( traceSwitch.TraceVerbose )
            {
                Trace.WriteLine( "Context is of type:   " + context.GetType() );
                Trace.WriteLine( "Full name of type is: " + type.AssemblyQualifiedName);
                Trace.WriteLine( "...and it has GUID:   " + type.GUID);
                if( this.licenseComponent != null )
                {
                    Trace.WriteLine( "License Component found with type: " + this.licenseComponent.GetType().AssemblyQualifiedName );
                }
                else
                {
                    Trace.WriteLine( "License Component was NOT found." );
                }
            }


            //
            // Call the base class GetLicense(), which will find the appropriate
            // license file or embedded license reference, and subsequently call
            // IsKeyValid() and GetKey().
            //

            license = base.GetLicense(context, type, instance, allowExceptions);


            //
            // At this point IsKeyValid() and GetKey() have been called.
            // If the license object is null, we have an invalid license. 
            // Handle failures.
            //

            if( traceSwitch.TraceVerbose )
            {
                if( license != null )
                {
                    Trace.WriteLine( "License was granted." );
                    Trace.WriteLine( "  License:            " + license.ToString() );
                    Trace.WriteLine( "  License.LicenseKey: " + license.LicenseKey );
                }
                else
                {
                    Trace.WriteLine( "License was NOT granted." );
                }
            }

           
            if( license == null )
            {
                //
                // Raise the license check failed event.  Note that we may 
                // sometimes have a NULL licenseComponent (for example, if the
                // caller used the wrong overload of LicenseManager.Validate()).
                //

                if( licenseComponent != null )
                {
                    licenseComponent.NotifyLicenseCheckFailed(
                        licenseCheckFailureReason, 
                        type, 
                        instance, 
                        licenseKeyString
                        );
                }                

                if( traceSwitch.TraceVerbose )
                {
                    Trace.WriteLine( String.Format("GetLicense: license was not granted.  Reason={0}", licenseCheckFailureReason) );
                    Trace.Unindent();
                }
                return null;
            }


            //
            // At this point, we have a valid license generated by 
            // LicFileLicenseProvider.  We'll create our own License object
            // to return to the caller.
            //

            LicXLicense l = new LicXLicense( license );
            
            if( traceSwitch.TraceVerbose )
            {
                Trace.WriteLine( String.Format("GetLicense: license granted.  Key={0}", license.LicenseKey) );
            }          


            //
            // If this is an evaluation license, and we are set up to nag during
            // evaluation at runtime, raise the "evaluation license used" event.
            //

            if(
                ( context.UsageMode == LicenseUsageMode.Runtime ) && 
                ( l.IsEvaluation ) && 
                ( licenseComponent.NagDuringEvaluation )
                )                
            {
                licenseComponent.NotifyEvaluationLicenseUsed(
                    type, 
                    instance, 
                    l.LicenseKey
                    );
            }                        
            Trace.Unindent();
            return l;
        }


        //
        // This routine gets the runtime license key assocaited with an 
        // application.  This is necessary ...
        //protected LicenseKey GetApplicationRuntimeLicenseKey()
        //{
        //}

      
        //
        //  IsKeyValid is defined by LicFileLicenseProvider, and is called
        //  during processing of GetLicense to determine if a given license
        //  key string represents a valid license.
        //
        //  Notice that we are only given the string and the type of the
        //  class being licensed.  We need to use the LicXLicenseComponent
        //  that is a member of the licensed instance, so we pull that out
        //  of the class field where it had been stored earlier.
        //
        //  Note that in the case of an application licensed with an evaluation
        //  license, we may have to go to the registry to find an appropriate
        //  license key string.
        //
        
        protected override bool IsKeyValid(string licenseKeyString, Type type)
        {
            bool keyIsValid;
            bool RunTimKeyFound = false;           
            LicenseKey licenseKey;            

            if ( traceSwitch.TraceVerbose )
            {
                Trace.WriteLine( String.Format( "IsKeyValid({0}, {1})", licenseKeyString, type ));
            }
            Trace.Indent();

            //
            // Save LicenseKeyString for later use in GetKey.  If this is a
            // runtime application using a design time key, it will be changed
            // and stored for GetKey()
            //

            this.licenseKeyString = licenseKeyString;

            //
            // Create a LicenseKey object with the given license key string.
            // The LicenseKey can throw an Exception for the int32.parse 
            // method.  Fail it if does, b/c something is a matter with our 
            // license key string.
            //

            try
            {                
                licenseKey = new LicenseKey(licenseKeyString);
            }
            catch( ArgumentException )
            {                
                licenseCheckFailureReason = LicenseCheckFailureReason.InvalidLicenseKey;                
                return false;
            } 
            
            //
            // If we are being invoked at runtime, and the license key string
            // we are given is of type "design time evaluation", we are dealing
            // with a licensed application (as opposed to a component).  In 
            // this case, the runtime license we use needs to be in the 
            // registry.  (The first time the application is run, we need to
            // create this registry license).
            //
            // BUGBUG: It doesn't appear that the check for 
            // BUGBUG: "!licenseKey.AbsoluteExpirationDate is ever needed?  
            // BUGBUG: Need to do a full test cycle to make sure...
            //
            
            if( 
                ( LicenseManager.CurrentContext.UsageMode == LicenseUsageMode.Runtime ) &&
                ( licenseKey.Type == LicenseKeyType.DesignTimeEvaluation ) && 
                ( !licenseKey.AbsoluteExpirationDate )
                )
            {               
                
                RunTimKeyFound = GetApplicationRuntimeLicenseKey( licenseKey, type);

                if(!RunTimKeyFound)
                {
                    // there was a problem writing or reading from the registry, so
                    // fail the license.
                    
                    licenseCheckFailureReason = LicenseCheckFailureReason.InvalidLicenseKey;
                    return false;
                }
                else
                {
                    // 
                    // this.licenseKeyString has changed to runtime application, so update
                    // licenseKey
                    //

                    try
                    {                
                        licenseKey = new LicenseKey(this.licenseKeyString);
                    }
                    catch( ArgumentException )
                    {                
                        licenseCheckFailureReason = LicenseCheckFailureReason.InvalidLicenseKey;                
                        return false;
                    } 
                }
            }
                
#if delete 
/*                //
                // Here we check the registry to find if a runtime license exists.
                // If we do not find one, assume that this is the first time this 
                // application is running.
                //

                String regKeyName = registryCacheName + "\\" + type.GUID.ToString();
                 
                try
                {                          
                    regKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey( regKeyName, true );
                }
                catch( ArgumentException e )
                {
                    Trace.WriteLine("Exception building license.");
                    Trace.WriteLine(e);
                    licenseCheckFailureReason = LicenseCheckFailureReason.InvalidLicenseKey;                
                    return false;
                }
                // BUGBUG: SecurityException ?


                if ( regKey == null )
                {
                    generateruntimefromdesign();
                    regKey = 
                }

                //
                // Read the license key string from the registry, and use it
                // to create a new licenseKey object.  regKey will 
                // be null (undefined in VS) if the value doesn't exist. 
                // 
                
                if ( regKey != null )
                {
                    // BUGBUG: SecurityException ?
                    this.licenseKeyString = (string) regKey.GetValue( "License" );
                    if ( this.licenseKeyString == NULL )
                    {
                        // fail.
                    }

                    try
                    {
                        licenseKey = new LicenseKey(this.licenseKeyString);
                    }
                    catch( ArgumentException e)
                    {
                        licenseCheckFailureReason = LicenseCheckFailureReason.InvalidLicenseKey;                
                        return false;
                    } 
                    
                    regKey.Close();
                }
                else
                {
                    noRunTimeKeyFound = true;
                }

                                
                //
                // If the license key string was not found in the registry, 
                // it's assumed that this is the first run of an application.  
                // Create a new runtime license key and save it 
                // to the registry.
                //
 
                if( noRunTimeKeyFound  )
                {
                    //
                    // Modify the license key to be a 'runtime evaluation' license by
                    // changing its type, setting the expiration date, and recalculating
                    // its hash value.  The serial number stays the same.
                    //
                                       
                    licenseKey.Type = LicenseKeyType.RuntimeEvaluation;
                    licenseKey.ExpirationDate = DateTime.Now.AddDays( licenseKey.EvaluationDays );
                    licenseKey.Hash = licenseKey.CalculateHash( licenseComponent.MasterLicensePassword );

    
                    //
                    // Save the new runtime license key string, as it is needed
                    // later by GetKey().
                    //

                    this.licenseKeyString = licenseKey.ToString();

    
                    //
                    // Write out to the Registry. CreateSubKey returns null if it fails.  
                    // If an exceptions occurs, fail the license. 
                    //
                    // Note that CreateSubKey may return a SecurityException, but the 
                    // compiler throws an error.
                    //
                    // BUGBUG: proibably just need to refer to it as System.Security.SecurityException
                    //
                    try
                    {     
                        regKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey( registryCacheName + "\\" + type.GUID.ToString() );
                        if ( regKey == null )
                        {
                            // BUGBUG: handle error.
                        }
                        regKey.SetValue( "License", (string) this.licenseKeyString );
                    }                    
                    catch( UnauthorizedAccessException e )
                    {
                        Trace.WriteLine("Exception writing to registry.");
                        Trace.WriteLine(e);
                        licenseCheckFailureReason = LicenseCheckFailureReason.InvalidLicenseKey;                        
                        return false;
                    }
                    //catch( SecurityException e)
                    //{
                    //    Trace.WriteLine("Exception writing to registry.");
                    //    Trace.WriteLine(e);
                    //    licenseCheckFailureReason = LicenseCheckFailureReason.InvalidLicenseKey;                        
                    //    return false;
                    //}
      
                    if( regKey != null ) 
                    {
                        regKey.Close();
                    }                                            
                }
            } // end of if licenseType is runtime using design time eval license              
*/
#endif //delete

            //
            // Now we have ensured that we have the right key ( The .lic file
            // was possibly replaced with a runtime key saved in the registry).
            // First validate that the key is authentic.
            //

            keyIsValid = licenseKey.IsHashValid( licenseComponent.MasterLicensePassword );
            if( !keyIsValid )
            {
                if( traceSwitch.TraceVerbose )
                {
                    Trace.WriteLine("IsKeyValid: License key is not properly signed.  Key=" + licenseKeyString);                
                }
                Trace.Unindent();
                licenseCheckFailureReason = LicenseCheckFailureReason.InvalidLicenseKey;
                return false;
            }

             
            //
            // If the licenseKey is a runtime evaluation, check if:
            //   - expiration date is expired
            //   - expiration period is longer than maximum allowed
            //   - it has an absolute evaluation date, check if its
            //     not expired
            //   - a 'time-bomb' has been set for the component itself,
            //     and if so, should it go boom.
            // 
            // If any of these returns a expired or invalid value, declare
            // that the key is not valid by returning false after setting
            // the licenseCheckFailureReason.  Otherwise return true.
            //

            if( licenseKey.Type == LicenseKeyType.RuntimeEvaluation )
            {
                if( traceSwitch.TraceVerbose )
                {
                    Trace.WriteLine("  License key is runtime evaluation.");                
                    Trace.WriteLine("    Current Date:    " + DateTime.Now);
                    Trace.WriteLine("    Expiration Date: " + licenseKey.ExpirationDate);
                }

                //
                // Check whether the current date is beyond the expiration date.
                //

                if( DateTime.Now > licenseKey.ExpirationDate )
                {
                    if( traceSwitch.TraceVerbose )
                    {
                        Trace.WriteLine("IsKeyValid: License is expired.  ExpirationDate = " + licenseKey.ExpirationDate);                    
                    }
                    Trace.Unindent();              
                    licenseCheckFailureReason = LicenseCheckFailureReason.EvaluationLicenseExpired;
                    return false;
                }                

                //
                // Check whether the expiration date is higher than possible - this points
                // to someone adjusting their system calender while building a component in
                // an attempt to have an eval license valid for a long time.
                //

                DateTime maxExpirationDate = DateTime.Now.AddDays( licenseKey.EvaluationDays );
                if( licenseKey.ExpirationDate > maxExpirationDate )
                {          
                    if( traceSwitch.TraceVerbose )
                    {
                        Trace.WriteLine("IsKeyValid: License has invalid expiration date.");
                        Trace.WriteLine("  Evaluation Days:     " + licenseKey.EvaluationDays);
                        Trace.WriteLine("  Max Expiration Date: " + maxExpirationDate);
                        Trace.Unindent();
                    }
                    licenseCheckFailureReason = LicenseCheckFailureReason.InvalidLicenseKey;                    
                    return false;
                }                
            }

            //
            // If its past the Absolute Expiration Date, fail it even if the evaluation
            // period is not expired
            //

            if( licenseKey.AbsoluteExpirationDate )
            {
                if( traceSwitch.TraceVerbose )
                {
                    Trace.WriteLine("  License key has absolute expiration date.");                
                    Trace.WriteLine("    Current Date:    " + DateTime.Now);
                    Trace.WriteLine("    Expiration Date: " + licenseKey.ExpirationDate);
                }

                //
                // Check whether the current date is beyond the expiration date.
                //

                if( DateTime.Now > licenseKey.ExpirationDate )
                {
                    Trace.WriteLine("IsKeyValid: License is expired.  ExpirationDate="+licenseKey.ExpirationDate);
                    Trace.Unindent();
                    licenseCheckFailureReason = LicenseCheckFailureReason.EvaluationLicenseExpired;					
                    return false;
                }
            }      


            //
            // Check to see if a 'time-bomb' has been set for the component itself.
            //

            if( 
                ( licenseComponent.ExpirationDate.Ticks != (long)0 ) && 
                ( DateTime.Now > licenseComponent.ExpirationDate )
                )
            {
                if( traceSwitch.TraceVerbose )
                {
                    Trace.WriteLine("IsKeyValid: Component has expired.  ExpirationDate=" + licenseComponent.ExpirationDate); 
                }               
                Trace.Unindent();
                licenseCheckFailureReason = LicenseCheckFailureReason.ComponentExpired;
                return false;
            }
            
            //
            // Passed all the tests, so we have a valid key
            //
   
            return true;
        }


        //
        //  GetKey() is defined by LicFileLicenseProvider.  It is called during
        //  GetLicense() processing in order to populate the LicenseKey property
        //  of the License object returned to the caller.
        //

        protected override string GetKey(Type type)
        {
            string key;
            LicenseKey licenseKey;

            if( traceSwitch.TraceVerbose )
            {
                Trace.WriteLine( String.Format( "GetKey(type={0})", type) );
            }
            Trace.Indent();

            //
            // We return whichever license key string we were presented in IsKeyValid(),
            // unless we had been presented a 'design time evaluation' license.
            // In the latter case, we generate a new license key that encodes the
            // evaluation expiration date in it.  This license key will subsequenty
            // be embedded in the executable being built.
            //

            key = this.licenseKeyString;

            //
            // Myk, what do we do if we catch an exception here?  we should
            // never, because key should be valid.  If we return null, will we
            // still get a license?  In that case, we should never throw any
            // exceptions.
            //

            try
            {
                licenseKey = new LicenseKey( key );
            }
            catch( ArgumentException)
            {                
                licenseCheckFailureReason = LicenseCheckFailureReason.InvalidLicenseKey;                
                return null; 
            }             
            
            if( traceSwitch.TraceVerbose )
            {
                Trace.WriteLine( "GetKey: License type is " + licenseKey.Type);
            }

            if( licenseKey.Type == LicenseKeyType.DesignTimeEvaluation )
            {
                //
                // Modify the license key to be a 'runtime evaluation' license by
                // changing its type, setting the expiration date, and recalculating
                // its hash value.
                //

                licenseKey.Type = LicenseKeyType.RuntimeEvaluation;
                licenseKey.ExpirationDate = DateTime.Now.AddDays( licenseKey.EvaluationDays );
                licenseKey.Hash = licenseKey.CalculateHash( licenseComponent.MasterLicensePassword );
                
                key = licenseKey.ToString();
            }

            if( traceSwitch.TraceVerbose )
            {
                Trace.WriteLine("Generated license key: " + key);
            }
            Trace.Unindent();

            //
            // Return the new license key string to be embedded.
            //

            return key;
        }

   
        //
        // Tries to find the licXComponent in the object passed in. If
        // it does, returns that object, otherwise returns null;
        //


        LicXLicenseComponent GetLicenseComponent(Type type, object instance)
        {
            LicXLicenseComponent licenseComponent = null;

            if( instance == null )
            {
                return null;
            }

            //
            // Reach inside the object that is being licensed, and find our 
            // LicXLicenseComponent object.
            //

            FieldInfo[] fis = type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            foreach(FieldInfo fi in fis)
            {
                if( fi.FieldType == typeof(LicXLicenseComponent))
                {
                    licenseComponent = (LicXLicenseComponent)fi.GetValue(instance);
                    break;
                }
            }

            if( licenseComponent == null )
            {
                if( traceSwitch.TraceVerbose )
                {
                    Trace.WriteLine( "LicXLicenseComponent not found in licensed component!" );
                }
            }

            return licenseComponent;
        }

        
        //
        // This routine gets the runtime license key associated with an 
        // application.  It returns true if licence runtime key was 
        // retrieved from registry, or created and stored in registry.  
        // This is necessary ...
        //
    
        private bool GetApplicationRuntimeLicenseKey(LicenseKey licenseKey, Type type)
        {
            
            Microsoft.Win32.RegistryKey regKey = null;
            bool SuccessfulWriteToRegisty = false;

            //
            // Here we check the registry to find if a runtime license exists.
            // If we do not find one, assume that this is the first time this 
            // application is running.
            //

            String regKeyName = registryCacheName + "\\" + type.GUID.ToString();
                
            try
            {
                regKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey( regKeyName, true );
            }
            catch( ArgumentException e )
            {
                Debug.WriteLine("Exception trying to read registry.");
                Debug.WriteLine(e);
                return false;
            }
            catch( System.Security.SecurityException e)
            {
                Debug.WriteLine("Exception reading registry.");
                Debug.WriteLine(e);
                return false;
            }
            
           
            if ( regKey == null )
            {

                //
                // No key found in registry, so generate one and save it. 
                //

                SuccessfulWriteToRegisty = GenerateRunTimeLicenseKeyFromDesignTime(licenseKey, type);
                
                //
                // If there was a problem writing to the registry, 
                // return false thereby failing the license
                //

                if(!SuccessfulWriteToRegisty)
                {
                    licenseCheckFailureReason = LicenseCheckFailureReason.InvalidLicenseKey;
                    return false;
                }
                try
                {
                    regKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey( regKeyName, true );
                }
                catch( ArgumentException e )
                {
                    Debug.WriteLine("Exception trying to read registry.");
                    Debug.WriteLine(e);
                    return false;
                }
                catch( System.Security.SecurityException e)
                {
                    Debug.WriteLine("Exception reading registry.");
                    Debug.WriteLine(e);
                    return false;
                }

            }
            
            if ( regKey != null )
            {                
                this.licenseKeyString = (string) regKey.GetValue( "License" );
                if ( this.licenseKeyString == null )
                {
                    //
                    // Someone probably deleted key manually.  Fail the license.
                    //

                    licenseCheckFailureReason = LicenseCheckFailureReason.InvalidLicenseKey;
                    regKey.Close();
                    return false;

                }       
                    
                regKey.Close();
            }
            else
            {

                licenseCheckFailureReason = LicenseCheckFailureReason.InvalidLicenseKey;                 
                return false;
            }

            
              
            //
            // this.licenseKeyString updated               
            //

            return true;
        }

        
        //
        // Returns false if it failed to generate run time LicenseKey 
        // either b/c an exception was thrown when creating or opening a 
        // registry subkey, or a regKey could not succesfully write to the
        // registry.
        //
        // Returns boolean newKeyInRegistry
        //

        private bool GenerateRunTimeLicenseKeyFromDesignTime(LicenseKey licenseKey, Type type)
        {
            Microsoft.Win32.RegistryKey regKey = null;
            bool newKeyInRegistry = false;
 
            //
            // Modify the license key to be a 'runtime evaluation' license by
            // changing its type, setting the expiration date, and recalculating
            // its hash value.  The serial number stays the same.
            //
                                       
            licenseKey.Type = LicenseKeyType.RuntimeEvaluation;
            licenseKey.ExpirationDate = DateTime.Now.AddDays( licenseKey.EvaluationDays );
            licenseKey.Hash = licenseKey.CalculateHash( licenseComponent.MasterLicensePassword );

            //
            // Write out to the Registry. CreateSubKey returns null if it fails.  
            // If null is returned or an exceptions occurs, catch it and return null. 
            //
                    
            try
            {     
                regKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey( registryCacheName + "\\" + type.GUID.ToString() );
                if ( regKey == null )
                {
                    Debug.WriteLine("Failed writing to registry.");
                    return newKeyInRegistry;           
                }
                regKey.SetValue( "License", (string) licenseKey.ToString() );
            }                    
            catch( UnauthorizedAccessException e )
            {
                Debug.WriteLine("Exception writing to registry.");
                Debug.WriteLine(e);
                return newKeyInRegistry;
            }
            catch( System.Security.SecurityException e)
            {
                Debug.WriteLine("Exception writing to registry.");
                Debug.WriteLine(e);
                return newKeyInRegistry;
            }
      
            if( regKey != null ) 
            {
                regKey.Close();
            }
                    
            //
            // Save the new runtime license key string, as it is needed
            // later by GetKey().
            //
            
            this.licenseKeyString = licenseKey.ToString();
          
            newKeyInRegistry = true;
                             
            return newKeyInRegistry; 
                                                                    
        }             

    }
}
