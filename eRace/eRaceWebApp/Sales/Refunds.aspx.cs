using eRaceSystem.BLL;
using eRaceSystem.DataModels;
using eRaceWebApp.Admin.Security;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using eRaceSystem.DTOs.Sales;

namespace eRaceWebApp.Sales
{
    public partial class Refunds : System.Web.UI.Page
    {
        public int EmployeeId
        {
            get
            {
                int? id = 0;
                if (Request.IsAuthenticated)
                {
                    var manager = Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
                    var controller = new SecurityController();
                    id = controller.GetCurrentUserEmployeeId(User.Identity.Name);
                }
                return (int)id;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.IsInRole("Clerk"))
                Response.Redirect("~", true);

            var controller = new eRaceController();
            var EmployeeName = controller.GetStoreEmployees(EmployeeId);
            EmployeeLabel.Text = EmployeeName.FullName;

           
            decimal amount = CalculateTotal();
            decimal tax = amount * (decimal)0.05;
            decimal total = amount + tax;

            Subtotal.Text = amount.ToString("C");
            GST.Text = tax.ToString("C");
            Total.Text = total.ToString("C");
        }
        protected void CheckForException(object sender, ObjectDataSourceStatusEventArgs e)
        {
            MessageUserControl.HandleDataBoundException(e);
        }

        protected void LookupBtn_Click(object sender, EventArgs e)
        {
            int orderNumber;

            bool isNumeric = int.TryParse(OrderNumber.Text, out orderNumber);
            

            if(isNumeric.Equals(false))
            {
                MessageUserControl.ShowInfo("Error", "Order Number is a numeric only field.");
            }
            else
            {
                if (string.IsNullOrEmpty(OrderNumber.Text))
                {
                    MessageUserControl.ShowInfo("Error", "Please input the original order number.");
                }
                else
                {
                    int orderID = int.Parse(OrderNumber.Text);
                    var controller = new SalesController();
                    var refundList = controller.RefundbyOrderNumber(orderID);

                    if (refundList.Count == 0)
                    {
                        MessageUserControl.ShowInfo("No Results Found", "Order Number does not exist.");
                    }
                    else
                    {
                        ReturnGV.DataSource = refundList;
                        ReturnGV.DataBind();

                        foreach (GridViewRow row in ReturnGV.Rows)
                        {
                            HiddenField isRefund = row.FindControl("IsRefundable") as HiddenField;
                            CheckBox restock = row.FindControl("RestockSelect") as CheckBox;
                            CheckBox reasonCheck = row.FindControl("ReasonSelect") as CheckBox;
                            TextBox reasonTB = row.FindControl("Reason") as TextBox;


                            if (isRefund.Value == "False")
                            {
                                restock.Visible = false;
                                reasonCheck.Visible = false;
                                reasonTB.Visible = false;
                            }
                            
                        }
                        ReturnGV.Enabled = true;
                    }


                }
            }

        }

      

        protected void RefundBtn_Click(object sender, EventArgs e)
        {
            decimal amountTotal;
            decimal total = 0;
            int checkedBoxes = 0;
            int originalInvoice = int.Parse(OrderNumber.Text);
            List<Refund> refundItems = new List<Refund>();
            if (string.IsNullOrEmpty(OrderNumber.Text))
            {
                MessageUserControl.ShowInfo("Error", "Please input the original order number.");
            }
            else
            {
                foreach (GridViewRow row in ReturnGV.Rows)
                {
                    
                    HiddenField prodId = row.FindControl("ProductID") as HiddenField;
                    CheckBox reasonCheck = row.FindControl("ReasonSelect") as CheckBox;
                    TextBox reasonTB = row.FindControl("Reason") as TextBox;
                    Label itemName = row.FindControl("ProductName") as Label;
                    Label qty = row.FindControl("Quantity") as Label;
                    Label amount = row.FindControl("Amount") as Label;

                    if(reasonCheck.Checked)
                    {
                        checkedBoxes++;
                    }

                    
                    amountTotal = decimal.Parse(amount.Text, NumberStyles.Currency);

                    if (reasonCheck.Checked && reasonTB.Text != "")
                    {
                        total -= amountTotal;
                    }

                    if (reasonCheck.Checked && reasonTB.Text == "")
                    {
                        MessageUserControl.ShowInfo("Error", "You must input a reason for returned items");
                    }
                    else
                    {
                        if (reasonCheck.Checked && reasonTB.Text != "")
                        {
                            Refund refund = new Refund
                            {
                                ItemName = itemName.Text,
                                Quantity = int.Parse(qty.Text),
                                ProductID = int.Parse(prodId.Value),
                                OriginalInvoiceID = int.Parse(originalInvoice.ToString()),
                                Reason = reasonTB.Text
                            };
                            refundItems.Add(refund);

                        }
                    }

                }
                if(refundItems.Count == checkedBoxes)
                {
                    MessageUserControl.TryRun(() =>
                    {
                        var controller = new SalesController();
                        int InvoiceID = controller.ProcessRefund(refundItems, EmployeeId, total, originalInvoice);

                        ReturnGV.Enabled = false;
                        Invoice.Text = InvoiceID.ToString();
                        Subtotal.Text = total.ToString("C");
                        GST.Text = (total * (decimal)0.05).ToString("C");
                        Total.Text = (total * (decimal)1.05).ToString("C");

                    }, "Return Complete", "Items can be set aside for managerial inspection.");
                }
                else
                {
                    MessageUserControl.ShowInfo("Error", "You must input a reason for returned items");
                }
               
            }   

        }

        protected void ClearBtn_Click(object sender, EventArgs e)
        {
            
            OrderNumber.Text = null;
            ReturnGV.DataSource = null;
            ReturnGV.DataBind();
            MessageUserControl.ShowInfo("Refund Cancelled.", "Please input the original order number.");
            Subtotal.Text = 0.00.ToString("C");
            GST.Text = 0.00.ToString("C");
            Total.Text = 0.00.ToString("C");
            Invoice.Text = null;
            ReturnGV.Enabled = true;
        }


        public decimal CalculateTotal()
        {
            decimal amount = 0;



            for (int i = 0; i < ReturnGV.Rows.Count; i++)
            {
                GridViewRow row = ReturnGV.Rows[i] as GridViewRow;

                CheckBox restock = row.FindControl("RestockSelect") as CheckBox;
                CheckBox reasonCheck = row.FindControl("ReasonSelect") as CheckBox;
                Label amountLabel = row.FindControl("Amount") as Label;
                Label restockLabel = row.FindControl("Restock") as Label;
                TextBox reasonTb = row.FindControl("Reason") as TextBox;

                if (reasonCheck.Checked && restock.Checked)
                {
                    amount -= decimal.Parse(amountLabel.Text, NumberStyles.Currency) - decimal.Parse(restockLabel.Text, NumberStyles.Currency);
                }
                else if (reasonCheck.Checked)
                {
                    amount -= decimal.Parse(amountLabel.Text, NumberStyles.Currency);
                }


            }

            return amount;

        }
    }


}