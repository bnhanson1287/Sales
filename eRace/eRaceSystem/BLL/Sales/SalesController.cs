using eRaceSystem.DAL;
using eRaceSystem.DataModels;
using eRaceSystem.DTOs.Sales;
using eRaceSystem.Entities;
using eRaceSystem.POCO.Sales;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eRaceSystem.BLL
{

    
    [DataObject]
    public class SalesController
    {
        #region Sales
        [DataObjectMethod(DataObjectMethodType.Select)]
        public IEnumerable<ProductSelection> ListProducts()
        {
            using (var context = new eRaceContext())
            {
                var results = from items in context.Products.ToList()
                              select new ProductSelection
                              {
                                  ProductID = items.ProductID,
                                  ItemName = items.ItemName
                              };
                return results;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public IEnumerable<ProductSelection> ListProducts_ByCategory(int CategoryID)
        {
            using (var context = new eRaceContext())
            {
                var results = from aCategory in context.Products.ToList()
                              where aCategory.CategoryID.Equals(CategoryID)
                              select new ProductSelection
                              {
                                  ProductID = aCategory.ProductID,
                                  ItemName = aCategory.ItemName
                              };

                return results;
            }
        }
        [DataObjectMethod(DataObjectMethodType.Select)]
        public IEnumerable<CategorySelection> ListCategory()
        {
            using (var context = new eRaceContext())
            {
                var results = from category in context.Categories.ToList()
                              select new CategorySelection
                              {
                                  CategoryID = category.CategoryID,
                                  Description = category.Description
                              };
                return results;
               
            }
        }

        public void AddItemToCart(int productID, int quantity, int EmployeeId)
        {
            using(var context = new eRaceContext())
            {

                var exists = (from item in context.SalesCartItems where item.EmployeeID == EmployeeId && item.ProductID == productID select item).FirstOrDefault();

                if(exists == null)
                {
                    SalesCartItem inCart = new SalesCartItem();

                    inCart.EmployeeID = EmployeeId;
                    inCart.ProductID = productID;
                    inCart.Quantity = quantity;
                    context.SalesCartItems.Add(inCart);
                }
                else
                {
                   
                    var newQty = context.SalesCartItems.Where(x => x.EmployeeID == EmployeeId && x.ProductID == productID).FirstOrDefault();

                    newQty.Quantity += quantity;
                }
                    
                context.SaveChanges();
            }
        }

        public List<CartItems> ItemsInCart(int employeeId)
        {
            using (var context = new eRaceContext())
            {
                var shoppingCart = from x in context.SalesCartItems
                                   where x.EmployeeID.Equals(employeeId) 
                                   select new CartItems
                                   {
                                       ProductID = x.ProductID,
                                       ItemName = x.Product.ItemName,
                                       Quantity = x.Quantity,
                                       ItemPrice = x.Product.ItemPrice,
                                       Amount = x.Quantity * x.Product.ItemPrice
                                   };
                return shoppingCart.ToList();
            }
        }

        public void RemoveCartItem(int productID, int employeeID)
        {
            using(var context = new eRaceContext())
            {
                var result = context.SalesCartItems.Where(x => x.ProductID == productID && x.EmployeeID == employeeID).FirstOrDefault();

                context.SalesCartItems.Remove(result);
                context.SaveChanges();
            }
        }
        public void ClearCart(int employeeID, int productID)
        {
            using(var context = new eRaceContext())
            {
                var result = context.SalesCartItems.Find(employeeID,productID);
                context.SalesCartItems.Remove(result);
                context.SaveChanges();

            }
        }

        public int ProcessSale(List<CartItems> cartItems, int EmployeeID, decimal total)
        {
            using(var context = new eRaceContext())
            {

                if(cartItems == null)
                {
                    throw new Exception("There are no items in the cart.");
                }


                var invoice = new Invoice
                {
                    EmployeeID = EmployeeID,
                    InvoiceDate = DateTime.Now,
                    SubTotal = total,
                    GST = Math.Round(total * (decimal)0.05, 2),
                    Total = Math.Round(total * (decimal)1.05, 2)
                };
               
                foreach(var item in cartItems)
                {
                    invoice.InvoiceDetails.Add(new InvoiceDetail
                    {
                        ProductID = item.ProductID,
                        Quantity = item.Quantity,
                        Price = item.ItemPrice

                    });

                    var newQOH = context.Products.Where(x => x.ProductID == item.ProductID).FirstOrDefault();

                    newQOH.QuantityOnHand -= item.Quantity;

                    ClearCart(EmployeeID, item.ProductID);
                }

                context.Invoices.Add(invoice);

                context.SaveChanges();

                return invoice.InvoiceID;

            }
        }
        #endregion


        #region Refunds
      
        public List<Refund> RefundbyOrderNumber(int orderid)
        {
            using(var context = new eRaceContext())
            {
                //List<int> returned = (from item in context.StoreRefunds where item.OriginalInvoiceID == orderid select item.ProductID).ToList();


                var result = from items in context.InvoiceDetails
                                 where items.InvoiceID == orderid
                                 select new Refund
                                 {
                                     ProductID = items.ProductID,
                                     ItemName = items.Product.ItemName,
                                     Price = items.Product.ItemPrice,
                                     Quantity = items.Quantity,
                                     Amount = items.Quantity * items.Product.ItemPrice,
                                     IsRefundable = (items.Product.Category.Description == "Confectionary" ? false : true),
                                     RestockCharge = items.Product.ReStockCharge * items.Quantity                                    
                                 };
                    return result.ToList();
                
            }
        }
        public int ProcessRefund(List<Refund> returns, int employeeId, decimal total, int orderID)
        {
            using(var context = new eRaceContext())
            {
                if (total == 0)
                {
                    throw new Exception("There are no returns on this order.");
                }

                List<int> returned = (from item in context.StoreRefunds where item.OriginalInvoiceID == orderID select item.ProductID).ToList();


                var invoice = new Invoice
                {
                    EmployeeID = employeeId,
                    InvoiceDate = DateTime.Now,
                    SubTotal = total,
                    GST = Math.Round(total * (decimal)0.05,2),
                    Total = Math.Round(total * (decimal)1.05, 2)
                };

                foreach(var item in returns)
                {
                    bool alreadyReturned = returned.Contains(item.ProductID);

                    if(alreadyReturned == false)
                    {
                        if (item.Reason != "")
                        {
                            invoice.StoreRefunds.Add(new StoreRefund
                            {

                                InvoiceID = invoice.InvoiceID,
                                ProductID = item.ProductID,
                                OriginalInvoiceID = orderID,
                                Reason = item.Reason


                            });
                            var newQOH = context.Products.Where(x => x.ProductID == item.ProductID).FirstOrDefault();

                            newQOH.QuantityOnHand += item.Quantity;
                        }
                        else
                        {
                            throw new Exception($"{item.ItemName} requires a return reason.");
                        }
                    }
                    else
                    {
                        throw new Exception($"{item.ItemName} has already been returned");
                    }

                       

                    
                }
                context.Invoices.Add(invoice);

                context.SaveChanges();

                return invoice.InvoiceID;


            }
            
        }
        #endregion
    }
}
