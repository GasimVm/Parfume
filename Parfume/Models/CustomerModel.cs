﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parfume.Models
{
    public class CustomerModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string FatherName { get; set; }
        public string Fincode { get; set; }
        public string Address { get; set; }
        public string WorkAddress { get; set; }
        public string InstagramAddress { get; set; }
 
        public string Note { get; set; }
        public string BlockNote { get; set; }

        public string BaseNumber { get; set; }
        public string FirstNumber { get; set; }
        public string FirstNumberWho { get; set; }

        public string SecondNumber { get; set; }
        public string SecondNumberWho { get; set; }
        public string ThirdNumber { get; set; }
        public string ThirdNumberWho { get; set; }
        public string Birthday { get; set; }
        public string WhoIsOkey { get; set; }
        public string BonusAmount { get; set; }
        public string CardId { get; set; }
        public string ReferencesId { get; set; }
    }
}
