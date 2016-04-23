using KTraining.Resources.Controllers;
using System;
using System.Linq;

namespace KTraining.Controllers
{
    public class ConvertResource
    {
        // Convert coded value to his real value from resources
        public string ConvertContentCode(string content)
        {

            if (content != null && content.Count(c => c == '^') == 2)
            {
                int startIndex = content.IndexOf("^") + 1;
                int endIndex = content.LastIndexOf("^") - 1;
                string code = content.Substring(
                  startIndex, endIndex - startIndex + 1);
                var codeValue = Gtresource(code);
                content = content.Replace(code, codeValue);
                content = content.Remove(content.IndexOf("^"), 1);
                content = content.Remove(content.LastIndexOf("^"), 1);
            }
            if (content != null && content.Count(c => c == '^') > 2)
            {
                int size = content.Count(c => c == '^');
                for (int i = 0; i < size / 2; i++)
                {
                    int startIndex = content.IndexOf("^") + 1;
                    int endIndex = IndexOfSecond(content, "^") - 1;
                    string code = content.Substring(
                      startIndex, endIndex - startIndex + 1);
                    var codeValue = Gtresource(code);
                    content = content.Replace(code, codeValue);
                    content = content.Remove(content.IndexOf("^"), 1);
                    content = content.Remove(content.IndexOf("^"), 1);
                }
            }
            return content;

        }

        //Get element from second index
        int IndexOfSecond(string theString, string toFind)
        {
            int first = theString.IndexOf(toFind);

            if (first == -1) return -1;

            // Find the "next" occurrence by starting just past the first
            return theString.IndexOf(toFind, first + 1);
        }

        //Get resourse by key
        private string Gtresource(string rulename)
        {
            string value = null;
            var rM = new System.Resources.ResourceManager((typeof(Common)));
            value = rM.GetString(rulename).ToString();

            if (value != null && value != "")
            {
                return value;
            }
            else
            {
                return "";
            }

        }
    }
}