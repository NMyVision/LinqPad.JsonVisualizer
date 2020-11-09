using LINQPad;
using Newtonsoft.Json.Linq;
using System;
using System.Windows.Forms;

namespace NMyVision.LinqPad
{
    public static class JsonVisualizer
    {
        /// <summary>
        /// Dump the object to JSON Tree Visualizer
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">Object to be dumpped.</param>
        /// <param name="title">Title for the panel, defaults to 'JSON'.</param>
        /// <returns></returns>
        public static T DumpJson<T>(this T value, string title = "JSON")
        {
            string json = "";
            if (value is string)
            {
                json = value as string;
            }
            else
            {
                json = Newtonsoft.Json.JsonConvert.SerializeObject(value);
            }

            TreeView tv = new TreeView();
            var root = JToken.Parse(json);
            tv.BeginUpdate();

            string prefix = (root is JArray) ? "[]" : "{}";

            try
            {
                var tNode = new TreeNode(prefix);

                AddNode(root, tNode);

                tv.Nodes.Add(tNode);

                tv.ExpandAll();
            }
            finally
            {
                tv.EndUpdate();
            }

#if NETCORE
            tv.Dump(title);
#else
            LINQPad.PanelManager.DisplayControl(tv, title);            
#endif
            return value;
        }

        private static void AddNode(JToken token, TreeNode parent)
        {
            if (token == null || token is JValue)
                return;
            
            if (token is JObject obj)
            {
                foreach (var property in obj.Properties())
                {
                    var childNode = parent.Nodes[parent.Nodes.Add(new TreeNode($"{property.Name} : "))];
                    childNode.Tag = property;
                    if (property.Value is JArray)
                        childNode.Text += "[]";
                    else if (property.Value is JObject)
                        childNode.Text += "{}";
                    else if (property.Value is JValue)
                        childNode.Text += GetValue(property.Value as JValue);

                    childNode.ToolTipText = token.ToString();
                    AddNode(property.Value, childNode);
                }
            }
            else if (token is JArray array)
            {
                for (int i = 0; i < array.Count; i++)
                {
                    var item = array[i];
                    var childNode = new TreeNode($"{i} : {{}}");
                    parent.Nodes.Add(childNode);
                    if (item is JValue jv)
                    {
                        childNode.Text = $"{i} : { GetValue(jv) }";
                    }
                    else
                    {
                        AddNode(item, childNode);
                    }
                }
            }
            else
            {
                //Debug.WriteLine(string.Format("{0} not implemented", token.Type)); // JConstructor, JRaw
            }
        }


        private static string GetValue(JValue item)
        {
            if (item.Value == null) return "NULL";

            return (item.Value is String) ?
                $"\"{ item.Value.ToString() }\"" :
                item.Value.ToString();

        }
    }
}