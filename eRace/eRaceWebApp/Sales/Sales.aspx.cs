using eRaceSystem.BLL;
using eRaceSystem.DataModels;
using eRaceSystem.DTOs.Sales;
using eRaceWebApp.Admin.Security;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eRaceWebApp.Sales
{
    public partial class Sales : System.Web.UI.Page
    {

        public int EmployeeId
        {
            get
            {
                int? id = 0;
                if (Request.IsAuthenticated)
                {
                    
                    var controller = new SecurityController();
                    id = controller.GetCurrentUserEmployeeId(User.Identity.Name);
                }
                return (int)id;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            // user items
            if (!User.IsInRole("Clerk"))
                Response.Redirect("~", true);

            var controller = new eRaceController();
            var EmployeeName = controller.GetStoreEmployees(EmployeeId);
            EmployeeLabel.Text = EmployeeName.FullName;

            //if employee cart has items
            var salesController = new SalesController();
            List<CartItems> cartItems = salesController.ItemsInCart(EmployeeId);

            if(cartItems.Count > 0 && !IsPostBack)
            {
                Cart.DataSource = cartItems;
                Cart.DataBind();
            }
            
            

        }
       

        protected void AddItem_Click(object sender, EventArgs e)
        {
            if(Quantity.Text == "")
            {
                MessageUserControl.ShowInfo("Error", "Please input a quantity.");
            }
            else
            {
                if (int.Parse(Quantity.Text) <= 0)
                {
                    MessageUserControl.ShowInfo("Error", "Please input a quantity greater than 0");
                }
                else
                {
                    if (ProductDropDown.SelectedValue == "")
                    {
                        MessageUserControl.ShowInfo("Error", "Please select a product");
                    }
                    else
                    {
                        MessageUserControl.TryRun(() =>
                        {
                            int productID = int.Parse(ProductDropDown.SelectedValue);
                            int quantity = int.Parse(Quantity.Text);
                            decimal amount = 0;
                            decimal tax = 0;
                            decimal allin = 0;

                            var controller = new SalesController();
                            controller.AddItemToCart(productID, quantity, EmployeeId);
                            List<CartItems> cartItems = controller.ItemsInCart(EmployeeId);
                            CategoryDropDown.SelectedValue = null;
                            ProductDropDown.SelectedValue = null;
                            Quantity.Text = "1";
                            Cart.DataSource = cartItems;
                            Cart.DataBind();


                            amount = CalculateTotal();
                            tax = amount * (decimal)0.05;
                            allin = amount + tax;
                            Subtotal.Text = amount.ToString("C");
                            GST.Text = tax.ToString("C");
                            Total.Text = allin.ToString("C");
                        },"Success", $"You have added an item.");
                    }

                }
            }
            
                    
        }

        

        protected void DeleteBtn_Command(object sender, CommandEventArgs e)
        {
            int productId = int.Parse(e.CommandArgument.ToString());
            var controller = new SalesController();


            MessageUserControl.TryRun(() =>
            {
                controller.RemoveCartItem(productId, EmployeeId);
            },"Success","You have removed an item from the cart.");

            List<CartItems> cartItems = controller.ItemsInCart(EmployeeId);

            Cart.DataSource = cartItems;
            Cart.DataBind();

            // Totals Section
            decimal amount = CalculateTotal();
            decimal tax = amount * (decimal)0.05;
            decimal allin = amount + tax;
            Subtotal.Text = amount.ToString("C");
            GST.Text = tax.ToString("C");
            Total.Text = allin.ToString("C");

            
        }

        protected void Refresh_Command(object sender, CommandEventArgs e)
        {
            
            foreach (GridViewRow row in Cart.Rows)
            {
                
                Label amountLabel = row.FindControl("Amount") as Label;
                TextBox qtyTB = row.FindControl("Quantity") as TextBox;
                Label priceLabel = row.FindControl("itemPrice") as Label;


                if(qtyTB.Text == "")
                {
                    MessageUserControl.ShowInfo("Error", "Line quantity cannot empty.");
                }
                else
                {
                    decimal qty = decimal.Parse(qtyTB.Text, NumberStyles.Currency);
                    decimal price = decimal.Parse(priceLabel.Text, NumberStyles.Currency);
                    if (qty <= 0)
                    {
                        MessageUserControl.ShowInfo("Error", "Line quantity cannot be 0 or less.");
                    }
                    else
                    {
                        
                        amountLabel.Text = (price * qty).ToString("C");
                        
                    }
                }
                
            }


            // Totals Section
            decimal amount = CalculateTotal();
            decimal tax = amount * (decimal)0.05;
            decimal allin = amount + tax;

            Subtotal.Text = amount.ToString("C");
            GST.Text = tax.ToString("C");
            Total.Text = allin.ToString("C");
        }



        protected void SaleBtn_Command(object sender, CommandEventArgs e)
        {
            decimal amount;
            decimal price;
            decimal total = 0; 
            int qty;
            int productID;
            var controller = new SalesController();

            //gathering sale items
            List<CartItems> cartItems = new List<CartItems>();
            foreach(GridViewRow row in Cart.Rows)
            {
                HiddenField prodId = row.FindControl("ProductIDHF") as HiddenField;
                TextBox qtyText = row.FindControl("Quantity") as TextBox;
                Label priceLabel = row.FindControl("ItemPrice") as Label;
                Label amountLabel = row.FindControl("Amount") as Label;

                qty = int.Parse(qtyText.Text, NumberStyles.Currency);
                price = decimal.Parse(priceLabel.Text, NumberStyles.Currency);

                amount = qty * price;
                amountLabel.Text = amount.ToString("C");
                
                total += qty * price;
                productID = int.Parse(prodId.Value);
                if (prodId != null && qtyText != null)
                {
                    CartItems item = new CartItems
                    {
                        ProductID = productID,
                        Quantity = qty,
                        ItemPrice = price
                    };
                    cartItems.Add(item);
                    
                }
               
            }
            // processing sale
            MessageUserControl.TryRun(() =>
            {
                
                int InvoiceID = controller.ProcessSale(cartItems, EmployeeId, total);

                // Post sale editing disabled
                Cart.Enabled = false;
                CategoryDropDown.Enabled = false;
                Quantity.Enabled = false;
                AddItem.Enabled = false;
                
                Invoice.Text = InvoiceID.ToString();
                Subtotal.Text = total.ToString("C");
                GST.Text = (total * (decimal)0.05).ToString("C");
                Total.Text = (total * (decimal)1.05).ToString("C");

            }, "Sale complete", "Record of this sale has been made.");
        }

        protected void Clear_Command(object sender, CommandEventArgs e)
        {
            int productID = 0;
            var controller = new SalesController();
            List<CartItems> cartItems = controller.ItemsInCart(EmployeeId);

            if(cartItems.Count() > 0)
            {
                foreach (GridViewRow row in Cart.Rows)
                {
                    HiddenField prodId = row.FindControl("ProductIDHF") as HiddenField;
                    productID = int.Parse(prodId.Value);


                    MessageUserControl.TryRun(() =>
                    {
                        controller.ClearCart(EmployeeId, productID);
                        CategoryDropDown.SelectedIndex = 0;
                        Subtotal.Text = "";
                        GST.Text = "";
                        Total.Text = "";

                    }, "Cart Cleared", "You are free to start a new order.");
                }

                
            }
            else
            {
                Invoice.Text = "";
                Subtotal.Text = "";
                GST.Text = "";
                Total.Text = "";
                Cart.Enabled = true;
                CategoryDropDown.Enabled = true;
                Quantity.Enabled = true;
                AddItem.Enabled = true;
                MessageUserControl.ShowInfo("Cart Cleared", "You are free to start a new order.");
            }
            Cart.DataSource = null;
            Cart.DataBind();


        }

        #region Methods

        public decimal CalculateTotal()
        {
            decimal amount = 0;



            for (int i = 0; i < Cart.Rows.Count; i++)
            {
                GridViewRow row = Cart.Rows[i] as GridViewRow;

                Label amountLabel = row.FindControl("Amount") as Label;
                amount += decimal.Parse(amountLabel.Text, NumberStyles.Currency);

            }

            return amount;
    
        }



        #endregion

       
    }
}