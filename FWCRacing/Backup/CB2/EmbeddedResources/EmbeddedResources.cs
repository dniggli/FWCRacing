using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Drawing;
using System.Reflection;

namespace CodeBase2.EmbeddedResources
{
   public static class Resource
    {
        /// <summary>
        /// Extracts an embedded file out of a given assembly.
        /// </summary>
        /// <param name="assemblyName">The namespace of your assembly.</param>
        /// <param name="fileName">The name of the file to extract.</param>
        /// <returns>A stream containing the file data.</returns>
        public static Stream GetEmbeddedFile(string space, string fileName)
        {
            try
            {
                System.Reflection.Assembly a = System.Reflection.Assembly.Load(space.Split('.')[0]);
                Stream str;
                //if (assemblyName == "CB2" || assemblyName == "CodeBase2")
                //{
                //    str = a.GetManifestResourceStream(fileName);
                //}
                //else
                //{
                str = a.GetManifestResourceStream(space + "." + fileName);
               // }
                

                if (str == null)
                    throw new Exception("Could not locate embedded resource '" + fileName + "' in namespace '" + space + "'");
                return str;
            }
            catch (Exception e)
            {
                throw new Exception(space + ": " + e.Message);
            }
        }
       /*
           * The resourceName is case sensitive.
    * The resourceName must be the fully qualified name of the file: Default Namespace + folder name(s) + filename (with extension if applicable).
    * Your code file will need to have using statements for System.Reflection and System.IO. If you only change the text of an embedded resource file, you will need to rebuild your project because Visual Studio does not recognize the change as a code change worthy of recompiling. 

        string quote =
   new EmbeddedResourceTextReader().GetFromResources
   ("McKechney.EmbeddedResouceTextExample.Folder.Shakespeare.txt");
        */

       /// <summary>
       /// Gets embedded resource string
       /// </summary>
       /// <param name="assem">the Assembly containing the file</param>
       /// <param name="resourceName">Name.Space.And.Name.Of.File</param>
       /// <returns></returns>
        public static string GetEmbeddedString(Assembly assem, string resourceName)
        {
            using (Stream stream = assem.GetManifestResourceStream(resourceName))
            {
                try
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        return reader.ReadToEnd();
                    }
                }
                catch (Exception e)
                {
                    throw new Exception("Error retrieving from Resources. Tried '"
                                             + resourceName + "'\r\n" + e.ToString());
                }
            }
        }

        /// <summary>
        /// Gets embedded resource
        /// </summary>
        /// <param name="assem">the Assembly containing the file</param>
        /// <param name="resourceName">Name.Space.And.Name.Of.File</param>
        /// <returns></returns>
        public static Stream GetEmbeddedFile(Assembly assem, string resourceName)
        {
            return assem.GetManifestResourceStream(resourceName);
        }

        /// <summary>
        /// Gets embedded resource from calling Assembly
        /// </summary>
        /// <param name="resourceName">Name.Space.And.Name.Of.File</param>
        /// <returns></returns>
        public static Stream GetEmbeddedFile(string resourceName)
        {
            return Assembly.GetCallingAssembly().GetManifestResourceStream(resourceName);
        }

        /// <summary>
        /// Gets embedded resource string from calling assembly
        /// </summary>
        /// <param name="resourceName">Name.Space.And.Name.Of.File</param>
        /// <returns></returns>
        public static string GetEmbeddedString(string resourceName)
        {
            return GetEmbeddedString(Assembly.GetCallingAssembly(), resourceName);
        }

        /// <summary>
        /// Gets embedded resource string from calling assembly
        /// </summary>
        /// <param name="resourceName">Name.Space.And.Name.Of.File</param>
        /// <returns></returns>
        public static string FormatEmbeddedJS(string resourceName,params object[] args)
        {
           string js = GetEmbeddedString(Assembly.GetCallingAssembly(), resourceName);

           //modify the javascript so this works like String.format()
           for (int i = 0; i < args.Length; i++)
           {
               js = js.Replace("{" + i + "}", Convert.ToString(args[i]));
           }

           return "<script type='text/javascript'>\n" + js+  "\n</script>";
        }
       

       public static XmlDocument GetEmbeddedXml(Type type, string fileName)
       {
           Stream str = GetEmbeddedFile(type, fileName);
           XmlTextReader tr = new XmlTextReader(str);
           XmlDocument xml = new XmlDocument();
           xml.Load(tr);
           return xml;
       }


       public static Bitmap GetEmbeddedImage(Type type, string fileName)
       {
           Stream str = GetEmbeddedFile(type, fileName);
           return new Bitmap(str);
       }

       //public static Stream GetEmbeddedFile(System.Reflection.Assembly assembly, string fileName)
       //{
       //    string assemblyName = assembly.GetName().Name;
       //    return GetEmbeddedFile(assemblyName, fileName);
       //}


       public static Stream GetEmbeddedFile(Type type, string fileName)
       {
           string assemblyName = type.Assembly.GetName().Name;
           return GetEmbeddedFile(assemblyName, fileName);
       }
    }
   public static class Testing
   {
       /// <summary>
       /// Retrieve TestData for PathDirectory
       /// </summary>
       public static Stream Data
       {
           get
           {
             return Resource.GetEmbeddedFile("CodeBase2.EmbeddedResources.TestData", "TestData.xml");
           }
       }

       /// <summary>
       /// Retrieve TestSchema for PathDirectory
       /// </summary>
       public static Stream Schema
       {
           get
           {
               return Resource.GetEmbeddedFile("CodeBase2.EmbeddedResources.TestData", "PathDirectory.xsd");
           }
       }
   }
}
