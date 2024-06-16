using System;
using System.Linq;

using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

using System.IO;

namespace IM999MaxBonum{
    public static class Menus{

        //در این تابع نباید دو منو با نام یکسان باشد 
        //چون در انتخاب زیر منوها از نام منو استفاده میشود
        public static string GetMenu(string menuName, string langMark, int deepCount = 1)
        {
            string menu = "";
            string xpath = "/menu/item";

            //string path = "Menus/"+menuName+"."+langMark+".xml";
            string path = AppDomain.CurrentDomain.BaseDirectory+"/Menus/"+menuName+"."+langMark+".xml";
            
            XDocument doc = XDocument.Load(path);

            XPathNavigator navigator = doc.CreateNavigator();  
            foreach (XPathNavigator node in navigator.Select(xpath))
            {
                int deep = 1;
                string href = "";
                string name = "";
                if(node.HasAttributes){
                    href = node.SelectSingleNode("@url").Value.Trim();
                    name = node.SelectSingleNode("@name").Value.Trim();
                }
                menu+="<li><a href='"+href+"'>"+name+"</a>";
                if(node.HasChildren){
                    if(deepCount> deep){
                        menu+=GetSubMenu(path, deepCount, deep+1, xpath, name);
                    }
                }
                menu+="</li>\n";
            }

            return menu;
        }

        public static string GetSubMenu(string path, int deepCount, int deep, string xpath, string name){
            string menu = "\n<ul>\n";
            XDocument doc = XDocument.Load(path);

            XPathNavigator navigator = doc.CreateNavigator();  
            foreach (XPathNavigator node in navigator.Select(xpath))
            {
                string href = "";
                string _name = "";
                if(node.SelectSingleNode("@name").Value.Trim().ToLower() != name.Trim().ToLower() )
                    continue;

                if(node.HasAttributes){
                    foreach (XPathNavigator nodeChild in node.SelectChildren("item","")){
                        if(node.HasAttributes){
                            href = nodeChild.SelectSingleNode("@url").Value.Trim();
                            _name = nodeChild.SelectSingleNode("@name").Value.Trim();
                        }
                        if(nodeChild.HasChildren){
                            if(deepCount> deep){
                                menu+="<li><a>"+_name+"</a>";
                                menu+=GetSubMenu(path, deepCount, deep+1, xpath+"/item", _name);
                            }else{
                                menu+="<li><a href='"+href+"'>"+_name+"</a>";
                            }
                        }else{
                            menu+="<li><a href='"+href+"'>"+_name+"</a>";
                        }
                        menu+="</li>\n";
                    }
                }
            }
            menu+="</ul>\n";
            
            return menu;
        }

    }
}