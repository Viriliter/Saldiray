﻿#pragma checksum "..\..\..\..\SubPages\SubSettings\Settings03.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "F3DAD4D1FDB9A83B1ACF22F2CEF8B275531959ED38704D8BAA5104358E0C1166"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using FornoxGUI;
using FornoxGUI.ExternalScripts;
using FornoxGUI.SubPages.SubSettings;
using Ozeki.Media;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using WPFMediaKit.DirectShow.Controls;
using WPFMediaKit.DirectShow.MediaPlayers;


namespace FornoxGUI.SubPages.SubSettings {
    
    
    /// <summary>
    /// Settings03
    /// </summary>
    public partial class Settings03 : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 31 "..\..\..\..\SubPages\SubSettings\Settings03.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox Combo_CameraID;
        
        #line default
        #line hidden
        
        
        #line 36 "..\..\..\..\SubPages\SubSettings\Settings03.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Btn_Connect;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\..\..\SubPages\SubSettings\Settings03.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Btn_Disconnect;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/FornoxGUI;component/subpages/subsettings/settings03.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\SubPages\SubSettings\Settings03.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.Combo_CameraID = ((System.Windows.Controls.ComboBox)(target));
            
            #line 31 "..\..\..\..\SubPages\SubSettings\Settings03.xaml"
            this.Combo_CameraID.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.cobVideoSource_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 2:
            this.Btn_Connect = ((System.Windows.Controls.Button)(target));
            
            #line 36 "..\..\..\..\SubPages\SubSettings\Settings03.xaml"
            this.Btn_Connect.Click += new System.Windows.RoutedEventHandler(this.LoadCamViewer);
            
            #line default
            #line hidden
            return;
            case 3:
            this.Btn_Disconnect = ((System.Windows.Controls.Button)(target));
            
            #line 37 "..\..\..\..\SubPages\SubSettings\Settings03.xaml"
            this.Btn_Disconnect.Click += new System.Windows.RoutedEventHandler(this.UnloadCamViewer);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

