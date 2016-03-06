using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Xml.Schema;

namespace XML2DataGridView
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void addXML_Click(object sender, EventArgs e)
        {
            int size = -1;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                string xmlFile = openFileDialog1.FileName;
                try
                {

                    XmlReaderSettings settings = new XmlReaderSettings();
                    settings.ValidationType = ValidationType.Schema;
                    settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessInlineSchema;
                    settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessSchemaLocation;
                    settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
                    settings.ValidationEventHandler += new ValidationEventHandler(ValidationCallBack);

                    // Create the XmlReader object.
                    XmlReader reader = XmlReader.Create(xmlFile, settings);

                    // Parse the file. 
                    while (reader.Read()) ;
     
                    
                    DataSet dataSet = new DataSet();
                    dataSet.ReadXml(xmlFile, XmlReadMode.InferSchema);

                    // Then display informations to test
                    foreach (DataTable table in dataSet.Tables)
                    {
                        TabPage tabPage1 = new TabPage();
                        tabPage1.Name = table.ToString();
                        tabPage1.Name = dataSet.DataSetName;
                        tabPage1.Text = tabPage1.Name;
                        tabPage1.BackColor = Color.Green;
                        tabPage1.ForeColor = Color.White;
                        tabPage1.Font = new Font("Verdana", 12);
                        tabPage1.Width = 100;
                        tabPage1.Height = 100;
                        tabControl1.TabPages.Add(tabPage1);

                        DataGridView dataGridView1 = new DataGridView();
                        dataGridView1.DataSource = dataSet;
//                        dataGridView1.DataMember = "TableName";
                        dataGridView1.DataMember = "RowData";
                        dataGridView1.Visible = true;

                        System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new System.Windows.Forms.DataGridViewCellStyle();
                        dataGridView1.AllowUserToAddRows = false;
                        dataGridView1.AllowUserToDeleteRows = false;
                        //dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
                        //dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
                        //dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                        //dataGridViewCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
                        //dataGridViewCellStyle.BackColor = System.Drawing.SystemColors.Control;
                        //dataGridViewCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                        //dataGridViewCellStyle.ForeColor = System.Drawing.SystemColors.WindowText;
                        //dataGridViewCellStyle.SelectionBackColor = System.Drawing.SystemColors.Highlight;
                        //dataGridViewCellStyle.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
                        //dataGridViewCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
                        //dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
                        //dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        

                        tabPage1.SuspendLayout();
                        tabPage1.Controls.Add(dataGridView1);
                        tabPage1.ResumeLayout();
                        tabPage1.Refresh();


                        Console.WriteLine(table);
                        for (int i = 0; i < table.Columns.Count; ++i)
//                            Console.Write("\t" + table.Columns[i].ColumnName.Substring(0, Math.Min(16, table.Columns[i].ColumnName.Length)));
                          Console.Write("\t" + table.Columns[i].ColumnName);
                        Console.WriteLine();
                        foreach (var row in table.AsEnumerable())
                        {
                            for (int i = 0; i < table.Columns.Count; ++i)
                            {
                                Console.Write("\t" + row[i]);
                            }
                            Console.WriteLine();
                        }
                    }

                }
                catch (IOException)
                {
                }
            }
        }

    private static void ValidationCallBack(object sender, ValidationEventArgs args)
    {
        if (args.Severity == XmlSeverityType.Warning)
            Console.WriteLine("\tWarning: Matching schema not found.  No validation occurred." + args.Message);
        else
            Console.WriteLine("\tValidation error: " + args.Message);

    }

        
    }
}
