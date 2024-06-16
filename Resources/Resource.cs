using System;
using System.Linq;

using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

using System.IO;

namespace IM999MaxBonum{
    public static class Resource{
        
        public static string GetData(string LangMark, string Name){
            
            //string path = "Resources/Resource."+LangMark+".resx";
            string path = AppDomain.CurrentDomain.BaseDirectory+"/Resources/Resource."+LangMark+".resx";
            
            XDocument doc = XDocument.Load(path);

            string value = null;
            XPathNavigator navigator = doc.CreateNavigator();  
            foreach (XPathNavigator node in navigator.Select("/root/data"))
            {
                if(node.SelectSingleNode("@name").Value.Trim().ToLower() == Name.Trim().ToLower() ){
                    value = node.SelectSingleNode("value").Value; 
                    break;
                }               
            }

            return value;
        }
    }
}