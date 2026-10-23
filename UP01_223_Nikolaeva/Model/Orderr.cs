using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UP01_223_Nikolaeva.Model
{
    public partial class Order
    {
        public string GoodsInOrder
        {
            get
            {

                return string.Join(", ", OrderGood.Select(og => $"{og.Good.Name} ({og.Quantity} шт.)"));
            }
            set { }
        }
    }
}
