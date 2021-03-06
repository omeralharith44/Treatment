﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Treatment.Entity;
using Website.Classes;

namespace Treatment
{
    public partial class MasterEn : System.Web.UI.MasterPage
    {
        ECMSEntities db = new ECMSEntities();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["IsLocked"] != null)
                if ((bool)Session["IsLocked"])
                    Response.Redirect("~/Pages/Setting/admin/LockScreen.aspx");
            LoadBreadcrumb();
            LoadMenu();
        }

        private void LoadBreadcrumb()
        {
            string str = "<li class='breadcrumb-item'><a href = '~/' ><i class='feather icon-home'></i></a></li>";

            List<string> breadcrumbs = new List<string>();

            string Current_PageName = "DashBoard";

            string LocalPath =  String.Concat(HttpContext.Current.Request.Url.LocalPath.Skip(1));
            try
            {
                List<Permission> ListPermission = db.Permissions.ToList();
                Permission CurrentPage  =  ListPermission.Where(x => x.Url_Path == LocalPath).First();

                if (CurrentPage != null)
                {
                    Current_PageName = CurrentPage.Permission_Name_En;
                    do
                    {
                        breadcrumbs.Add(CurrentPage.Permission_Name_En);
                        CurrentPage = ListPermission.Where(x => x.Permission_Id == CurrentPage.Parent).First();
                    }
                    while (CurrentPage.Parent != 0);
                    breadcrumbs.Add(CurrentPage.Permission_Name_En);
                    for (int i = breadcrumbs.Count - 1; i > 0; i--)
                    {
                        str += "<li class='breadcrumb-item'><a href='#'> " + breadcrumbs[i] + "</a> </li>";
                    }
                }
                PageName.Text = Current_PageName;
                breadcrumb.Text = str;
            }
            catch { breadcrumb.Text = str; PageName.Text = Current_PageName; }
        }

        private void LoadMenu()
        {
            List<Permission> Permission_List = db.Permissions.ToList();
            string str = string.Empty;
            try
            {
                str += "<ul class='pcoded-item pcoded-left-item'>";
                str += "<li class=''>";
                str += "<a href = '../../../../' > ";
                str += "<span class='pcoded-micon active'><i class='feather icon-home'></i></span>";
                str += "<span class='pcoded-mtext'>Dashboard</span>";
                str += "</a>";
                str += "</li>";

                for (int First_Level = 0; First_Level < Permission_List.Count; First_Level++)
                {
                    if (Permission_List[First_Level].Parent == 0)
                    {
                        str += "<li class='pcoded-hasmenu active '>";
                        str += "<a href = 'javascript:void(0)' > ";
                        str += "<span class='pcoded-micon'><i class='"+ Permission_List[First_Level].Permission_Icon+ "'></i></span>";
                        str += "<span class='pcoded-mtext'>"+ Permission_List[First_Level].Permission_Name_En + "</span>";
                        str += "</a>";
                        str += "<ul class='pcoded-submenu'>";
                        List<Permission> Second_List = Permission_List.Where(x => x.Parent == Permission_List[First_Level].Permission_Id).ToList();
                        for (int Second_Level = 0; Second_Level < Second_List.Count; Second_Level++)
                        {
                            List<Permission> Third_List = Permission_List.Where(x => x.Parent == Second_List[Second_Level].Permission_Id).ToList();
                            if (Third_List.Count > 0)
                            {
                                str += "<li class='pcoded-hasmenu active '>";
                                str += "<a href = 'javascript:void(0)' > ";
                                str += "<span class='pcoded-micon'><i class='"+ Second_List[Second_Level].Permission_Icon+ "'></i></span>";
                                str += "<span class='pcoded-mtext'>"+ Second_List[Second_Level].Permission_Name_En + "</span>";
                                str += "</a>";
                                str += "<ul class='pcoded-submenu'>";
                                for (int Third_Level = 0; Third_Level < Third_List.Count; Third_Level++)
                                {
                                    str += "<li class=''>";
                                    str += "<a href = '../../../../" + Third_List[Third_Level].Url_Path + "' > ";
                                    str += "<span class='pcoded-mtext'>"+ Third_List[Third_Level].Permission_Name_En + "</span>";
                                    str += "</a>";
                                    str += "</li>";
                                }
                                str += "</ul>";
                                str += "</li>";
                            }
                            else
                            {
                                str += "<li class=''>";
                                str += "<a href = '../../../../" + Second_List[Second_Level].Url_Path + "' > ";
                                str += "<span class='pcoded-mtext'>" + Second_List[Second_Level].Permission_Name_En + "</span>";
                                str += "</a>";
                                str += "</li>";
                            }
                        }
                        str += "</ul>";
                        str += "</li>";
                    }
                }
                str += "</li>";
                str += "</ul>";
            }
            catch { }
            Menu.Text = str;
        }

        
    }
}