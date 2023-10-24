using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;

namespace Platformer.Dialogue
{
   [XmlRoot("dialogue")]
   public  class Dialog
   {
      [XmlElement("node")]
      public Node[] nodes;

      public static Dialog Load(TextAsset _xml)
      {
         XmlSerializer serializer = new XmlSerializer (typeof(Dialog));
         StringReader reader = new StringReader (_xml.text);
         Dialog dial = serializer.Deserialize(reader) as Dialog;
         return dial;
      }
      
      //public abstract void startDialog(TextAsset ta);
   }

   [System.Serializable]
   public class Node
   {
      [XmlElement("quest")]
      public Quest quest;
      
      
      [XmlElement("npcname")]
      public string npcName;
      
      [XmlElement("npctext")]
      public string npcText;

      [XmlArray("answers")]
      [XmlArrayItem("answer")]
      public Answer[] answers;
   }

   [System.Serializable]
   public class Quest
   {
      [XmlElement("questname")]
      public string questName;

      [XmlAttribute("needquestvalue")]
      public int needQuestValue;
   }

   [System.Serializable]
   public class Answer
   {
      [XmlAttribute("tonode")]
      public int nextNode;
      [XmlElement("text")]
      public string text;
      [XmlElement("dialend")]
      public string end;
      [XmlElement("tolevel")]
      public string toLevel;

      [XmlAttribute("questvalue")]
      public int questValue;
   }
}

