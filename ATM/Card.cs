using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM
    {
    public class Card
        {
        public Guid CardNumber { get; set; }
        public bool IsBlocked { get; set; }

        public Card(Guid cardNumber)
            {
            CardNumber = cardNumber;
            IsBlocked = false;
            }
        }
    }
