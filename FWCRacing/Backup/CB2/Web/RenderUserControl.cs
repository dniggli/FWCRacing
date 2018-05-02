using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.IO;
using System.Web;
using System.Reflection;


//http://jamesewelch.wordpress.com/2008/07/11/how-to-render-a-aspnet-user-control-within-a-web-service-and-return-the-generated-html/
//http://aspnet.4guysfromrolla.com/articles/070407-1.aspx
namespace CodeBase2.Web
{
    /// <summary>
    /// Allows one to Render a UserControl directly to HTML to output from a webservice with the DynamicPopulateExtender.
    /// </summary>
   public static class RenderUserControl
    {
       public static string RenderToHTML(string path, string propertyName, object propertyValue)
       {
           var prop = new Dictionary<string,object>();
           prop.Add(propertyName,propertyValue);

           return RenderToHTML(path, prop);
       }


       public static string RenderToHTML(string path, Dictionary<string,object> propertyNameValuePairs)
       {
           Page pageHolder = new Page();
           UserControl viewControl =
              (UserControl)pageHolder.LoadControl(path);

           if (propertyNameValuePairs == null) throw new NullReferenceException("No Property Values given for UserControl");

           if (propertyNameValuePairs.Count > 0)
           {
                //Load each property value from the dictionary into the usercontrol
               foreach (KeyValuePair<string, object> propertyPair in propertyNameValuePairs)
               {
                   Type viewControlType = viewControl.GetType();
                   PropertyInfo property =
                      viewControlType.GetProperty(propertyPair.Key);

                   if (property != null)
                   {
                       property.SetValue(viewControl, propertyPair.Value, null);
                   }
                   else
                   {
                       throw new Exception(string.Format(
                          "UserControl: {0} does not have a public {1} property.",
                          path, propertyPair.Key));
                   }
                }
           }

           pageHolder.Controls.Add(viewControl);
           StringWriter output = new StringWriter();
           HttpContext.Current.Server.Execute(pageHolder, output, false);
           return output.ToString();
       }

       ///////////////////////////////////////////////////
       // example use from webservice 
       //////////
       //
       //[WebMethod()]
       //public string GetMyUserControlHtml(string contextKey)
       //{
       //   
       //    return htmlResponse =
       //        UserControlRenderer.RenderUserControl(
        //        "myUserControl.ascx", "PropertyName", contextKey);
       //}
       //
        //[WebMethod()]
        //public string GetMyUserControlHtml(string contextKey)
        //{
        ////////contextKey = an AutopsyCaseID//////////
        //
        //    return htmlResponse =
        //        UserControlRenderer.RenderUserControl(
        //        "myUserControl.ascx", "AutID", contextKey);
        ///////the AutID property of the usercontrol is set and the User Control's Page_Load is triggered
        ////////then it is rendered to HTML by the webservice
        //}


    }
}
