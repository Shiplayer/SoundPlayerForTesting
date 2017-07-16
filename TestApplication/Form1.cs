using System;
using System.IO;
using System.Media;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestApplication
{
    public partial class Form1 : Form
    {
        private String pathDirWithMedialFiles;
        private List<string> files;
        private List<int> associateArray;
        private SoundPlayer player;
        private string currentPath;

        public Form1()
        {
            currentPath = Directory.GetCurrentDirectory();
            InitializeComponent();
            player = new SoundPlayer();
            pathDirWithMedialFiles = currentPath;
            label1.Text = pathDirWithMedialFiles;
            associateArray = new List<int>();
            files = new List<string>();
            KeyValuePair<string, string>[] pair = new KeyValuePair<string, string>[10];
            
        }

        //custom methods
        private void DataClear()
        {
            listView1.Clear();
            associateArray.Clear();
            files.Clear();
        }

        private void setAllFilesInListView()
        {
            ListView.ListViewItemCollection list = listView1.Items;
            string[] files = Directory.GetFiles(pathDirWithMedialFiles, "*.wav");
            foreach (string file in files)
            {
                associateArray.Add(associateArray.Count + 1);
                list.Add(file);
            }
            Sorting();
        }

        private void Sorting()
        {
            ListView.ListViewItemCollection list = listView1.Items;
            Random rnd = new Random();
            int[] rndIndex = new int[list.Count];
            bool[] checkArr = new bool[list.Count];
            string[] items = new string[list.Count];
            for (int index = 0; index < rndIndex.Length; index++)
            {

                int n = rnd.Next(list.Count);
                rndIndex[index] = n;
                if (!checkArr[n])
                {
                    checkArr[n] = true;
                    items[n] = list[index].Text;
                    if(list[index].BackColor == Color.Red)
                    {
                        list[index].BackColor = Color.White;
                    }
                }
                else
                    index--;
            }
            label2.Text = string.Join(", ", rndIndex);
            string line = "";

            for (int i = 0; i < list.Count; i++)
            {
                list[i].Text = items[i];
            }
        }

        //Forms methods
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            StreamWriter sw = new StreamWriter(new FileStream(currentPath + "\\save.data", FileMode.Create));
            sw.WriteLine(pathDirWithMedialFiles);
            sw.Close();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            if (File.Exists(currentPath + "\\save.data"))
            {
                StreamReader sr = new StreamReader(currentPath + "\\save.data");
                pathDirWithMedialFiles = sr.ReadLine();
                setAllFilesInListView();
                sr.Close();
            }
        }

        //listView1 methods
        private void listView1_CollectionChanged(object sender, EventArgs e)
        {

        }

        //StripMenu methods
        private void setDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataClear();
            FolderBrowserDialog bd = new FolderBrowserDialog();
            bd.ShowDialog();
            if (bd.SelectedPath.Length > 0)
            {
                pathDirWithMedialFiles = bd.SelectedPath;
                label1.Text = pathDirWithMedialFiles;
                setAllFilesInListView();
            }
        }

        private void addDirWithFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog bd = new FolderBrowserDialog();
            bd.ShowDialog();
            if (bd.SelectedPath.Length > 0)
            {
                pathDirWithMedialFiles = bd.SelectedPath;
                label1.Text = pathDirWithMedialFiles;
                setAllFilesInListView();
            }
        }

        //buttons methods
        private void button1_Click(object sender, EventArgs e)
        {
            ListView.SelectedListViewItemCollection collection = listView1.SelectedItems;
            if (collection.Count > 0)
            {
                collection[0].BackColor = Color.Red;
                player = new SoundPlayer(collection[0].Text);
                player.PlaySync();
            }
            else
            {
                MessageBox.Show(this, "no selected element", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Sorting();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (player.IsLoadCompleted)
            {
                player.Stop();
            }
        }

        private void aboutAProgrammToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 aboutbox = new AboutBox1();
            aboutbox.Show();
        }
    }
}
