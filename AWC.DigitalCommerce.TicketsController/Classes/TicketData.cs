using System;
using System.Collections.Generic;
using System.Drawing;

public class TicketData
{
    public string BusinessName { get; set; }

    public string BusinessID { get; set; }

    public string Phone { get; set; }

    public string Address1 { get; set; }

    public string Address2 { get; set; }

    public Image Logo { get; set; }

    public int TicketNumber { get; set; }

    public string Table { get; set; }

    public string Customer { get; set; }

    public string Waiter { get; set; }

    public DateTime Date { get; set; }

    public List<TicketItem> Items { get; set; }

    public decimal Subtotal { get; set; }

    public decimal Service { get; set; }

    public decimal Tax { get; set; }

    public decimal Total { get; set; }

    public decimal TotalUSD { get; set; }

    public string PaymentMethod { get; set; }

    public bool Paid { get; set; }
}