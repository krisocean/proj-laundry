﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Laundry_Platypus
{
    public partial class OrderDetail : System.Web.UI.Page
    {
        string orderid=null;
        Order order = null;
        /**
       * This function is to initial the page
       * */
        protected void Page_Load(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("garment_id", Type.GetType("System.String"));
            dt.Columns.Add("type_name", Type.GetType("System.String"));
            dt.Columns.Add("abbreviation", Type.GetType("System.String"));
            dt.Columns.Add("activate", Type.GetType("System.String"));
            dt.Columns.Add("amount", Type.GetType("System.String"));
            
            string garment;
             orderid = Request["orderid"].ToString();
            Session["order_id"] = orderid;
            if (orderid != null) { order=System_L.Instance.GetOrder(orderid); }
            if (order != null)
            {
                garment = order.Garment;
                string[] garment_detail=garment.Split(';');
                foreach (string t in garment_detail)
                {
                    string[] t2 = t.Split(',');
                    MySqlDataReader reader = Datacon.getRow("SELECT * FROM tb_Garment_type WHERE garment_id = '"+t2[0]+"';");
                    if (reader.Read())
                    {
                        DataRow dr = dt.NewRow();
                        dr["garment_id"] = reader["garment_id"];
                        dr["type_name"] = reader["type_name"]; 
                        dr["abbreviation"] = reader["abbreviation"];
                        dr["activate"] = reader["activate"];
                        dr["amount"] = t2[1];
                       dt.Rows.Add(dr);
                    }
                }
                DataList1.DataSource = dt;
                DataList1.DataBind();
            }
            if (IsPostBack != true)
            {
                MySqlDataReader reader2 = Datacon.getRow("SELECT note FROM tb_Order WHERE order_id = '" + orderid + "';");
                reader2.Read();
                TextBox2.Text = reader2["note"].ToString();
            }

        }
        /**
       * This function is to update the state of order and distribute them by using the system
       * */
        protected void Button1_Click(object sender, EventArgs e)
        {
            string garment=null;
            orderid = Session["order_id"].ToString();
            if (orderid != null)
            {
                if (System_L.Instance.Distribute(orderid))
                {
                    foreach (DataListItem item in DataList1.Items)
                    {
                        Label label1 = item.FindControl("Label1") as Label;
                        string garment_id = label1.Text;
                        TextBox textb = item.FindControl("TextBox1") as TextBox;
                        string garment_amount = textb.Text;
                        garment = garment + garment_id + "," + garment_amount + ";";
                    }
                    order.Garment = garment;
                    if (System_L.Instance.SaveOrder(order))
                    {
                        Datacon.execSQL("UPDATE tb_Order SET note = '" + TextBox2.Text + "' WHERE order_id = '" + orderid + "';");
                        if (Session["role_id"].ToString() == "1")
                        {
                            Response.Redirect("Manager.aspx");
                        }
                        if (Session["role_id"].ToString() == "2")
                        {
                            Response.Redirect("PackerOverview.aspx");
                        }
                        if (Session["role_id"].ToString() == "3")
                        {
                            Response.Redirect("DriverOverview.aspx");
                        }
                        if (Session["role_id"].ToString() == "4")
                        {
                            Response.Redirect("login.aspx");
                        }

                    }
                    else
                    {
                        Response.Write("< script lanuage = javascript > alert('failed'); window.location.href = 'driveroverview.aspx' </ script >");

                    }
                }
                else {
                    Response.Write("< script lanuage = javascript > alert('failed'); window.location.href = 'driveroverview.aspx' </ script >");
                }
            }
        }
        /**
       * This function is to redirect the pages according to different role id for the back button
       * */
        protected void Button2_Click(object sender, EventArgs e)
        {
            if (Session["role_id"].ToString() == "1")
            {
                Response.Redirect("Manager.aspx");
            }
            if (Session["role_id"].ToString() == "2")
            {
                Response.Redirect("PackerOverview.aspx");
            }
            if (Session["role_id"].ToString() == "3")
            {
                Response.Redirect("DriverOverview.aspx");
            }
            if (Session["role_id"].ToString() == "4")
            {
                Response.Redirect("login.aspx");
            }
        }
    }
}