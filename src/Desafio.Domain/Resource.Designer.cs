//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Desafio.Domain {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resource {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resource() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Desafio.Domain.Resource", typeof(Resource).Assembly);
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
        ///   Looks up a localized string similar to {0} already exists.
        /// </summary>
        internal static string CONFLICT {
            get {
                return ResourceManager.GetString("CONFLICT", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} is not allowed.
        /// </summary>
        internal static string FORBIDEN {
            get {
                return ResourceManager.GetString("FORBIDEN", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Field {0} is invalid.
        /// </summary>
        internal static string INVALID {
            get {
                return ResourceManager.GetString("INVALID", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Password must have letters and numbers.
        /// </summary>
        internal static string INVALID_PASSWORD {
            get {
                return ResourceManager.GetString("INVALID_PASSWORD", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid fields.
        /// </summary>
        internal static string INVALIDS {
            get {
                return ResourceManager.GetString("INVALIDS", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} not found.
        /// </summary>
        internal static string NOT_FOUND {
            get {
                return ResourceManager.GetString("NOT_FOUND", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} is required.
        /// </summary>
        internal static string REQUIRED {
            get {
                return ResourceManager.GetString("REQUIRED", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unauthorized.
        /// </summary>
        internal static string UNAUTHORIZED {
            get {
                return ResourceManager.GetString("UNAUTHORIZED", resourceCulture);
            }
        }
    }
}
