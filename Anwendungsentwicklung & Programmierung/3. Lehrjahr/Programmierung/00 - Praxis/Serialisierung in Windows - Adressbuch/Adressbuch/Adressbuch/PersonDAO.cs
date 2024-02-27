﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Adressbuch
{
    class PersonDAO : IPersonDAO
    {
        private List<Person> pList = new List<Person>();
        public List<Person> PList { get => pList; }

        private SaveFileDialog sfDialog = new SaveFileDialog();
        private OpenFileDialog opDialog = new OpenFileDialog();
        private string path = string.Empty;
        private string initialPath = "K:Berufsschule\\Anwendungsentwicklung & Programmierung\\3. Lehrjahr\\Programmierung\\02 - Praxis\\Serialisierung in Windows - Adressbuch\\Adressbuch\\SerializedFiles";
        private string initialFilter = "Adressdateien (*.adr)|*.adr|XMLAdressen (*.xml)|*.xml|JSONAdressen (*.json)|*.json";

        private void Load()
        {
            try
            {
                IFormatter f = new BinaryFormatter();
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    pList = (List<Person>)f.Deserialize(fs);
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Fehler!",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadXML()
        {
            try
            {
            XmlSerializer s = new XmlSerializer(typeof(IPersonDAO));
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                pList = (List<Person>)s.Deserialize(fs);
                fs.Close();
            }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Fehler!",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadJSON()
        {
            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    string result = sr.ReadToEnd();
                }

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                pList = serializer.Deserialize<IPersonDAO>(result);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Fehler!",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Save()
        {
            try
            {
                IFormatter f = new BinaryFormatter();
                using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    f.Serialize(fs, pList);
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Fehler!",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveXML()
        {
            try
            {
                XmlSerializer s = new XmlSerializer(typeof(IPersonDAO));
                using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    s.Serialize(fs, pList);
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Fehler!",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void LoadData()
        {
            opDialog.InitialDirectory = initialPath;
            opDialog.Filter = initialFilter;
            if (path != string.Empty)
            {
                opDialog.FileName = path;
            }
            if (opDialog.ShowDialog() == DialogResult.OK)
            {
                path = opDialog.FileName;
                Load();
            }
        }

        public void SaveData()
        {
            sfDialog.InitialDirectory = initialPath;
            sfDialog.Filter = initialFilter;
            if (path != string.Empty)
            {
                sfDialog.FileName = path;
            }
            if (sfDialog.ShowDialog() == DialogResult.OK)
            {
                path = sfDialog.FileName;
                Save();
            }
        }

        public void NewData()
        {
            if (pList.Count > 0)
            {
                if (MessageBox.Show("Wollen Sie die bereits eingegebenen Daten speichern?", "Speichern", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    SaveData();
                }
            }
            pList = new List<Person>();
            path = string.Empty;
        }
    }
}