using Ardalis.Specification;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESA.Core.Specs
{
    public class CouponSpec : Specification<Coupons>
    {
        public CouponSpec(int couponId, bool includeRelations = true)
        {
            Query.Where(x => x.Id == couponId);

        }
        public CouponSpec(string code, bool includeRelations = true)
        {
            Query.Where(x => x.Coupon == code);

        }
    }
}
