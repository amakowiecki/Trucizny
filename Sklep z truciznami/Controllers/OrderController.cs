using Sklep_z_truciznami.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sklep_z_truciznami.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private Product2Context ProductDb = new Product2Context();
        private Order2Context OrderDb = new Order2Context();
        string orderLabel = "order";

        public ActionResult AddProductToCart(int productID, int quantity)
        {
            Session.Timeout = 30;

            Dictionary<Product, int> order;

            if (Session[orderLabel] != null)
            {
                order = Session[orderLabel] as Dictionary<Product, int>;
            }
            else
            {
                order = new Dictionary<Product, int>();
            }

            Product product = ProductDb.Products.Find(productID);

            bool found = false;
            if (order.Count > 0)
            {
                foreach (var item in order)
                {
                    if (item.Key.ProductId == productID)
                    {
                        order[item.Key] = Math.Min(item.Value + quantity, product.Quantity);
                        found = true;
                        break;
                    }
                }
            }
            if (!found)
            {
                if (quantity > 0) { order.Add(product, Math.Min(product.Quantity, quantity)); }
            }

            Session[orderLabel] = order;

            CheckCartForInvalidQuantity();

            return RedirectToAction("ShowCart");
        }

        public ActionResult RemoveProductFromCart(int productID)
        {
            if (Session[orderLabel] == null) { return View("Error"); }

            Dictionary<Product, int> order = Session[orderLabel] as Dictionary<Product, int>;

            Product productToRemove = order.FirstOrDefault(x => x.Key.ProductId == productID).Key;

            if (productToRemove != null)
            {
                order.Remove(productToRemove);
            }

            Session[orderLabel] = order;

            CheckCartForInvalidQuantity();

            return RedirectToAction("ShowCart");
        }

        public ActionResult EditProductInCart(int productID, int newQuantity)
        {
            RemoveProductFromCart(productID);
            AddProductToCart(productID, newQuantity);
            CheckCartForInvalidQuantity();
            return RedirectToAction("ShowCart");
        }

        // CheckCartForInvalidQuantity
        public ActionResult CheckCartForInvalidQuantity()
        {
            Dictionary<Product, int> order;

            if (Session[orderLabel] != null)
            {
                order = Session[orderLabel] as Dictionary<Product, int>;
                foreach (var orderPair in order)
                {
                    if (orderPair.Value <= 0)
                    {
                        order.Remove(orderPair.Key);
                    }
                }
                Session[orderLabel] = order;
            }
            return RedirectToAction("ShowCart");
        }

        //GET: ShowCart
        public ActionResult ShowCart()
        {
            return View();
        }
        //Get: PlaceOrder
        public ActionResult PlaceOrder()
        {
            return View();
        }

        // POST: PlaceOrder
        [HttpPost]
        public ActionResult PlaceOrder(bool acceptance)
        {

            if (Session[orderLabel] != null && (Session[orderLabel] as Dictionary<Product, int>).Count != 0)
            {
                List<Order> orderList = new List<Order>();

                foreach (var orderPair in (Session[orderLabel] as Dictionary<Product, int>))
                {
                    Order order = new Order();
                    order.ClientId = User.Identity.Name;
                    order.ProductName = orderPair.Key.ProductName;
                    order.OrderDate = DateTime.Now;
                    order.AnswerDate = null;
                    order.ProductId = orderPair.Key.ProductId;
                    order.Quantity = orderPair.Value;
                    order.Status = OrderStatus.Ordered;
                    orderList.Add(order);
                }
                OrderDb.Orders.AddRange(orderList);
                OrderDb.SaveChanges();
                Session[orderLabel] = null;
            }
            else
            {
                ViewBag.message = "Koszyk jest pusty, nie możesz teraz złożyć zamówienia";
                return View("Error");
            }
            return RedirectToAction("ShowOrders");
        }

        //Get: ShowOrders
        public ActionResult ShowOrders()
        {
            
            List<Order> orderList = new List<Order>();
            if (User.IsInRole("Owner"))
            {
                orderList = OrderDb.Orders.ToList();
            }
            else if (User.Identity.IsAuthenticated)
            {
                orderList = OrderDb.Orders.Where(x => x.ClientId == User.Identity.Name).ToList();

            }
            string sortOrder = ViewBag.sortOrder;

            if (orderList != null || orderList.Count < 2)
                orderList.Sort((x, y) => x.OrderDate.CompareTo(y.OrderDate));

            return View(orderList);
        }
        
        public List<Order> GetOrders()
        {
             List<Order> orderList = new List<Order>();
            if (User.IsInRole("Owner"))
            {
                orderList = OrderDb.Orders.ToList();
            }
            else if (User.Identity.IsAuthenticated)
            {
                orderList = OrderDb.Orders.Where(x => x.ClientId == User.Identity.Name).ToList();

            }
            string sortOrder = ViewBag.sortOrder;

            if (orderList != null || orderList.Count < 2)
                orderList.Sort((x, y) => x.OrderDate.CompareTo(y.OrderDate));

            return orderList;
        }

        //get: HandleOrder
        public ActionResult HandleOrder(int orderID, bool acceptance)
        {
            Order order = OrderDb.Orders.Find(orderID);
            if (User.IsInRole("Owner"))
            {
                if (order != null)
                {
                    if (acceptance)
                    {
                        Product product = ProductDb.Products.FirstOrDefault(x => x.ProductId == order.ProductId);
                        if ((product.Quantity - order.Quantity) >= 0)
                        {
                            product.Quantity = product.Quantity - order.Quantity;
                            ProductDb.Entry(product).State = EntityState.Modified;
                            ProductDb.SaveChanges();

                            order.Status = OrderStatus.Realized;
                            ViewBag.Message = "Zamówienie zrealizowane";
                        }
                        else
                        {
                            ViewBag.Message = "Niewystarczająca ilość produktów do realizacji zamówienia";
                        }
                    }
                    else
                    {
                        order.Status = OrderStatus.Rejected;
                        ViewBag.Message = "Zmieniono status na 'Odrzucone'";
                    }

                    order.AnswerDate = DateTime.Now;
                    OrderDb.Entry(order).State = EntityState.Modified;
                    OrderDb.SaveChanges();
                }
            }
            if (User.Identity.Name == order.ClientId)
            {
                if (!acceptance)
                {
                    order.Status = OrderStatus.Cancelled;
                    order.AnswerDate = DateTime.Now;
                    OrderDb.Entry(order).State = EntityState.Modified;
                    OrderDb.SaveChanges();
                    ViewBag.Message = "Zamówienie anulowane";
                }
            }

            return View("ShowOrders",GetOrders());
        }

        public ActionResult ShowCartInMenu()
        {
            return View("PartialCart");
        }
    }
}