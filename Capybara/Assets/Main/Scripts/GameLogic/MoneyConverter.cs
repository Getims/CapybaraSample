namespace Main.Scripts.GameLogic
{
    public static class MoneyConverter
    {
        public static string ConvertToShortValue(long money)
        {
            if (money < 1000)
                return money.ToString();

            if (money < 100000)
            {
                float thousands = money / 1000f;
                return $"{thousands:0.##}K";
            }

            if (money < 1000000)
            {
                float thousands = money / 1000f;
                return $"{thousands:0.#}K";
            }

            float millions = money / 1000000f;
            return $"{millions:0.##}M";
        }

        /// <summary>
        /// Adds a space every 3 characters from the end
        /// </summary>
        public static string ConvertToSpaceValue(long money)
        {
            string numberString = money.ToString();
            if (numberString.Length <= 3)
                return numberString;

            string result = "";
            int count = 0;
            for (int i = numberString.Length - 1; i >= 0; i--)
            {
                result = numberString[i] + result;
                count++;
                if (count == 3 && i > 0)
                {
                    result = " " + result;
                    count = 0;
                }
            }

            return result;
        }
    }
}