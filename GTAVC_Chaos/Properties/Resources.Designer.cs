﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GTAVC_Chaos.Properties {
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
    public class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("GTAVC_Chaos.Properties.Resources", typeof(Resources).Assembly);
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
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;
        ///&lt;xs:schema id=&quot;MemoryAddressSchema&quot; elementFormDefault=&quot;qualified&quot; xmlns:xs=&quot;http://www.w3.org/2001/XMLSchema&quot;&gt;
        ///
        ///  &lt;xs:element name=&quot;addresses&quot;&gt;
        ///    &lt;xs:complexType&gt;
        ///      &lt;xs:sequence&gt;
        ///        &lt;xs:element name=&quot;memoryaddress&quot;  maxOccurs=&quot;unbounded&quot; type=&quot;MemoryAddress&quot;/&gt;
        ///      &lt;/xs:sequence&gt;
        ///    &lt;/xs:complexType&gt;
        ///  &lt;/xs:element&gt;
        ///
        ///  &lt;xs:complexType name=&quot;MemoryAddress&quot;&gt;
        ///    &lt;xs:sequence&gt;
        ///      &lt;xs:element name=&quot;name&quot; type=&quot;xs:string&quot;/&gt;
        ///      &lt;xs:element n [rest of string was truncated]&quot;;.
        /// </summary>
        public static string MemoryAddressSchema {
            get {
                return ResourceManager.GetString("MemoryAddressSchema", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Icon similar to (Icon).
        /// </summary>
        public static System.Drawing.Icon SunriseIcon {
            get {
                object obj = ResourceManager.GetObject("SunriseIcon", resourceCulture);
                return ((System.Drawing.Icon)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;
        ///&lt;xs:schema id=&quot;TimedEffectSchema&quot; elementFormDefault=&quot;qualified&quot; xmlns:xs=&quot;http://www.w3.org/2001/XMLSchema&quot;&gt;
        ///
        ///  &lt;xs:element name=&quot;timedeffects&quot;&gt;
        ///    &lt;xs:complexType&gt;
        ///      &lt;xs:sequence&gt;
        ///        &lt;xs:element name=&quot;timedeffect&quot;  maxOccurs=&quot;unbounded&quot; type=&quot;TimedEffect&quot;/&gt;
        ///      &lt;/xs:sequence&gt;
        ///    &lt;/xs:complexType&gt;
        ///  &lt;/xs:element&gt;
        ///
        ///
        ///  &lt;xs:complexType name=&quot;TimedEffect&quot;&gt;
        ///    &lt;xs:sequence&gt;
        ///      &lt;xs:element name=&quot;name&quot; type=&quot;xs:string&quot;/&gt;
        ///      &lt;xs:element name [rest of string was truncated]&quot;;.
        /// </summary>
        public static string TimedEffectSchema {
            get {
                return ResourceManager.GetString("TimedEffectSchema", resourceCulture);
            }
        }
    }
}
