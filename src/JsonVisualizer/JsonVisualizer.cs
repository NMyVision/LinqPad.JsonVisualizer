using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LINQPad;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

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
                var tNode = new TreeNode( prefix );

                AddNode(root, tNode);

                tv.Nodes.Add(tNode);

                tv.ExpandAll();
            }
            finally
            {
                tv.EndUpdate();
            }

            PanelManager.DisplayControl(tv, title);

            return value;
        }

        private static void AddNode(JToken token, TreeNode parent)
        {
            if (token == null)
                return;
            if (token is JValue)
            { }
            else if (token is JObject)
            {
                var obj = (JObject)token;
                foreach (var property in obj.Properties())
                {
                    var childNode = parent.Nodes[parent.Nodes.Add(new TreeNode($"{property.Name} : "))];
                    childNode.Tag = property;
                    if (property.Value is JArray)
                        childNode.Text += "[]";
                    else if (property.Value is JObject)
                        childNode.Text += "{}";
                    else
                        childNode.Text += property.Value.ToString();
                    childNode.ToolTipText = token.ToString();
                    AddNode(property.Value, childNode);
                }
            }
            else if (token is JArray)
            {
                var array = (JArray)token;
                for (int i = 0; i < array.Count; i++)
                {
                    var childNode = new TreeNode($"{i} : {{}}");
                    parent.Nodes.Add(childNode);
                    AddNode(array[i], childNode);
                }
            }
            else
            {
                //Debug.WriteLine(string.Format("{0} not implemented", token.Type)); // JConstructor, JRaw
            }
        }
    }
}
