using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace UP01_223_Nikolaeva
{
    public static class PriceParser
    {
        public static bool TryParse(string input, out decimal price, out string error)
        {
            price = 0;
            error = null;

            if (string.IsNullOrWhiteSpace(input))
            {
                error = "Введите цену";
                return false;
            }

            string normalized = input.Trim().Replace(',', '.').Replace(" ", "");



            if (!decimal.TryParse(normalized, NumberStyles.Number, CultureInfo.InvariantCulture, out price))
            {
                error = "Не удалось распознать цену";
                return false;
            }

            if (price < 0)
            {
                error = $"Цена не может быть отрицательной";
                return false;
            }
            if (price > 1000000)
            {
                error = $"Цена должна быть не больше 1000000";
                return false;
            }

            return true;
        }
    }
}
