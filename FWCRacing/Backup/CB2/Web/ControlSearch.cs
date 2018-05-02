using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace CodeBase2.Web
{
    public static class ControlSearch
    {
        public const string WebSite = "CLSSwebsite@urmc.rochester.edu";

        /// <summary>
        /// Recursively searches the "root" control and its children for a control matching "id"
        /// One would usually specify root as the form control of the page - Me.Form
        /// </summary>
        /// <param name="root"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static Control FindControlRecursive(ref Control root, string id)
        {
            string bugger = root.ClientID;
            //looking at this value gets rid of a really strange bug, or we would not need this line
            if (root.ID.ToLower() == id.ToLower())
            {
                return root;
            }

            for (int x = 0; x < root.Controls.Count; x++)
            {
                Control c = root.Controls[x];
                Control t = FindControlRecursive(ref c, id);
                if (((t != null)))
                {
                    return t;
                }
            }

            return null;
        }

        /// <summary>
        /// Recursively searches the "root" control and its children and populates a list of controls
        /// One would usually specify root as the form control of the page - Me.Form
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static List<Control> ControlList(Control root)
        {
            string bugger = root.ClientID;
            //looking at this value gets rid of a really strange bug, or we would not need this line
            List<Control> a = new List<Control>();
            a.Add(root);

            for (int x = 0; x < root.Controls.Count; x++)
            {
                Control c = root.Controls[x];
                List<Control> al = ControlList(c);

                if (((al != null)))
                {
                    a.AddRange(al);
                }
            }

            return a;
        }

        /// <summary>
        /// Recursively searches the "root" control and its children and populates a list of controls
        /// One would usually specify root as the form control of the page - Me.Form
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static List<T> ControlList<T>(Control root) where T : Control
        {
            List<Control> list = ControlList(root);
            List<T> controltypes = new List<T>();
            foreach (Control c in list)
            {
                if (c is T)
                {
                    controltypes.Add((T)c);
                }
            }
            return controltypes;
        }
    }
}



