using System;
using System.Linq;
using System.Net;
using System.ServiceModel.Web;


namespace WCFService
{
    // NOTE: In order to launch WCF  Client for testing , please select ConverterService.svc.
    public class ConverterService : IConverter
    {
        string phrase = "";
        double int_num;
        double dec_num = 0;
        public string ConverstionRequest(string amount)
        {
            if (amount == "0")
                return " zero dollars ";
            if(amount.Length >= 3 && amount.Contains(','))
            {
                string[] commaArr = amount.ToString().Split(',');
                int_num = double.Parse(commaArr[0]);
                dec_num = double.Parse(commaArr[1]);
            }
            else
            {
                int_num = double.Parse(amount);
            }

            if (int_num == 0)
            {
               phrase +=  " zero dollars ";
            }

            if (int_num < 0 || int_num > 999999999)
            {
                throw new WebFaultException<string>
                    ("Numbers less than zero or greater than 999,999,999 are not supported",
                    HttpStatusCode.BadRequest);
            }

            if (int_num > 0)
            {
                phrase += NumberTophrase(int_num) + (int_num > 1 ? " dollars " : " dollar ");
            }

            if (dec_num > 0)
            {
                if (phrase != "")
                    phrase += " and ";
                phrase += NumberTophrase(dec_num) + (dec_num > 1 ? " cents" : " cent ");
            }
         
            return phrase;
        }


        public String NumberTophrase(double number)
        {
            string[] units = new string[] { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
            string[] tens = new string[] { "twenty", "thirty", "fourty", "fifty", "sixty", "seventy", "eighty", "ninty" };
            string[] thousand_million = new string[] { "thousand", "million" };

            string phrase = "";
            bool ten = false;
            int power = 9;

            try
            {
                while (power > 3)
                {
                    double pow = Math.Pow(10, power); 
                    if (number >= pow)
                    {
                        if (number % pow >= 0) 
                        {
                            phrase += NumberTophrase(Math.Floor(number / pow)) + " " + thousand_million[(power / 3) - 1] + " ";
                        }

                        number %= pow;
                    }
                    power -= 3;
                }

                #region thousand unit
                if (number >= 1000)
                {
                    if (number % 1000 > 0) phrase += NumberTophrase(Math.Floor(number / 1000)) + " thousand ";
                    number %= 1000;
                }
                #endregion

                #region hundred, tens, and units
                if (number >= 0 && number <= 999)
                {
                    //hundred 
                    if ((int)number / 100 > 0)
                    {
                        phrase += NumberTophrase(Math.Floor(number / 100)) + " hundred";
                        number %= 100;
                    }

                    //tens
                    if ((int)number / 10 > 1)
                    {
                        if (phrase != "")
                            phrase += " ";
                        phrase += tens[(int)number / 10 - 2];
                        ten = true;
                        number %= 10;
                    }

                    //units 
                    if (number < 20 && number > 0)
                    {
                        if (phrase != "" && ten == false)
                            phrase += " ";
                        phrase += (ten ? "-" + units[(int)number - 1] : units[(int)number - 1]);
                        number -= Math.Floor(number);
                    }
                }

                #endregion
            }
            catch (Exception ex)
            {
                throw new SystemException(ex.Message);
            }
            return phrase;

        }

    }
}
