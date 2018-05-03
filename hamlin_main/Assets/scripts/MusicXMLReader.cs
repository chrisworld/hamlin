using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Xml;
using System.IO;


public class MusicXMLReader : MonoBehaviour {

  //TextAsset xml_file;
  XmlTextReader rdr;

	// Use this for initialization
	void Start () {

    //xml_file = Resources.Load("musicXML/hamlin_easy.xml") as TextAsset;
    XmlTextReader rdr = new XmlTextReader("Assets/musicXML/hamlin_easy.xml");
    ReadandWrite(rdr);

    if (rdr != null){
      Debug.Log("xml file not found");
    }
    Debug.Log("xml file found");
  }
	
	// Update is called once per frame
	void Update () {
		
	}

  // XML Reader debug
  public void ReadandWrite(XmlReader rdr)
  {
    //Read each node in the tree.
    while (rdr.Read())
    {
       switch (rdr.NodeType)
       {
         case XmlNodeType.Element:
           Debug.Log("<" + rdr.Name);
           while (rdr.MoveToNextAttribute())
             Debug.Log(" " + rdr.Name + "='" + rdr.Value + "'");
             Debug.Log(">");
             if (rdr.IsEmptyElement == true)
                Debug.Log("#EmptyElement");
             else
                Debug.Log("#Element");
             break;
           case XmlNodeType.Text:
             Debug.Log(rdr.Value);
             Debug.Log("#Text");
           break;
           case XmlNodeType.CDATA:
             Debug.Log(rdr.Value);
           break;
           case XmlNodeType.ProcessingInstruction:
             Debug.Log("<?" + rdr.Name + " " + rdr.Value + "?>");
           break;
           case XmlNodeType.Comment:
             Debug.Log("<!--" + rdr.Value + "-->");
           break;
           case XmlNodeType.Document:
             Debug.Log("<?xml version='1.0'?>");
           break;
           case XmlNodeType.Whitespace:
             Debug.Log(rdr.Value);
           break;
           case XmlNodeType.SignificantWhitespace:
             Debug.Log(rdr.Value);
           break;
           case XmlNodeType.EndElement:
             Debug.Log("</" + rdr.Name + ">");
             Debug.Log("#EndElement");
           break;
        }
     }
  }

}