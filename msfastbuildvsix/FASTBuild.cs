// Copyright 2016 Liam Flookes and Yassine Riahi
// Available under an MIT license. See license file on github for details.

using System;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.VCProjectEngine;

using System.Reflection;
using System.IO;
using EnvDTE;
using EnvDTE80;
using System.Text;

namespace msfastbuildvsix
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class FASTBuild
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 0x0100;
		public const int SlnCommandId = 0x0101;
		public const int ContextCommandId = 0x0102;
		public const int SlnContextCommandId = 0x0103;

		/// <summary>
		/// Command menu group (command set GUID).
		/// </summary>
		public static readonly Guid CommandSet = new Guid("7c132991-dea1-4719-8c67-c20b24b6775c");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly Package package;

        /// <summary>
        /// Initializes a new instance of the <see cref="FASTBuild"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        private FASTBuild(Package package)
        {
            if (package == null)
            {
                throw new ArgumentNullException("package");
            }

            this.package = package;

            OleMenuCommandService commandService = this.ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (commandService != null)
            {
                var menuCommandID = new CommandID(CommandSet, CommandId);
                var menuItem = new MenuCommand(this.MenuItemCallback, menuCommandID);
                commandService.AddCommand(menuItem);

				menuCommandID = new CommandID(CommandSet, SlnCommandId);
				menuItem = new MenuCommand(this.MenuItemCallback, menuCommandID);
				commandService.AddCommand(menuItem);

				menuCommandID = new CommandID(CommandSet, ContextCommandId);
				menuItem = new MenuCommand(this.MenuItemCallback, menuCommandID);
				commandService.AddCommand(menuItem);

				menuCommandID = new CommandID(CommandSet, SlnContextCommandId);
				menuItem = new MenuCommand(this.MenuItemCallback, menuCommandID);
				commandService.AddCommand(menuItem);
			}
		}

		/// <summary>
		/// Gets the instance of the command.
		/// </summary>
		public static FASTBuild Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        private IServiceProvider ServiceProvider
        {
            get
            {
                return this.package;
            }
        }

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static void Initialize(Package package)
        {
            Instance = new FASTBuild(package);
        }

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void MenuItemCallback(object sender, EventArgs e)
        {
            FASTBuildPackage fbPackage = (FASTBuildPackage)this.package;
            if (null == fbPackage.m_dte.Solution)
                return;

			MenuCommand eventSender = sender as MenuCommand;

			if (eventSender == null)
			{
				fbPackage.m_outputPane.OutputString("VSIX failed to cast sender to OleMenuCommand.\r");
				return;
			}

			fbPackage.m_outputPane.Activate();
			fbPackage.m_outputPane.Clear();

			string fbCommandLine = "";
			string fbWorkingDirectory = "";

			Solution sln = fbPackage.m_dte.Solution;
			SolutionBuild sb = sln.SolutionBuild;
			SolutionConfiguration2 sc = sb.ActiveConfiguration as SolutionConfiguration2;
			VCProject proj = null;

			if (eventSender.CommandID.ID != SlnCommandId && eventSender.CommandID.ID != SlnContextCommandId)
			{
				if (fbPackage.m_dte.SelectedItems.Count > 0)
				{
					Project envProj = (fbPackage.m_dte.SelectedItems.Item(1).Project as EnvDTE.Project);
					if (envProj != null)
					{
						proj = envProj.Object as VCProject;
					}
				}

				if (proj == null)
				{
					string startupProject = "";
					foreach (String item in (Array)sb.StartupProjects)
					{
						startupProject += item;
					}
					proj = sln.Item(startupProject).Object as VCProject;
				}

				if (proj == null)
				{
					fbPackage.m_outputPane.OutputString("No valid vcproj selected for building or set as the startup project.\r");
					return;
				}

				fbPackage.m_outputPane.OutputString("Building " + Path.GetFileName(proj.ProjectFile) + " " + sc.Name + " " + sc.PlatformName + "\r");
				fbCommandLine = string.Format("-p \"{0}\" -c {1} -f {2} -s \"{3}\" -a\"{4}\" -b \"{5}\"", Path.GetFileName(proj.ProjectFile), sc.Name, sc.PlatformName, sln.FileName, fbPackage.OptionFBArgs, fbPackage.OptionFBPath);
				fbWorkingDirectory = Path.GetDirectoryName(proj.ProjectFile);
			}
			else
			{
				fbCommandLine = string.Format("-s \"{0}\" -c {1} -f {2} -a\"{3}\" -b \"{4}\"", sln.FileName, sc.Name, sc.PlatformName, fbPackage.OptionFBArgs, fbPackage.OptionFBPath);
				fbWorkingDirectory = Path.GetDirectoryName(sln.FileName);
			}

            string msfastbuildPath = Assembly.GetAssembly(typeof(msfastbuild.msfastbuild)).Location;
            try
            {
				fbPackage.m_outputPane.OutputString("Launching msfastbuild with command line: " + fbCommandLine + "\r");

				System.Diagnostics.Process FBProcess = new System.Diagnostics.Process();
                FBProcess.StartInfo.FileName = msfastbuildPath;
				FBProcess.StartInfo.Arguments = fbCommandLine;
				FBProcess.StartInfo.WorkingDirectory = fbWorkingDirectory;
                FBProcess.StartInfo.RedirectStandardOutput = true;
                FBProcess.StartInfo.UseShellExecute = false;
                FBProcess.StartInfo.CreateNoWindow = true;
                FBProcess.StartInfo.StandardOutputEncoding = Encoding.GetEncoding("ibm850");

                System.Diagnostics.DataReceivedEventHandler OutputEventHandler = (Sender, Args) => {
                    if (Args.Data != null)
                        fbPackage.m_outputPane.OutputString(Args.Data + "\r");
                };

                FBProcess.OutputDataReceived += OutputEventHandler;
                FBProcess.Start();
                FBProcess.BeginOutputReadLine();
                //FBProcess.WaitForExit();
            }
            catch (Exception ex)
            {
				fbPackage.m_outputPane.OutputString("VSIX exception launching msfastbuild. Could be a broken VSIX? Exception: " + ex.Message + "\r");
            }
        }
    }
}
