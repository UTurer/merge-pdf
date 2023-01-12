
namespace MergePDF
{
    public partial class Form1 : System.Windows.Forms.Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            openFileDialog1.Multiselect = true;
            System.Windows.Forms.DialogResult dialogResult1 = openFileDialog1.ShowDialog();
            if (dialogResult1 == System.Windows.Forms.DialogResult.OK)
            {
                for (int i=0;i<openFileDialog1.FileNames.Length;i++)
                {
                    System.Windows.Forms.ListViewItem listViewItem1 = listView1.Items.Add(openFileDialog1.FileNames[i]);
                    listViewItem1.Tag = listViewItem1.Text;
                    if(showFileNameOnlyToolStripMenuItem.Checked)
                    {
                        listViewItem1.Text = System.IO.Path.GetFileName(listViewItem1.Text);
                    }
                }
            }
        }

        private void Form1_Load(object sender, System.EventArgs e)
        {
            listView1.MultiSelect = true;
            radioButton1.Checked = true;
            radioButton3.Checked = true;
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            button6.Enabled = false;
            button7.Enabled = false;
            showFileNameOnlyToolStripMenuItem.Checked = true;
        }

        private void button4_Click(object sender, System.EventArgs e)
        {
            if (listView1.Items.Count < 0)
                return;

            if (listView1.SelectedItems.Count < 0)
                return;

            for (int i = 0; i < listView1.SelectedItems.Count;i++)
            {
                listView1.Items.Remove(listView1.SelectedItems[i]);
            }
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            if (listView1.SelectedItems.Count < 1)
                return;

            for (int i=0;i<listView1.SelectedItems.Count;i++)
            {
                System.Windows.Forms.ListViewItem listViewItem1 = listView1.SelectedItems[i];
                int previousIndex = listViewItem1.Index;
                if (previousIndex-1 >= 0)
                {
                    if(listView1.SelectedIndices.Contains(previousIndex-1))
                    {
                        continue;
                    }
                }
                int newIndex = System.Math.Max(0, previousIndex - 1);
                listView1.Items.Remove(listViewItem1);
                listView1.Items.Insert(newIndex, listViewItem1);
            }
        }

        private void button3_Click(object sender, System.EventArgs e)
        {
            if (listView1.SelectedItems.Count < 1)
                return;

            for (int i = 0; i < listView1.SelectedItems.Count; i++)
            {
                System.Windows.Forms.ListViewItem listViewItem1 = listView1.SelectedItems[i];
                int previousIndex = listViewItem1.Index;
                if (previousIndex + 1 < listView1.Items.Count)
                {
                    if (listView1.SelectedIndices.Contains(previousIndex + 1))
                    {
                        continue;
                    }
                }
                int newIndex = System.Math.Min(listView1.Items.Count-1, previousIndex + 1);
                listView1.Items.Remove(listViewItem1);
                listView1.Items.Insert(newIndex, listViewItem1);
            }
        }

        private void button5_Click(object sender, System.EventArgs e)
        {
            if (listView1.Items.Count < 2)
                return;
        
            System.String dirEXE = System.AppContext.BaseDirectory;
            System.String filenameEXE = "pdftk.exe";
            if (radioButton2.Checked)
            {
                dirEXE = System.IO.Path.GetDirectoryName(textBox1.Text);
                filenameEXE = System.IO.Path.GetFileName(textBox1.Text);
            }
            System.String pathEXE = System.IO.Path.Combine(dirEXE, filenameEXE);


            System.String dirOutput = System.AppContext.BaseDirectory;
            System.String filenameOutput = "merged.pdf";
            if (radioButton4.Checked)
            {
                dirOutput = System.IO.Path.GetDirectoryName(textBox2.Text);
                filenameOutput = System.IO.Path.GetFileName(textBox2.Text);
            }
            
            System.String[] pathPDFs = new System.String[2];

            for (int i=0;i<listView1.Items.Count-1;i++)
            {
                if (i==0)
                {
                    pathPDFs[0] = (System.String)listView1.Items[i].Tag;
                }
                pathPDFs[1] = (System.String)listView1.Items[i+1].Tag;

                System.String output = "";
                
                if (i == listView1.Items.Count - 2)
                {
                    output = filenameOutput;
                }
                else
                {
                    output = "dummy" + i.ToString() + ".pdf";
                }
                System.String pathOutput = System.IO.Path.Combine(dirOutput, output);
                System.String string2 = "A=\"" + pathPDFs[0] + "\" " + "B=\"" + pathPDFs[1] + "\" cat A B output \"" + pathOutput + "\"";
                System.Diagnostics.Process process1 = new System.Diagnostics.Process();
                process1.StartInfo = new System.Diagnostics.ProcessStartInfo(pathEXE, string2);
                process1.StartInfo.RedirectStandardOutput = true;
                process1.StartInfo.RedirectStandardError = true;
                process1.StartInfo.UseShellExecute = false;
                process1.Start();
                System.Text.StringBuilder stringBuilder1 = new System.Text.StringBuilder();
                while(!process1.HasExited)
                {
                    stringBuilder1.Append(process1.StandardOutput.ReadToEnd());
                    stringBuilder1.Append(process1.StandardError.ReadToEnd());
                }
                richTextBox1.AppendText(stringBuilder1.ToString() + System.Environment.NewLine);
                process1.WaitForExit();
                pathPDFs[0] = output;
            }
        }

        private void radioButton2_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radioButton2.Checked)
            {
                textBox1.Enabled = true;
                button6.Enabled = true;
            }
            else
            {
                textBox1.Enabled = false;
                button6.Enabled = false;
            }
        }

        private void button6_Click(object sender, System.EventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            openFileDialog1.Filter = "*.exe|*.exe|All Files|*.*";
            System.Windows.Forms.DialogResult dialogResult1 = openFileDialog1.ShowDialog();
            if(dialogResult1 == System.Windows.Forms.DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
            }
        }

        private void button7_Click(object sender, System.EventArgs e)
        {
            System.Windows.Forms.SaveFileDialog saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            saveFileDialog1.Filter = "*.pdf|*.pdf|All Files|*.*";
            System.Windows.Forms.DialogResult dialogResult1 = saveFileDialog1.ShowDialog();
            if (dialogResult1 == System.Windows.Forms.DialogResult.OK)
            {
                textBox2.Text = saveFileDialog1.FileName;
            }
        }

        private void radioButton4_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radioButton4.Checked)
            {
                textBox2.Enabled = true;
                button7.Enabled = true;
            }
            else
            {
                textBox2.Enabled = false;
                button7.Enabled = false;
            }
        }

        private void addFileToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            button1_Click(null, null);
        }

        private void removeSelectedFilesToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            button4_Click(null, null);
        }

        private void exitToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void moveSelectedUpToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            button2_Click(null,null);
        }

        private void moveSelectedDownToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            button3_Click(null, null);
        }

        private void aboutToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            System.Windows.Forms.Form form1 = new System.Windows.Forms.Form();
            System.Windows.Forms.RichTextBox richTextBox1 = new System.Windows.Forms.RichTextBox();
            form1.Controls.Add(richTextBox1);
            richTextBox1.Parent = form1;
            richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            System.String string1 = "";
            string1 = string1 + "-----------------------" + System.Environment.NewLine;
            string1 = string1 + "Version: 1.00" + System.Environment.NewLine;
            string1 = string1 + "Created By: Utku TURER" + System.Environment.NewLine;
            string1 = string1 + "-----------------------" + System.Environment.NewLine;
            string1 = string1 + "This program needs pdftk.exe and libiconv2.dll to work. ";
            string1 = string1 + "Both of these files must be in the same folder. Because of ";
            string1 = string1 + "limitations of pdftk.exe, only two files can be combined at ";
            string1 = string1 + "a time. Hence if more then two pdf files are merged, ";
            string1 = string1 + "intermediate pdf files are generated. These files are named ";
            string1 = string1 + "dummy and they can be safely erased.";
            richTextBox1.Text = string1;
            form1.Show();
        }

        private void showFileNameOnlyToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            for (int i=0;i<listView1.Items.Count;i++)
            {
                listView1.Items[i].Text = System.IO.Path.GetFileName((System.String)listView1.Items[i].Tag);
            }
            showFullFilePathToolStripMenuItem.Checked = false;
        }

        private void showFullFilePathToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                listView1.Items[i].Text = (System.String)listView1.Items[i].Tag;
            }
            showFileNameOnlyToolStripMenuItem.Checked = false;
        }
    }
}
