﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace K2NE.WorkingHours.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("K2NE.WorkingHours.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to Working Hours.
        /// </summary>
        internal static string CategoryName {
            get {
                return ResourceManager.GetString("CategoryName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Returns the difference in time between two dates, only considering hours within the provided working hours zone.  Warning - this will return a negative number if the end date is before the start date.  Remember you can&apos;t set escalations in the past on K2..
        /// </summary>
        internal static string DateDiffDescription {
            get {
                return ResourceManager.GetString("DateDiffDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to End Date.
        /// </summary>
        internal static string DateDiffEndDateDescription {
            get {
                return ResourceManager.GetString("DateDiffEndDateDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to End Date.
        /// </summary>
        internal static string DateDiffEndName {
            get {
                return ResourceManager.GetString("DateDiffEndName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Working Hours Date Diff.
        /// </summary>
        internal static string DateDiffName {
            get {
                return ResourceManager.GetString("DateDiffName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Start Date.
        /// </summary>
        internal static string DateDiffStartDateDescription {
            get {
                return ResourceManager.GetString("DateDiffStartDateDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Start Date.
        /// </summary>
        internal static string DateDiffStartDateName {
            get {
                return ResourceManager.GetString("DateDiffStartDateName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Working hours zone name - Empty for default working hours.
        /// </summary>
        internal static string DateDiffZoneDescription {
            get {
                return ResourceManager.GetString("DateDiffZoneDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to WorkingHours.
        /// </summary>
        internal static string DateDiffZoneName {
            get {
                return ResourceManager.GetString("DateDiffZoneName", resourceCulture);
            }
        }
        
        internal static System.Drawing.Bitmap Icon {
            get {
                object obj = ResourceManager.GetObject("Icon", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
    }
}
