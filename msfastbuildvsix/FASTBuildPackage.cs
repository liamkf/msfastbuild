// Copyright 2016 Liam Flookes and Yassine Riahi
// Available under an MIT license. See license file on github for details.

using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;

using EnvDTE;
using EnvDTE80;

namespace msfastbuildvsix
{
	public class OptionPageGrid : DialogPage
	{
		private string FBArgs = "-dist -ide";
		private string FBPath = "fbuild.exe";

		[Category("msfastbuild")]
		[DisplayName("FASTBuild arguments")]
		[Description("Arguments that will be passed to FASTBuild, default \"-dist -ide\"")]
		public string OptionFBArgs
		{
			get { return FBArgs; }
			set { FBArgs = value; }
		}

		[Category("msfastbuild")]
		[DisplayName("FBuild.exe path")]
		[Description("Can be used to specify the path to FBuild.exe")]
		public string OptionFBPath
		{
			get { return FBPath; }
			set { FBPath = value; }
		}
	}

	[PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(FASTBuildPackage.PackageGuidString)]
	[ProvideOptionPage(typeof(OptionPageGrid),
	"msfastbuild", "Options", 0, 0, true)]
	[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    public sealed class FASTBuildPackage : Package
    {
        /// <summary>
        /// FASTBuildPackage GUID string.
        /// </summary>
        public const string PackageGuidString = "b5f4430f-5f92-4bf7-8c27-ed6e0eadacdc";

        public DTE2 m_dte;
        public OutputWindowPane m_outputPane;

        /// <summary>
        /// Initializes a new instance of the <see cref="FASTBuild"/> class.
        /// </summary>
        public FASTBuildPackage()
        {
            // Inside this method you can place any initialization code that does not require
            // any Visual Studio service because at this point the package object is created but
            // not sited yet inside Visual Studio environment. The place to do all the other
            // initialization is the Initialize method.
        }

		public string OptionFBArgs
		{
			get
			{
				OptionPageGrid page = (OptionPageGrid)GetDialogPage(typeof(OptionPageGrid));
				return page.OptionFBArgs;
			}
		}

		public string OptionFBPath
		{
			get
			{
				OptionPageGrid page = (OptionPageGrid)GetDialogPage(typeof(OptionPageGrid));
				return page.OptionFBPath;
			}
		}

		#region Package Members

		/// <summary>
		/// Initialization of the package; this method is called right after the package is sited, so this is the place
		/// where you can put all the initialization code that rely on services provided by VisualStudio.
		/// </summary>
		protected override void Initialize()
        {
            FASTBuild.Initialize(this);
            base.Initialize();

			m_dte = (DTE2)GetService(typeof(DTE));
            OutputWindow outputWindow = m_dte.ToolWindows.OutputWindow;

            m_outputPane = outputWindow.OutputWindowPanes.Add("FASTBuild");
            m_outputPane.OutputString("FASTBuild\r");
		}

        #endregion
    }
}
