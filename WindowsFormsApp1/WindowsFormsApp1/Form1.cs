using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            checkedListBox1.SetItemChecked(5,true); //Select the best algorithm as default
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (checkedListBox1.CheckedIndices.Count == 0) {    //check if algorithm is been chosen
                MessageBox.Show("Choose algorithm!");   
                return;
            }
            if (textBox1.Text.Length<1) //check if the represented number string is not empty
            {
                MessageBox.Show("Type integer number!"); 
                return;
            }
            string checkstring = "0123456789";  
            try
            {
                //Check if the string only consists of the numerals
                int SymbolsCount = 0;
                foreach (char c in checkstring) for(int i = 0;i<textBox1.Text.Length;i++)
                    if (textBox1.Text[i]==c) SymbolsCount++;
                if(SymbolsCount==textBox1.Text.Length)
                {
                    AccurateFactorial n = new AccurateFactorial(textBox1.Text);
                    n = n.DisposeZero(n);   //Dispose of extra zeros in the beginning
                    textBox1.Text = n.ToString();
                    //Choosing different methods depending on the selected algorithm
                    if (checkedListBox1.CheckedIndices[0] != 5) 
                    {
                        if (long.TryParse(textBox1.Text, out long m))
                        {
                            switch (checkedListBox1.CheckedIndices[0])
                            {
                                case 0:
                                    textBox2.Text = Factorial.GetFactorialWithCycles(m).ToString();
                                    break;
                                case 1:
                                    textBox2.Text = Factorial.GetFactorial(m).ToString();
                                    break;
                                case 2:
                                    textBox2.Text = Factorial.Approximation(m, "Ramanujan").ToString();
                                    break;
                                case 3:
                                    textBox2.Text = Factorial.Approximation(m, "Stirling").ToString();
                                    break;
                                case 4:
                                    textBox2.Text = Factorial.Approximation(m, "").ToString();
                                    break;
                            }
                            if (long.TryParse(textBox2.Text, out long k))
                                if (k<1 || m>20)
                                {//Coud not calculate the factorial
                                textBox2.Text = "Error";
                                MessageBox.Show("The number is too big for the current method");
                                return;
                            }
                        }
                        return;
                    }
                    //check if the number is small enough
                    if (n.value.Length > 4 || (n.value.Length>3 && n.value[0] > 6 && n.value[1]>7)) {
                        textBox2.Text = $"The number {n.ToString()} is too big integer number (>=7500)";
                        return;
                    } 
                    Factorial f = new Factorial();  //I made all methods to be used in the Factorial class
                    n = new AccurateFactorial(f.Accurate(n).value); //calculate using the best method
                    textBox2.Text = n.ToString();   //write result
                }
                else
                {
                    textBox2.Text = "Error: the string must contain only numerals";
                }
            }
            catch (Exception) {
                MessageBox.Show("Unkown Error");
            }
        }

        private void checkedListBox1_ItemChecked(object sender,EventArgs e)
        {
        
        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            //Uncheck all values except the recently checked one 
                if (e.NewValue == CheckState.Checked)
                    for (int i = 0; i < checkedListBox1.Items.Count; i++)
                        if (e.Index != i) checkedListBox1.SetItemChecked(i, false);
        }
    }
}
