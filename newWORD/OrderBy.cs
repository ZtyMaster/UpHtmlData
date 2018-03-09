using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace newWORD
{
    public class OrderBy
    {

     public static    XmlDocument SortXMLDoc(XmlDocument xmldoc)
        {
            XmlDocument xmlDocCopy = new XmlDocument();
            xmlDocCopy.LoadXml(xmldoc.OuterXml);
            XmlNode XmlNodeCopy = xmlDocCopy.SelectSingleNode("//class1");
            if (XmlNodeCopy == null)
               return  new XmlDocument();
            XmlNodeCopy.RemoveAll();

            //XmlNodeCopy.InnerXml = "<NewsCategory Category=\"NA\"></NewsCategory>";


            XmlNode node = xmldoc.SelectSingleNode("//class1");
            XPathNavigator navigator = node.CreateNavigator();
            XPathExpression selectExpression = navigator.Compile("Lists/Count");

            selectExpression.AddSort(".", XmlSortOrder.Descending, XmlCaseOrder.None, "", XmlDataType.Text);

            //DateTimeComparer datesort = new DateTimeComparer();
            //selectExpression.AddSort(".", datesort);
            XPathNodeIterator nodeIterator = navigator.Select(selectExpression);
            while (nodeIterator.MoveNext())
            {
                //XmlNode currentNode = (XmlNode)nodeIterator.Current.ValueAs( typeof(XmlNode)) ;

                XmlNode linkNode = xmldoc.SelectSingleNode("//Lists[Count=\"" + nodeIterator.Current.Value + "\"]");
                XmlNode importedLinkNode = xmlDocCopy.ImportNode(linkNode, true);
                xmlDocCopy.SelectSingleNode("//class1").AppendChild(importedLinkNode);
            }

            return xmlDocCopy;
        }
    }
}
