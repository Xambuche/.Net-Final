using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShopOnline.Models;
using System.IO;
using Microsoft.Ajax.Utilities;
using System.Diagnostics;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace ShopOnline.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        private ShopThoiTrangDBContext db = new ShopThoiTrangDBContext();

        // GET: Admin/Product
        public ActionResult Index()
        {
            var list = db.Products
                .Join(
                db.Categorys, p => p.CatID,
                c => c.Id,
                (p, c) => new ProductCategory
                {
                    Id = p.Id,
                    CatID = p.CatID,
                    Name = p.Name,
                    Slug = p.Slug,
                    Detail = p.Detail,
                    Metadesc = p.Metadesc,
                    Metakey = p.Metakey,
                    Img = p.Img,
                    Number = p.Number,
                    Price = p.Price,
                    Pricesale = p.Pricesale,
                    Created_At = p.Created_At,
                    Created_By = p.Created_By,
                    Updated_At = p.Updated_At,
                    Updated_By = p.Updated_By,
                    Status = p.Status,
                    CatName = c.Name
                }
                )
            .Where(m => m.Status != 0)
            .OrderByDescending(m => m.Created_At)
            .ToList();
            return View(list);
        }

        // GET: Admin/Product/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Admin/Product/Create
        public ActionResult Create()
        {
            ViewBag.ListCat = new SelectList(db.Categorys.ToList(), "ID", "Name", 0);
            
            return View();
        }

        // POST: Admin/Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product,HttpPostedFileBase imaged)
            
        {
            db.Products.Add(product);
            db.SaveChanges();
            if(imaged !=null&& imaged.ContentLength > 0)
            {
                int id = int.Parse(db.Products.ToList().Last().Id.ToString());
                string FName = "";
                int index = imaged.FileName.IndexOf('.');
                FName = "product" + id.ToString() + "." + imaged.FileName.Substring(index + 1);
                string PathImg = Path.Combine(Server.MapPath("~/Public/images/Product/"), FName);
                imaged.SaveAs(PathImg);

                Product product1 = db.Products.FirstOrDefault(x => x.Id == id);
                product1.Img = FName;
                db.SaveChanges();
                
            }
            return RedirectToAction("Index");


            /*if (ModelState.IsValid)
            {


                var Img = Request.Files["imaged"];
                string[] FileExtension = { ".jpg", ".png", ".gif" };
                if (Img.ContentLength != 0)
                {
                    if (FileExtension.Contains(Img.FileName.Substring(Img.FileName.LastIndexOf("."))))
                    {
                        //upload file
                        string imgName = Img.FileName.Substring(Img.FileName.LastIndexOf("."));
                        product.Img = imgName; //Luu vao csdl
                        string PathImg = Path.Combine(Server.MapPath("~/Public/images/Product/"), imgName);
                        Img.SaveAs(PathImg);//Luu file len sv
                    }
                }
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ListCat = new SelectList(db.Categorys.ToList(), "ID", "Name", 0);
            return View(product);*/
        }

        // GET: Admin/Product/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.ListCat = new SelectList(db.Categorys.ToList(), "ID", "Name", 0);
           
            return View(product);
        }

        // POST: Admin/Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product, HttpPostedFileBase imaged)
        {

            Product product1 = db.Products.FirstOrDefault(x => x.Id == product.Id);
                    product1.Name = product.Name;
                    product1.CatID = product.CatID;
                    product1.Slug = product.Slug;
                    product1.Detail = product.Detail;
                    product1.Metadesc = product.Metadesc;
                    product1.Metakey = product.Metakey;
                    product1.Img = product.Img;
                    product1.Number = product.Number;
                    product1.Price = product.Price;
                    product1.Status = product.Status;

            if (imaged != null && imaged.ContentLength > 0)
            {
                int id = int.Parse(db.Products.ToList().Last().Id.ToString());
                string FName = "";
                int index = imaged.FileName.IndexOf('.');
                FName = "product" + id.ToString() + "." + imaged.FileName.Substring(index + 1);
                string PathImg = Path.Combine(Server.MapPath("~/Public/images/Product/"), FName);
                imaged.SaveAs(PathImg);

               
                product1.Img = FName;
                

            }
            db.SaveChanges();
            return RedirectToAction("Index");

            /*if (ModelState.IsValid)
            {

                var Img = Request.Files["imaged"];
                string[] FileExtension = { ".jpg", "png", ".gif" };
                if (Img.ContentLength != 0)
                {
                    if (FileExtension.Contains(Img.FileName.Substring(Img.FileName.LastIndexOf("."))))
                    {
                        //upload 
                        string imgName = Img.FileName.Substring(Img.FileName.LastIndexOf("."));
                        //Xoa
                        String DelPath = Path.Combine(Server.MapPath("~/Public/images/Product"), product.Img);
                        if (System.IO.File.Exists(DelPath))
                        {
                            System.IO.File.Delete(DelPath);
                        }

                        product.Img = imgName;
                        string PathImg = Path.Combine(Server.MapPath("~/Public/images/Product"), imgName);
                        Img.SaveAs(PathImg);
                    }
                }

                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ListCat = new SelectList(db.Categorys.ToList(), "ID", "Name", 0);

            return View(product);*/
        }

        // GET: Admin/Product/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Admin/Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Status(int id)
        {
            Product product = db.Products.Find(id);
            int status = (product.Status == 1) ? 2 : 1;
            product.Status = status;
            product.Updated_By = 1;
            product.Updated_At = DateTime.Now;
            db.Entry(product).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");
        }


        //Xoa 
        public ActionResult DelTrash(int id)
        {
            Product product = db.Products.Find(id);
            product.Status = 0;
            product.Updated_By = 1;
            product.Updated_At = DateTime.Now;
            db.Entry(product).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index", "Product");
        }

        //Huy xoa
        public ActionResult ReTrash(int id)
        {
            Product product = db.Products.Find(id);
            product.Status = 2;
            product.Updated_By = 1;
            product.Updated_At = DateTime.Now;
            db.Entry(product).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Trash", "Product");
        }

        public ActionResult Trash()
        {
            var list = db.Products.Where(m => m.Status == 0)
                .OrderByDescending(m => m.Status)
                .ToList();
            return View("Trash", list);
        }
    }
}
