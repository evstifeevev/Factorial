using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    class Factorial
    {
        public static int[] factorial = null;
        public static long GetFactorial(long n) //Recursive implementation
        {
            if (n < 0) return 0;
            if (n < 2) return 1;
            return n * GetFactorial(n - 1);
        }
        public static long GetFactorialWithCycles(long n) //Cycle implementation
        {
            if (n < 0) return 0;
            if (n < 2) return 1;
            long temp = n;
            for (long i = n - 1; i > 1; i--)
                temp *= i;
            return temp;
        }
        public static long Approximation(double n, string str) //Using approximate formulas
        {
            if (n < 0) return 0;
            if (n < 2) return 1;
            decimal temp = 1;
            temp = Decimal.Multiply(temp, (decimal)Math.Sqrt(2 * Math.PI * n));
            temp = Decimal.Multiply(temp, (decimal)Math.Pow(n / Math.E, n));
            switch (str) //different formulas depending on the approximation
            {
                case "Stirling": break;
                case "Ramanujan": temp = Decimal.Multiply(temp, (decimal)Math.Pow(1 + 0.5 / n * +0.125 / n / n, 1.0 / 6.0)); break;
                default: temp = Decimal.Multiply(temp, (decimal)Math.Exp(1.0 / 12.0 * (1.0 / n - 1.0 / 30.0 / n / n / n))); break;
            }
            return (long)Math.Round(temp);
        }
        public AccurateFactorial Accurate(AccurateFactorial n)  //Most accurate method
            //This method uses arrays instead of numbers 
        {
            n = n.DisposeZero(n);
            if (!n.BigEnough(n)) {  //Check if the number is higher than 1
                if (n.value[0] > 0) return n;
                else
                    return new AccurateFactorial(1);    //only returns if the number is zero
            }
            // If I used "return n.multiply(Accurate(n.previous(n)), n)" 
            //the program worked much slower
            AccurateFactorial m = new AccurateFactorial(n.value);
            m.value = n.value;
            m = n.previous(m);
            AccurateFactorial m2 = new AccurateFactorial(Accurate(m).value);
            
            return n.multiply(m2, n);
        }
    }
    public class AccurateFactorial  //Class representing the big integer number as an array
    {
        public int[] value;
        //Constructors
        public AccurateFactorial()  
        {
            value = null;
        }
        public AccurateFactorial(int n2)
        {
            int i = 1, m = n2;
            while (m / 10 > 0)
            {
                i++;
                m /= 10;
            }
            int[] temp = new int[i];
            m = n2;
            for (int k = i - 1; k > -1; k--)
            {
                temp[k] = m % 10;
                m /= 10;
            }
            value = temp;
        }
        public AccurateFactorial(string n3)
        {
            value = new int[n3.Length];
            for (int i = 0; i < n3.Length; i++)
            {
                if (int.TryParse(n3[i].ToString(), out int temp))
                    value[i] = temp;
                else
                    return;
            }
        }
        public AccurateFactorial(int[] array)
        {
            value = new int[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                value[i] = array[i];
            }
        }
        //Changing length of the array
        public void SetSize(long n)
        {
            value = new int[n];
        }
        //Dispose from leading zeros
        public AccurateFactorial DisposeZero(AccurateFactorial n)
        {
            if (n.value.Length > 1)
            {
                int len = 0, i = 0;
                while (i < n.value.Length - 1 && n.value[i] == 0)
                {
                    len++;
                    i++;
                }
                AccurateFactorial temp = new AccurateFactorial();

                temp.SetSize(n.value.Length - len);
                for (i = 0; i < n.value.Length - len; i++)
                {
                    temp.value[i] = n.value[len + i];
                }
                return temp;
            }
            return n;
        }
        public int Size()
        {
            return value.Length;
        }
        //Check if the number is bigger than 1
        public bool BigEnough(AccurateFactorial n)
        {
            if (n.value != null) if (n.value.Length > 1 || n.value[0] > 1) return true;
            return false;
        }
        //Get previous number
        public AccurateFactorial previous(AccurateFactorial n4)
        {

            if (!BigEnough(n4)) return n4;
            AccurateFactorial prev = new AccurateFactorial(n4.value);
            int i = prev.Size() - 1;
            if (i > -1)
            {
                while (i > 0 && prev.value[i] < 1)
                {
                    prev.value[i]--;
                    prev.value[i] = (prev.value[i] + 10) % 10;
                    i--;
                }
                prev.value[i]--;
                return prev;
            }
            prev.value[i]--;
            return prev;

        }
        //Get multiplication of two big numbers
        public AccurateFactorial multiply(params AccurateFactorial[] af)
        {
            AccurateFactorial mult = new AccurateFactorial();
            for (int k = 0; k < 2; k++) DisposeZero(af[0]);//prepare the numbers to work
            //Set the size of new array
            int length = af[0].value.Length + af[1].value.Length,
                len1 = af[0].value.Length - 1, len2 = af[1].value.Length - 1;
            mult.SetSize(length);
            //implement the multiplication algorithm
            length--; int reminder = 0;
            int MaxLength = length;
            for (int i = len1; i >= 0; i--)
            {

                for (int k = len2; k >= 0; k--)
                {
                    int temp = reminder;
                    temp = af[0].value[i] * af[1].value[k];
                    reminder += temp / 10;
                    mult.value[length] += temp % 10;
                    mult.value[--length] += reminder;
                    reminder = 0;

                }
                MaxLength--;
                length = MaxLength;
            }
            reminder = 0;
            length = mult.value.Length - 1;
            //Final summarization
            for (int i = length; i > 0; i--)
            {
                mult.value[i - 1] += mult.value[i] / 10;
                mult.value[i] = mult.value[i] % 10;
            }
            mult = DisposeZero(mult);
            //Result
            return mult;

        }


        public override string ToString() //Overriding method to get the number as string
        {
            string s = "";
            foreach (int i in value) s += i.ToString();
            return s;
        }
    }
}
