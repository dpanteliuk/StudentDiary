﻿#pragma checksum "..\..\..\Dialogs\LoginDialog.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "119AA55C8F1AF728E8AE7B3518ABEF5F"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
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


namespace StudentDiary.UI.Dialogs {
    
    
    /// <summary>
    /// LoginDialog
    /// </summary>
    public partial class LoginDialog : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 12 "..\..\..\Dialogs\LoginDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox LoginInput;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\..\Dialogs\LoginDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox PasswordInput;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\..\Dialogs\LoginDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Login;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\..\Dialogs\LoginDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Cancel;
        
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
            System.Uri resourceLocater = new System.Uri("/StudentDiary.UI;component/dialogs/logindialog.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Dialogs\LoginDialog.xaml"
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
            this.LoginInput = ((System.Windows.Controls.TextBox)(target));
            return;
            case 2:
            this.PasswordInput = ((System.Windows.Controls.PasswordBox)(target));
            return;
            case 3:
            this.Login = ((System.Windows.Controls.Button)(target));
            
            #line 16 "..\..\..\Dialogs\LoginDialog.xaml"
            this.Login.Click += new System.Windows.RoutedEventHandler(this.LoginUser);
            
            #line default
            #line hidden
            return;
            case 4:
            this.Cancel = ((System.Windows.Controls.Button)(target));
            
            #line 17 "..\..\..\Dialogs\LoginDialog.xaml"
            this.Cancel.Click += new System.Windows.RoutedEventHandler(this.CloseDialog);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

