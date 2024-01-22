using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ATM
    {
    public class Card
        {
        [JsonProperty]
        public Guid CardNumber { get; private set; }
        [JsonProperty]
        public bool IsBlocked { get; set; }
        [JsonConstructor]
        public Card(Guid cardNumber)
            {
            CardNumber = cardNumber;
            IsBlocked = false;
            }
        }
    }
