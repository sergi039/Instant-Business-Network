namespace Mediachase.UI.Web.Documents.Modules
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using System.Resources;
	using Mediachase.IBN.Business;
	using Mediachase.UI.Web.Util;
	using Mediachase.UI.Web.Modules;

	/// <summary>
	///		Summary description for DocumentsToDo.
	/// </summary>
	public partial class DocumentsToDo : System.Web.UI.UserControl
	{
		public ResourceManager LocRM = new ResourceManager("Mediachase.UI.Web.App_GlobalResources.Documents.Resources.strDocuments", typeof(DocumentsToDo).Assembly);

		#region DocumentId
		private int DocumentId
		{
			get
			{
				try
				{
					return int.Parse(Request["DocumentId"]);
				}
				catch
				{
					throw new Exception("Not valid Document ID!!!");
				}
			}
		}
		#endregion

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.Visible = true;
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}

		/// <summary>
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
		}
		#endregion

		#region BindToolbar
		private void BindToolbar()
		{
			tbToDo.AddText(LocRM.GetString("tbToDo"));
			if (Document.CanAddToDo(DocumentId))
			{
				tbToDo.AddRightLink("<img alt='' src='../Layouts/Images/icons/task_create.gif'/> " + LocRM.GetString("tbAdd"), "../ToDo/ToDoEdit.aspx?DocumentId=" + DocumentId);
			}
		}

		#endregion

		#region BindDataGrid
		private void BindDataGrid()
		{
			dgToDo.Columns[4].Visible = Document.CanDeleteToDo(DocumentId);

			DataTable dt = Document.GetListToDoDataTable(DocumentId);
			DataView dv = dt.DefaultView;

			if (dv.Count == 0)
			{
				this.Visible = false;
				return;
			}

			dv.Sort = "Title";
			dgToDo.DataSource = dv;
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				if (!(bool)dt.Rows[i]["IsCompleted"])
					dt.Rows[i]["ReasonName"] = "";
			}

			dgToDo.Columns[1].HeaderText = LocRM.GetString("Title");
			dgToDo.Columns[2].HeaderText = LocRM.GetString("Status");
			dgToDo.DataBind();
		}

		#endregion

		#region DataGrid Methods
		protected bool GetBool(int val)
		{
			if (val == 1)
				return true;
			else
				return false;

		}

		public string ShowCompletion(bool isCompleted)
		{
			if (isCompleted)
				return LocRM.GetString("Yes");
			return "";
		}

		protected string CompletionString(object ReasonId, object CompletedBy)
		{
			if (ReasonId != DBNull.Value && CompletedBy != DBNull.Value)
				return GetCompletionType((int)ReasonId) + " " + CommonHelper.GetUserStatus((int)CompletedBy);
			else
				return "";
		}

		private string GetCompletionType(int type)
		{
			CompletionReason rsn = (CompletionReason)type;
			switch (rsn)
			{
				case CompletionReason.SuspendedManually:
				case CompletionReason.SuspendedAutomatically:
					return LocRM.GetString("Suspended");
				case CompletionReason.CompletedManually:
					return LocRM.GetString("CompletedByManager");
				case CompletionReason.CompletedAutomatically:
					return LocRM.GetString("CompletedByResource");
				case CompletionReason.NotCompleted:
					return LocRM.GetString("NotCompleted");
			}
			return "";
		}
		#endregion

		#region Page_PreRender
		private void Page_PreRender(object sender, EventArgs e)
		{
			BindDataGrid();
			BindToolbar();
		}
		#endregion

		#region lbDeleteToDoAll_Click
		protected void lbDeleteToDoAll_Click(object sender, System.EventArgs e)
		{
			int ToDoId = int.Parse(hdnID.Value);
			ToDo.Delete(ToDoId);
		}
		#endregion

		//===========================================================================
		// This public property was added by conversion wizard to allow the access of a protected, autogenerated member variable tbToDo.
		//===========================================================================
		public Mediachase.UI.Web.Modules.BlockHeaderLightWithMenu tbToDo
		{
			get { return Migrated_tbToDo; }
			//set { Migrated_tbToDo = value; }
		}
	}
}