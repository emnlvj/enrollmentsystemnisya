using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnrollmentSystem
{
    public partial class SubjectEntry : Form
    {
        string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\mis\Desktop\Enrollment System\Jacaba1.accdb";
        public SubjectEntry()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            panel1.BackColor = Color.FromArgb(150, panel1.BackColor);
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            panel2.BackColor = Color.FromArgb(150, panel1.BackColor);
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            bool exist = false;
            OleDbConnection thisConnection = new OleDbConnection(connectionString);
            thisConnection.Open();
            OleDbCommand thisCommand = thisConnection.CreateCommand();
            thisCommand.CommandText = "SELECT * FROM SUBJECTFILE";
            OleDbDataReader thisDataReader = thisCommand.ExecuteReader();


            while (thisDataReader.Read())
            {
                if (thisDataReader["SFSUBJCODE"].ToString().Trim().ToUpper() == SubjectCodeTextBox.Text.Trim().ToUpper())
                {
                    MessageBox.Show("SUBJECT CODE ALREADY EXISTS");
                    exist = true;
                    break;
                }
            }

            if (!exist)
            {
                thisConnection = new OleDbConnection(connectionString);
                OleDbDataAdapter thisAdapter = new OleDbDataAdapter("SELECT * FROM SUBJECTFILE", thisConnection);
                OleDbCommandBuilder thisBuilder = new OleDbCommandBuilder(thisAdapter);
                DataSet thisDataSet = new DataSet();
                thisAdapter.Fill(thisDataSet, "SUBJECTFILE");
                DataRow thisRow = thisDataSet.Tables["SUBJECTFILE"].NewRow();

                thisRow["SFSUBJCODE"] = SubjectCodeTextBox.Text;
                thisRow["SFSUBJDESC"] = DescTextBox.Text;
                thisRow["SFSUBJUNITS"] = UnitsTextBox.Text;
                thisRow["SFSUBJREGOFRNG"] = OfferingComboBox.Text.Substring(0, 1);
                thisRow["SFSUBJCATEGORY"] = CategoryComboBox.Text.Substring(0, 3).ToUpper();
                thisRow["SFSUBJSTATUS"] = "AC";
                thisRow["SFSUBJCOURSECODE"] = CourseCodeComboBox.Text;
                thisRow["SFSUBJCURRCODE"] = YearTextBox.Text;

                thisDataSet.Tables["SUBJECTFILE"].Rows.Add(thisRow);
                thisAdapter.Update(thisDataSet, "SUBJECTFILE");

                if (SubjectCodeTwoTextBox.Text != string.Empty)
                {
                    bool radiobuttonchecked = false;
                    string category = null;
                    if (PreRequisiteRadioButton.Checked || CoRequisiteRadioButton.Checked)
                        radiobuttonchecked = true;

                    if (radiobuttonchecked)
                    {
                        thisConnection = new OleDbConnection(connectionString);
                        thisAdapter = new OleDbDataAdapter("SELECT * FROM SUBJECTPREQFILE", thisConnection);
                        thisBuilder = new OleDbCommandBuilder(thisAdapter);
                        thisDataSet = new DataSet();
                        thisAdapter.Fill(thisDataSet, "SUBJECTPREQFILE");
                        thisRow = thisDataSet.Tables["SUBJECTPREQFILE"].NewRow();

                        thisRow["SUBJCODE"] = SubjectCodeTextBox.Text;
                        thisRow["SUBJPRECODE"] = SubjectCodeTwoTextBox.Text;

                        if (PreRequisiteRadioButton.Checked)
                            category = "PR";
                        else
                            category = "CR";

                        thisRow["SUBJCATEGORY"] = category;

                        thisDataSet.Tables["SUBJECTPREQFILE"].Rows.Add(thisRow);
                        thisAdapter.Update(thisDataSet, "SUBJECTPREQFILE");
                    }
                }

                MessageBox.Show("RECORDED");

                SubjectCodeTextBox.Text = null;
                DescTextBox.Text = null;
                UnitsTextBox.Text = null;
                OfferingComboBox.SelectedIndex = -1;
                CategoryComboBox.SelectedIndex = -1;
                CourseCodeComboBox.SelectedIndex = -1;
                YearTextBox.Text = null;
                SubjectCodeTwoTextBox.Text = null;
                PreRequisiteRadioButton.Checked = false;
                CoRequisiteRadioButton.Checked = false;
            }
        }
    }
}
    
