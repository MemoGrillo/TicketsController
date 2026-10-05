using AWC.DigitalCommerce.TicketsController.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.Windows.Forms;

namespace AWC.DigitalCommerce.TicketsController
{
    public class xPrinterTicket2
    {
        private string workVar = string.Empty;
        private PrintDocument pdoc = null;
        private clsTicketsForDataGrid ticket = new clsTicketsForDataGrid();
        private int step = 0;

        public xPrinterTicket2()
        {

        }

        public xPrinterTicket2(clsTicketsForDataGrid _ticket, string newName = "")
        {
            ticket = _ticket;

            if (newName.Length > 0)
            {
                ticket.CustomerID = newName;
            }
        }

        public void print()
        {
            try
            {
                if (Settings.Default.TicketPrinter.Length == 0) return;

                PrintDialog pd = new PrintDialog();
                pdoc = new PrintDocument();

                PrinterSettings ps = new PrinterSettings();
                PaperSize psize = new PaperSize("Custom", Settings.Default.TicketWidth, Settings.Default.TicketLength);

                pd.Document = pdoc;
                pd.Document.DefaultPageSettings.PaperSize = psize;
                pdoc.DefaultPageSettings.PaperSize.Width = Settings.Default.TicketWidth;
                pdoc.DefaultPageSettings.PaperSize.Height = Settings.Default.TicketLength;
                pdoc.DefaultPageSettings.PrinterSettings.PrinterName = Settings.Default.TicketPrinter;

                pdoc.PrintPage += new PrintPageEventHandler(pdoc_PrintPage);
                pdoc.Print();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Logger.WriteToLog(Constants.Titles.SHORTGAPPTITLE, ex, Logger.Severity.ERROR);
            }
        }

