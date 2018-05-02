using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace CodeBase2
{
    // http://allantech.blogspot.com/2007/06/generic-type-conversion-in-c.html

    public static class GenericTypeConverter
    {

        public static DestType ConvertType<SrcType, DestType>(SrcType Source) where DestType : class, new()
        {
            return ConvertType<SrcType, DestType>(Source, null, null);
        }

        public static DestType ConvertType<SrcType, DestType>(SrcType Source, Dictionary<string, string> SrcDestMapping) where DestType : class, new()
        {
            return ConvertType<SrcType, DestType>(Source, SrcDestMapping, null);
        }

        private static string GetMappedName(Dictionary<string, string> Map, string OrigName)
        {
            if ((Map != null) && (Map.ContainsKey(OrigName))) return Map[OrigName];
            return OrigName;
        }

        /// <summary>
        /// Uses reflection to convert an object to a destination type, e.g. transfers all the properties and members they have in common
        /// </summary>
        /// <typeparam name="SrcType">Source Type</typeparam>
        /// <typeparam name="DestType">Destination Type</typeparam>
        /// <param name="Source">Object to convert</param>
        /// <param name="SrcDestMap">Mapping between source and destination property names. Null if no mapping exist.</param>
        /// <param name="Dest">Destination object or null if it should be created</param>
        /// <returns>An object where as many properties and fields as possible have been transferred from Source.</returns>
        private static DestType ConvertType<SrcType, DestType>(SrcType Source, Dictionary<string, string> SrcDestMap, DestType Dest) where DestType : class
        {
            //Create object if it doesn't exist.
            DestType dstVar = Dest;
            if (dstVar == null) dstVar = Activator.CreateInstance<DestType>();

            //Loop through Source' public properties
            Type srcTp = typeof(SrcType);
            PropertyInfo[] props = srcTp.GetProperties(System.Reflection.BindingFlags.Public
            | System.Reflection.BindingFlags.Instance
            | System.Reflection.BindingFlags.Static
            | System.Reflection.BindingFlags.GetProperty);
            foreach (PropertyInfo p in props)
            {
                //Check if destination type has a settable property of the same type
                PropertyInfo pDest = typeof(DestType).GetProperty(GetMappedName(SrcDestMap, p.Name), p.PropertyType);
                if ((pDest != null) && (pDest.CanWrite)) pDest.SetValue(dstVar, p.GetValue(Source, null), null);
            }

            //Loop through Source' public fields
            FieldInfo[] mems = srcTp.GetFields();
            foreach (FieldInfo fi in mems)
            {
                FieldInfo mDest = typeof(DestType).GetField(GetMappedName(SrcDestMap, fi.Name));
                if ((mDest != null) && (fi.FieldType == mDest.FieldType))
                {
                    mDest.SetValue(dstVar, fi.GetValue(Source));
                }
            }

            return dstVar;
        }
    }
}
