using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UP01_223_Nikolaeva.Model
{
    public partial class DeliveryAddress
    {
        public string FullAddress
        {
            get
            {
                return $"{AvailabilityAddress.City}, ул. {AvailabilityAddress.Street}, д. {AvailabilityAddress.House}, кв. {Apartment}";
            }
            set { }
        }
    }
}
