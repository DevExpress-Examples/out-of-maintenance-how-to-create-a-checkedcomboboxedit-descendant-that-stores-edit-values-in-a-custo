using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CustomCheckedComboBox
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        BindingSource myBindingSource = new BindingSource();
        UsersList myUsers = new UsersList();
        CustomCheckedComboBoxEdit myCheckedComboBoxEdit = new CustomCheckedComboBoxEdit();

        private void Form1_Load(object sender, EventArgs e)
        {
            string[] Languge = new string[] { "English", "Franche", "Spanol", "Gapanise", "Ukrain" };

            myUsers.Add(new User("Antuan"));
            myUsers.Add(new User("Bill"));
            myUsers.Add(new User("Charli"));
            myBindingSource.DataSource = myUsers;


            myCheckedComboBoxEdit.Bounds = new Rectangle(10, 10, 300, 20);
            Controls.Add(myCheckedComboBoxEdit);
            foreach (string item in Languge)
                myCheckedComboBoxEdit.Properties.Items.Add(item, CheckState.Unchecked, true);
            myCheckedComboBoxEdit.DataBindings.Add("EditValue", myBindingSource, "Lang");
            myCheckedComboBoxEdit.Properties.ConvertCheckStateToEditValue += new ConvertCheckStateToEditValueEventHandler(Properties_ConvertCheckStateToEditValue);
            myCheckedComboBoxEdit.Properties.ConvertEditValueToCheckState += new ConvertEditValueToCheckStateEventHandler(Properties_ConvertEditValueToCheckState);

            textEdit1.DataBindings.Add("EditValue", myBindingSource, "Name");
            textEdit2.DataBindings.Add("EditValue", myBindingSource, "Lang");

        }

        void Properties_ConvertEditValueToCheckState(object sender, ConvertEditValueToCheckStateEventArgs e)
        {
            for (int i = 0; i < e.EditValue.Length; i++)
                if (e.EditValue[i] == '1') e.CheckedState[i] = true;
        }

        void Properties_ConvertCheckStateToEditValue(object sender, ConvertCheckStateToEditValueEventArgs e)
        {
            string newValue = "";
            for (int i = 0; i < e.CheckedState.Length; i++)
                if (e.CheckedState[i]) newValue = newValue + "1";
                else newValue = newValue + "0";
            e.EditValue = newValue;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            myBindingSource.MovePrevious();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            myBindingSource.MoveNext();
        }

    }
}