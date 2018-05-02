using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CodeBase2
{
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Concatenates a collection's elements together using the given separator string
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string ToString(this IEnumerable<string> collection, string separator)
        {
            if (collection.Count() == 0) return "";
            if (collection.Count() == 1) return collection.First();
            return collection.Aggregate((workingSentence, next) => workingSentence + separator + next);
        }

        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            foreach (T item in collection)
                action(item);
        }
        public static void ForEach<T,T2>(this IEnumerable<T> collection, Action<T,T2> action,T2 value)
        {
            foreach (T item in collection)
                action(item,value);
        }
        public static void ForEach<T, T2, T3>(this IEnumerable<T> collection, Action<T,T2,T3> action, T2 value,T3 value2)
        {
            foreach (T item in collection)
                action(item,value,value2);
        }

        ///// <summary>
        ///// This is essentially the same as the linq Aggregate function. When I made this I didn't know Aggregate existed. Performs a ForEach that chains outputs and inputs together so the output of the precediing Item is the input of the next Item, finally returning the value
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <typeparam name="TResult"></typeparam>
        ///// <param name="collection"></param>
        ///// <param name="func"></param>
        ///// <param name="value"></param>
        //public static TResult ForEachOutputIsInput<T,TResult>(this IEnumerable<T> collection, Func<T, TResult, TResult> func, TResult value)
        //{
        //    foreach (T item in collection)
        //      value = func(item, value);
        //    return value;
        //}

    }
    public static class LINQExtensionMethods
    {
        public static string ToTraceString(this IQueryable query)
        {
            System.Reflection.MethodInfo toTraceStringMethod = query.GetType().GetMethod("ToTraceString");

            if (toTraceStringMethod == null)
                return "";
            else
                return toTraceStringMethod.Invoke(query, null).ToString();

        }

        /// <summary>
        /// Performs a linq style Where command on the ListItemCollection and sets the collection to the output
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static void Where(this ListItemCollection collection, Func<ListItem, bool> predicate)
        {
            var l = new List<ListItem>();
            foreach (ListItem li in collection) l.Add(li);             
            
            collection.Clear();
            collection.AddRange(l.Where(predicate).ToArray());
        }
    }
    public static class AspControls
    {
        public static string ToHTMLString(this Control ctrl)
        {
            var sb = new StringBuilder();
            var tw = new StringWriter(sb);
            var hw = new HtmlTextWriter(tw);

            ctrl.RenderControl(hw);
            return sb.ToString();
        }
    }

}