        void pdoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            try
            {
                Graphics graphics = e.Graphics;

                int startX = 0;
                int startY = 0;
                int Offset = 0;

                if (Settings.Default.BusinessName.Length > 0)
                {
                    graphics.DrawString(Helper.FormatGralLine2(Settings.Default.BusinessName), new Font("Consolas", 10), new SolidBrush(Color.Black), startX, startY + Offset);
                    Offset += 15;

                }

                if (Settings.Default.BusinessID.Length > 0)
                {
                    graphics.DrawString(Helper.FormatGralLine2(Settings.Default.BusinessID), new Font("Consolas", 10), new SolidBrush(Color.Black), startX, startY + Offset);
                    Offset += 15;
                }

                if (Settings.Default.BusinessPhoneNumber.Length > 0)
                {
                    graphics.DrawString(Helper.FormatGralLine2(Settings.Default.BusinessPhoneNumber), new Font("Consolas", 10), new SolidBrush(Color.Black), startX, startY + Offset);
                    Offset += 15;
                }

                if (Settings.Default.BusinessAddress1.Length > 0)
                {
                    graphics.DrawString(Helper.FormatGralLine2(Settings.Default.BusinessAddress1), new Font("Consolas", 10), new SolidBrush(Color.Black), startX, startY + Offset);
                    Offset += 15;
                }

                if (Settings.Default.BusinessAddress2.Length > 0)
                {
                    graphics.DrawString(Helper.FormatGralLine2(Settings.Default.BusinessAddress2), new Font("Consolas", 10), new SolidBrush(Color.Black), startX, startY + Offset);
                    Offset += 15;
                }

                workVar = $"TURNO ACTIVO: {Settings.Default.Shift}";
                graphics.DrawString(Helper.FormatGralLine2(workVar), new Font("Consolas", 10), new SolidBrush(Color.Black), startX, startY + Offset);
                Offset += 15;

                workVar = "FEC: " + ticket.TicketDate + "  FOLIO: " + ticket.ID.ToString("000000");
                graphics.DrawString(Helper.FormatGralLine2(workVar), new Font("Consolas", 10), new SolidBrush(Color.Black), startX, startY + Offset);
                Offset += 25;

                graphics.DrawString(Helper.FormatGralLine2(ticket.CustomerAKA), new Font("Consolas", 10), new SolidBrush(Color.Black), startX, startY + Offset);
                Offset += 25;

                clsTicket t = DB.GetTicket(ticket.ID);
                clsUser userProf = Helper.CheckUserProfile(t.WhoOpened.ToString());
                workVar = "ATENDIDO POR: " + userProf.userName;
                graphics.DrawString(Helper.FormatGralLine2(workVar), new Font("Consolas", 10), new SolidBrush(Color.Black), startX, startY + Offset);
                Offset += 15;

                workVar = $"CREADO: {t.CreateAt.ToString("dd-MM-yyyy hh:mm tt")}";
                graphics.DrawString(Helper.FormatGralLine2(workVar), new Font("Consolas", 10), new SolidBrush(Color.Black), startX, startY + Offset);
                Offset += 25;

                workVar = "CANT DESCRIPCIÓN                          PRECIO";
                graphics.DrawString(workVar, new Font("Consolas", 10), new SolidBrush(Color.Black), startX, startY + Offset);
                Offset += 18;

                graphics.DrawString(new string('=', 48), new Font("Consolas", 10), new SolidBrush(Color.Black), startX, startY + Offset);
                Offset += 18;

                clsTicket tck = new clsTicket();
                int custID = 0;
                string guid = string.Empty;

                if (ticket.ID > 0)
                {
                    tck = DB.GetTicket(ticket.ID);
                    guid = tck.GUID;
                }
                else
                {
                    custID = DB.GetIDByCustomerID(ticket.CustomerID);
                    guid = DB.GetTicketGUID(Helper.RevertFormatDate(ticket.TicketDate), custID, Convert.ToInt32(ticket.Status));
                }

                List<clsItemDetailForDatagrid> lstItems = DB.GetItemsByGUID(guid, Settings.Default.AllowTicketSummary);

                int totalPrice = 0;
                int totalCash = 0;

                foreach (clsItemDetailForDatagrid itemDet in lstItems)
                {
                    if (itemDet.ItemDesc == null)
                    {
                        itemDet.ItemDesc = "PRODUCTO ELIMINADO";
                    }
                    else
                    {
                        if (itemDet.ItemDesc.Contains("EFECTIVO"))
                        {
                            totalCash += itemDet.TotalPrice;
                            continue;
                        }
                    }

                    workVar = Helper.FormatItemDetailLine2(itemDet);
                    graphics.DrawString(workVar, new Font("Consolas", 10), new SolidBrush(Color.Black), startX, startY + Offset);
                    Offset += 18;
                    totalPrice += itemDet.TotalPrice;
                }

                graphics.DrawString(new string('=', 48), new Font("Consolas", 10), new SolidBrush(Color.Black), startX, startY + Offset);
                Offset += 18;

                string subTot = totalPrice.ToString("N0", CultureInfo.GetCultureInfo("en-US"));
                workVar = new string(' ', 31) + "SUBTOTAL: " + subTot.PadLeft(7);
                graphics.DrawString(workVar, new Font("Consolas", 10), new SolidBrush(Color.Black), startX, startY + Offset);
                Offset += 18;

                // SERVICE FEE
                string serviceFee = ticket.ServiceFee.ToString("N0", CultureInfo.GetCultureInfo("en-US"));
                workVar = new string(' ', 27) + "10% SERVICIO: " + serviceFee.PadLeft(7);
                graphics.DrawString(workVar, new Font("Consolas", 10), new SolidBrush(Color.Black), startX, startY + Offset);
                Offset += 18;

                // IVA Fee
                if (Settings.Default.ATVApplyFee)
                {
                    string ivaFee = ticket.IVAFee.ToString("N0", CultureInfo.GetCultureInfo("en-US"));
                    workVar = new string(' ', 32) + "13% IVA: " + ivaFee.PadLeft(7);
                    graphics.DrawString(workVar, new Font("Consolas", 10), new SolidBrush(Color.Black), startX, startY + Offset);
                    Offset += 18;
                }

                // CASH IN ADVANCE
                if (totalCash > 0)
                {
                    Offset += 10;
                    workVar = new string(' ', 32) + "TOTAL VENTA : " + (totalPrice + ticket.ServiceFee + ticket.IVAFee).ToString("N0", CultureInfo.GetCultureInfo("en-US")).PadLeft(7);
                    graphics.DrawString(workVar, new Font("Consolas", 10), new SolidBrush(Color.Black), startX, startY + Offset);
                    Offset += 18;

                    workVar = new string(' ', 26) + "MÁS EFECTIVO : " + totalCash.ToString("N0", CultureInfo.GetCultureInfo("en-US")).PadLeft(7);
                    graphics.DrawString(workVar, new Font("Consolas", 10), new SolidBrush(Color.Black), startX, startY + Offset);
                    Offset += 18;

                    workVar = new string(' ', 28) + "POR COBRAR : " + (totalPrice + ticket.ServiceFee + +ticket.IVAFee + totalCash).ToString("N0", CultureInfo.GetCultureInfo("en-US")).PadLeft(7);
                    graphics.DrawString(workVar, new Font("Consolas", 10), new SolidBrush(Color.Black), startX, startY + Offset);
                    Offset += 30;
                }
                else
                {
                    Offset += 12;

                    // TICKET TOTAL
                    string tot = (totalPrice + ticket.ServiceFee + ticket.IVAFee).ToString("N0", CultureInfo.GetCultureInfo("en-US"));
                    graphics.DrawString(new string(' ', 14) + tot, new Font("Arial Narrow", 30), new SolidBrush(Color.Black), startX, startY + Offset);
                    Offset += 50;

                    // TICKET IN US DOLLARS
                    if (Settings.Default.USDollarExchangeRate > 0)
                    {
                        int totalUSD = (int)Math.Ceiling((double)(totalPrice + ticket.ServiceFee + ticket.IVAFee) / (double)Settings.Default.USDollarExchangeRate);
                        string totUSD = "USD " + (totalUSD).ToString("N0", CultureInfo.GetCultureInfo("en-US"));
                        graphics.DrawString(Helper.FormatGralLine2(totUSD), new Font("Consolas", 10), new SolidBrush(Color.Black), startX, startY + Offset);
                        Offset += 30;
                    }
                }

                // PAYMENT MODE
                if (!ticket.Status)
                {
                    string payMethod = string.Empty;

                    if (ticket.Cash > 0 && ticket.CreditCard == 0 && ticket.Transfer == 0)
                        payMethod = "EFECTIVO";

                    if (ticket.Cash == 0 && ticket.CreditCard > 0 && ticket.Transfer == 0)
                        payMethod = "TARJETA DE CRÉDITO";

                    if (ticket.Cash == 0 && ticket.CreditCard == 0 && ticket.Transfer > 0)
                        payMethod = "SINPE";

                    // MIXED PAYMENT
                    if (ticket.Cash > 0 && ticket.CreditCard > 0 && ticket.Transfer == 0)
                    {
                        payMethod = "EFEC+TARJ";
                    }

                    if (ticket.Cash > 0 && ticket.CreditCard == 0 && ticket.Transfer > 0)
                    {
                        payMethod = "EFEC+SINPE";
                    }

                    if (ticket.Cash == 0 && ticket.CreditCard > 0 && ticket.Transfer > 0)
                    {
                        payMethod = "TARJ+SINPE";
                    }

                    if (ticket.Cash > 0 && ticket.CreditCard > 0 && ticket.Transfer > 0)
                    {
                        payMethod = "EFEC+TARJ+SINPE";
                    }

                    Offset += 10;
                    graphics.DrawString(new string(' ', 24 - payMethod.Length) + payMethod, new Font("Consolas Bold", 10), new SolidBrush(Color.Black), startX, startY + Offset);
                    Offset += 30;
                }

                workVar = ticket.Status ? "*PENDIENTE*" : "*CANCELADA*";
                graphics.DrawString(Helper.FormatGralLine2(workVar), new Font("Arial Narrow", 20), new SolidBrush(Color.Black), startX, startY + Offset);
                Offset += 30;

                workVar = "GRACIAS POR SU VISITA";
                graphics.DrawString(Helper.FormatGralLine2(workVar), new Font("Consolas Bold", 14), new SolidBrush(Color.Black), startX, startY + Offset);
                Offset += 30;

                graphics.DrawString(Helper.FormatGralLine2(Environment.MachineName), new Font("Consolas", 10), new SolidBrush(Color.Black), startX, startY + Offset);
                Offset += 15;

                graphics.DrawString(Helper.FormatGralLine2($"© 2021 - {DateTime.Now.ToString("yyyy")} AIDAWARE"), new Font("Consolas", 10), new SolidBrush(Color.Black), startX, startY + Offset);
                Offset += 30;

                // Cut line
                workVar = ".   .    .    .    .    .    .";
                graphics.DrawString(workVar, new Font("Consolas", 10), new SolidBrush(Color.Black), startX, startY + Offset);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Logger.WriteToLog(Constants.Titles.SHORTGAPPTITLE, ex, Logger.Severity.ERROR);
            }
        }
    }
}
