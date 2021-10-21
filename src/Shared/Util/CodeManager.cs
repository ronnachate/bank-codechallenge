using System;
using System.Text.RegularExpressions;

namespace CodeChallenge.Util
{
    public static class CodeManager
    {
        private static readonly int ZERO_PADDING_LENGTH = 8;
        private static readonly string TRANSACTION_FLAG = "3";
        public static string GenerateTransactionCode(int transactionId)
        {
            var code = transactionId.ToString().PadLeft(ZERO_PADDING_LENGTH, '0');
            var evenFlag = IsEvenFlag(transactionId);
            var lastDigit = LastSumOfTWO(code);
            code = $"{TRANSACTION_FLAG}{evenFlag}{code}{lastDigit}";
            return code;
        }

        public static bool IsValidTransactionCode(string transactionCode)
        {

            Regex r = new Regex(@"(\d{1})(\d{1})(\d{8})(\d{1})");
            Match m = r.Match(transactionCode);
            if (m.Success)
            {
                var isValid = true;
                var paymentFlag = m.Groups[1].Value;
                var evenFlag = m.Groups[2].Value;
                var transactionStr = m.Groups[3].Value;
                var lastDigit = m.Groups[4].Value;
                var transactionId = Int32.Parse(transactionStr);
                if (paymentFlag != TRANSACTION_FLAG)
                {
                    isValid = false;
                }
                if (LastSumOfTWO(transactionStr) != lastDigit)
                {
                    isValid = false;
                }
                if (IsEvenFlag(transactionId) != evenFlag)
                {
                    isValid = false;
                }
                return isValid;
            }
            return false;
        }

        private static string IsEvenFlag(int number)
        {
            if (number % 2 == 0)
            {
                return "1";
            }
            return "2";
        }

        private static string LastSumOfTWO(string code)
        {
            var codeArray = code.ToCharArray();
            var sumOfLastTWO = Convert.ToInt32(codeArray[ZERO_PADDING_LENGTH - 1].ToString())
                + Convert.ToInt32(codeArray[ZERO_PADDING_LENGTH - 2].ToString())
                + Convert.ToInt32(codeArray[ZERO_PADDING_LENGTH - 3].ToString());
            return (sumOfLastTWO % 10).ToString();
        }
    }
}
