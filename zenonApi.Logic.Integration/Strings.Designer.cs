﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace zenonApi.Logic.Integration {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Strings {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Strings() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("zenonApi.Logic.Integration.Strings", typeof(Strings).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error while getting filename of current process. Unable to initialize environment variables..
        /// </summary>
        internal static string CurrentProcessMainModuleFileNameNull {
            get {
                return ResourceManager.GetString("CurrentProcessMainModuleFileNameNull", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error while getting zenon Logic project folder paths. Directory {0} does not exist..
        /// </summary>
        internal static string DirectoryNotFoundInGetZenonLogicProjectFolderPaths {
            get {
                return ResourceManager.GetString("DirectoryNotFoundInGetZenonLogicProjectFolderPaths", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error while initializing K5 toolset object. Directory {0} does not exist but is required for initialization..
        /// </summary>
        internal static string DirectoryNotFoundInK5UtilitiesContructor {
            get {
                return ResourceManager.GetString("DirectoryNotFoundInK5UtilitiesContructor", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error while calling external K5Prp.dll. Initial exception message: {0}.
        /// </summary>
        internal static string ExternalK5PrpCallError {
            get {
                return ResourceManager.GetString("ExternalK5PrpCallError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Parameter {0} of method {1} must not be null..
        /// </summary>
        internal static string MethodArgumentNullException {
            get {
                return ResourceManager.GetString("MethodArgumentNullException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error while initializing zenon Logic integration object. Parameter {0} must not be null. .
        /// </summary>
        internal static string ZenonProjectReferenceNull {
            get {
                return ResourceManager.GetString("ZenonProjectReferenceNull", resourceCulture);
            }
        }
    }
}
