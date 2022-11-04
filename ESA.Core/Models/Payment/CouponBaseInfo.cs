using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESA.Core.Models.Payment
{
    public class CouponBaseInfo
    {
        [Required]
        public string Coupon { get; set; } = string.Empty;
        [Required]
        public int Discount { get; set; }

        public bool IsActive { get; set; } = true;

    }

    public class PackBaseInfo
    {
        [Required]
        public int Hours { get; set; }
        [Required]
        public int Minutes { get; set; }
        public int Price { get; set; }
        public decimal Savings { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime Updated_At { get; set; }
    }

    public class PackBaseInfoEdit
    {
        public int Id { get; set; }
        public int Hours { get; set; }
        public int Minutes { get; set; }
        public int Price { get; set; }
        public decimal Savings { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime Updated_At { get; set; }

    }
}
