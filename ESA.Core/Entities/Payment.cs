﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace ESA.Core.Entities
{
    public partial class Payment
    {
        public Payment()
        {
            Refund = new HashSet<Refund>();
        }

        public int Id { get; set; }
        public short PaymentMethodId { get; set; }
        public short PaymentStatusId { get; set; }
        public int? InvoiceId { get; set; }
        /// <summary>
        /// PayPal Id
        /// </summary>
        public string OrderCode { get; set; }
        public string PayCode { get; set; }
        public string AmountCode { get; set; }
        public decimal AmountValue { get; set; }
        public string GrossAmountCode { get; set; }
        public decimal? GrossAmountValue { get; set; }
        public string PaypalFeeCode { get; set; }
        public decimal PaypalFeeValue { get; set; }
        public string NetAmountCode { get; set; }
        public decimal? NetAmountValue { get; set; }
        public string PayerEmailAddress { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastUpdate { get; set; }

        public virtual Invoice Invoice { get; set; }
        public virtual PaymentMethod PaymentMethod { get; set; }
        public virtual PaymentStatus PaymentStatus { get; set; }
        public virtual ICollection<Refund> Refund { get; set; }
    }
}