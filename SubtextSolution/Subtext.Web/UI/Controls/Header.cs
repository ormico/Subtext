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
using Subtext.Web.Controls;

namespace Subtext.Web.UI.Controls
{
	/// <summary>
	///	Used to display the header.
	/// </summary>
	public class Header : BaseControl
	{
		protected System.Web.UI.WebControls.HyperLink HeaderTitle;
		protected System.Web.UI.WebControls.Literal HeaderSubTitle;
	
		protected string Title
		{
			get;
			set;
		}

		public string Subtitle
		{
			get;
			set;
		}

		protected string HomeUrl
		{
			get;
			set;
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);

			this.Title = Blog.Title;
			this.HomeUrl = Url.BlogUrl();
			this.Subtitle = Blog.SubTitle;

			if (null != this.FindControl("HeaderTitle"))
			{
				HeaderTitle.NavigateUrl = HomeUrl;
				HeaderTitle.Text = Title;
				ControlHelper.SetTitleIfNone(HeaderTitle, "The Title Of This Blog.");
			}
			
			if (null != this.FindControl("HeaderSubTitle"))
			{
				HeaderSubTitle.Text = Subtitle;
			}
		}

		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);
			this.DataBind();
		}
	}
}

