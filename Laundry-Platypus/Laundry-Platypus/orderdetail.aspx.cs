﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Laundry_Platypus
{
    public partial class orderdetail : System.Web.UI.Page
    {
        /**
       * This function is get the; order detail from database and show it on page
       * */
        protected void Page_Load(object sender, EventArgs e)
        {
            string orderid = Request["orderid"];
            if (orderid != null)
            {
                if (Session["role_id"].ToString() == "1")
                {
                    Response.Redirect("orderdetail_p.aspx?orderid=" + orderid);
                }
                if (Session["role_id"].ToString() == "2")
                {
                    Response.Redirect("orderdetail_p.aspx?orderid=" + orderid);
                }
                if (Session["role_id"].ToString() == "3")
                {
                    Response.Redirect("orderdetail_p.aspx?orderid=" + orderid);
                }
                else
                {
                    Response.Redirect("login.aspx");
                }
            }
        }
    }
}