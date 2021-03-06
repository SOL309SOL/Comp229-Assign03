﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Comp229_Assign03
{
    public partial class StudentPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Displaying List and linking to the other page with button
            if (String.IsNullOrEmpty(Request["StudentID"]))
            {
                Response.Write("Wrong request");
                Response.End();
            }
            else
            {
                DisplayData();
                lnkMemoModify.NavigateUrl =
                    $"ModifyPage.aspx?StudentID={Request["StudentID"]}";
                lnkMemoDelete.NavigateUrl =
                    $"DeletePage.aspx?StudentID={Request["StudentID"]}";
            }

        }
        private void DisplayData()
        {
            //Display Selected Student Data using procedure.

            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Comp229Assign03"].ConnectionString;
            con.Open();

            SqlCommand cmd = new SqlCommand("ViewMemo15", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add("StudentID", SqlDbType.Int);
            cmd.Parameters["StudentID"].Value = Convert.ToInt32(Request["StudentID"]);

            cmd.Parameters.Add("CourseID", SqlDbType.Int);
            cmd.Parameters["CourseID"].Value = Convert.ToInt32(Request["CourseID"]);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();

            da.Fill(ds);

            CourseGridView.DataSource = ds;
            CourseGridView.DataBind();



            SqlDataReader dr = cmd.ExecuteReader();

            


            if (dr.Read())
            {
                this.lblId.Text = Request["StudentID"];
                this.lblFname.Text = dr["FirstMidName"].ToString();
                this.lblLname.Text = dr["LastName"].ToString();
                this.lblDate.Text = dr["EnrollmentDate"].ToString();
              
            }
            else
            {
                Response.Write("Cannot Find Data");
                Response.End();
            }
            dr.Close();
            con.Close();
        }
    }
}