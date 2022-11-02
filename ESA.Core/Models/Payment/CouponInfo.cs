using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESA.Core.Models.Payment
{
    public class CouponInfo
    {
        public int Id { get; set; }
        public string Coupon { get; set; } = string.Empty;
        public int Discount { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
