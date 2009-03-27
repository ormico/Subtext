#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext-devs@lists.sourceforge.net 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using System.Web;
using Subtext.Extensibility.Providers;
using Subtext.Framework;
using Subtext.Framework.Web;

namespace Subtext.Framework.Web.HttpModules
{
    /// <summary>
    /// Checks to see if the blog needs an upgrade.
    /// </summary>
    public class InstallationCheckModule : IHttpModule
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InstallationCheckModule"/> class.
        /// </summary>
        public InstallationCheckModule() {
        }

        /// <summary>
        /// Initializes a module and prepares it to handle
        /// requests.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpApplication"/> that provides access to the methods, properties, 
        /// and events common to all application objects within an ASP.NET application</param>
        public void Init(HttpApplication context)
        {
            context.BeginRequest += CheckInstallationStatus;
        }

        /// <summary>
        /// Disposes of the resources (other than memory) used by the
        /// module that implements <see langword="IHttpModule."/>
        /// </summary>
        public void Dispose()
        {
            //Do nothing.
        }

        /// <summary>
        /// Checks the installation status and redirects if necessary.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        public void CheckInstallationStatus(object sender, EventArgs e)
        {
            //Bypass for static files.
            if (HttpHelper.IsStaticFileRequest())
                return;

            BlogRequest blogRequest = BlogRequest.Current;

            if (HostInfo.Instance == null && blogRequest.RequestLocation != RequestLocation.Installation) {
                HttpContext.Current.Response.Redirect("~/Install/", true);
            }

            InstallationManager installationManager = new InstallationManager(Installation.Provider);

            // Want to redirect to install if installation is required, 
            // or if we're missing a HostInfo record.
            if ((installationManager.IsInstallationActionRequired(VersionInfo.FrameworkVersion) || HostInfo.Instance == null))
            {
                InstallationState state = Installation.Provider.GetInstallationStatus(VersionInfo.FrameworkVersion);
                if(state == InstallationState.NeedsInstallation 
                    && !blogRequest.IsHostAdminRequest
                    && blogRequest.RequestLocation != RequestLocation.Installation)
                {
                    HttpContext.Current.Response.Redirect("~/Install/", true);
                    return;
                }

                if(state == InstallationState.NeedsUpgrade || state == InstallationState.NeedsRepair)
                {
                    if (blogRequest.RequestLocation != RequestLocation.Upgrade 
                        && blogRequest.RequestLocation != RequestLocation.LoginPage 
                        && blogRequest.RequestLocation == RequestLocation.SystemMessages)
                    {
                        HttpContext.Current.Response.Redirect("~/SystemMessages/UpgradeInProgress.aspx", true);
                        return;
                    }
                }
            }
        }
    }
}