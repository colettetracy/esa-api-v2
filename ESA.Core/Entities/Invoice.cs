﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace ESA.Core.Entities
{
    public partial class Invoice
    {
        public Invoice()
        {
            InvoiceDetail = new HashSet<InvoiceDetail>();
            Payment = new HashSet<Payment>();
        }

        public int Id { get; set; }
        public int InvoiceConfigId { get; set; }
        public string Code { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Discount { get; set; }
        public decimal NetAmount { get; set; }
        public string NetAmountWords { get; set; }
        public decimal ReceivedAmount { get; set; }
        public decimal ChangeAmount { get; set; }
        public string CustomerName { get; set; }
        public string CustomerRtn { get; set; }
        public string NumberCerExonerated { get; set; }
        public string NumberExeOrder { get; set; }
        public string NumberRegSag { get; set; }
        public decimal TaxExemptAmount { get; set; }
        public decimal TaxExoneratedAmount { get; set; }
        public bool PaymentConfirmed { get; set; }
        public DateTime LastUpdate { get; set; }

        public virtual InvoiceConfig InvoiceConfig { get; set; }
        public virtual ICollection<InvoiceDetail> InvoiceDetail { get; set; }
        public virtual ICollection<Payment> Payment { get; set; }
    }
}