using System;
using System.Diagnostics;

namespace KaratsubaMultiplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Input a first positive mutiplier:");
            Console.WriteLine("For example: 3141592653589793238462643383279502884197169399375105820974944592");
            var num1 = Console.ReadLine();

            Console.WriteLine("Input a second positive mutiplier:");
            Console.WriteLine("For example: 2718281828459045235360287471352662497757247093699959574966967627");
            var num2 = Console.ReadLine();
            //Result of sample numbers should be
            //8539734222673567065463550869546574495034888535765114961879601127067743044893204848617875072216249073013374895871952806582723184
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Console.WriteLine();
            Console.WriteLine("KaratsubaMultiply result:");
            Console.WriteLine(KaratsubaMultiply(num1, num2));
            sw.Stop();

            Console.WriteLine();
            Console.WriteLine("Elapsed={0}", sw.Elapsed);
        }

        static string KaratsubaMultiply(string m1, string m2)
        {
            if (m1.Length > m2.Length)
            {
                var diff = m1.Length - m2.Length;
                m2 = new string('0', diff) + m2;
            }
            else if (m1.Length < m2.Length)
            {
                var diff = m2.Length - m1.Length;
                m1 = new string('0', diff) + m1;
            }

            if(m1.Length == 1)
            {
                return Multiply(m1[0], m2[0]);
            }

            if (m1.Length % 2 == 1) { //if length not divided on 2
                m1 = '0' + m1;
                m2 = '0' + m2;
            }

            int n_2 = m1.Length / 2;
            string a, b, c, d;
            a = m1.Substring(0, n_2);
            b = m1.Substring(n_2, n_2);
            c = m2.Substring(0, n_2);
            d = m2.Substring(n_2, n_2);

            var ac = KaratsubaMultiply(a, c);
            var bd = KaratsubaMultiply(b, d);
            var p = Sum(a, b);
            var q = Sum(c, d);
            var pq = KaratsubaMultiply(p,q);

            var abcd = Subtraction(pq, ac);
            abcd = Subtraction(abcd, bd);

            ac += new string('0', m1.Length);
            abcd += new string('0', n_2);
            var res = Sum(Sum(ac, abcd), bd);

            return res;
        }
        static string CutLeadingZeros(string num)
        {
            int i = 0;
            for (; i < num.Length; i++)
            {
                if(num[i] != '0')
                {
                    break;
                }
            }
            if(i == num.Length)
            {
                return "0";
            }
            return num.Substring(i);
        }
        static string Sum(string a, string b)//is a.Length == b.Length
        {
            if (a.Length > b.Length)
            {
                var diff = a.Length - b.Length;
                b = new string('0', diff) + b;
            }
            else if (a.Length < b.Length)
            {
                var diff = b.Length - a.Length;
                a = new string('0', diff) + a;
            }
            string res = "";
            char inMind = '0';
            for (int i = a.Length - 1; i >= 0; i--) {
                var intermediateSum = Sum(a[i], b[i]);
                if (intermediateSum.Length == 2) //length of result is 2!
                {
                    if (inMind == '0')
                    {
                        res = intermediateSum[1] + res;
                        inMind = intermediateSum[0];
                    }
                    else
                    {
                        var intermediateSum2 = Sum(intermediateSum[1], inMind);
                        if (intermediateSum2.Length == 2) //this never called
                        {
                            throw new NotImplementedException();
                            res = intermediateSum2[1] + res;
                            inMind = intermediateSum2[0];//here should be intermediateSum2[0] + current inMind
                        }
                        else
                        {
                            res = intermediateSum2[0] + res;
                        }
                    }
                }
                else //length of result is 1!
                {
                    if (inMind == '0')
                    {
                        res = intermediateSum[0] + res;
                    }
                    else
                    {
                        var intermediateSum2 = Sum(intermediateSum[0], inMind);
                        if (intermediateSum2.Length == 2)
                        {
                            res = intermediateSum2[1] + res;
                            inMind = intermediateSum2[0];
                        }
                        else
                        {
                            res = intermediateSum2[0] + res;
                            inMind = '0';
                        }
                    }
                }
            }

            res = inMind + res;
            
            return CutLeadingZeros(res);

        }

        static string Sum(char a, char b)
        {
            int i = (int)char.GetNumericValue(a);
            int j = (int)char.GetNumericValue(b);
            var res = i + j;
            return res.ToString();
        }

        static string Subtraction(string a, string b)
        {
            if (a.Length > b.Length)
            {
                var diff = a.Length - b.Length;
                b = new string('0', diff) + b;
            }
            else if (a.Length < b.Length)
            {
                var diff = b.Length - a.Length;
                a = new string('0', diff) + a;
            }

            var res = "";
            var transfer = "0";
            bool isAhigherB;
            for (int i = a.Length - 1; i >= 0; i--)
            {
                res = Subtraction(a[i], b[i], out isAhigherB) + res;
                transfer = (isAhigherB ? "0" : "1") + transfer;
            }
            if (transfer.Contains('1')) {
                res = Subtraction(res, transfer);
            }
            return CutLeadingZeros(res);
        }

        static string Subtraction(char a, char b, out bool isAhigherB) {
            int i = (int)char.GetNumericValue(a);
            int j = (int)char.GetNumericValue(b);
            var res = 0;
            if (i >= j)
            {
                isAhigherB = true;
                res = i - j;
            } else
            {
                isAhigherB = false;
                res = i + 10 - j;
            }
            return res.ToString();
        }

        static string Multiply(char a, char b)
        {
            int i = (int)char.GetNumericValue(a);
            int j = (int)char.GetNumericValue(b);
            var res = i * j;
            return res.ToString();
        }
    }
}
