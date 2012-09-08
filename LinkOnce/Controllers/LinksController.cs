using LinkOnce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace LinkOnce.Controllers
{
    public class LinksController : Controller
    {
        IDatabaseContext database;

        public LinksController(IDatabaseContext database)
        {
            this.database = database;
        }

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Links/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Links/Create

        public ActionResult Create()
        {
            ViewBag.Title = "Create a single-use link.";
            ViewBag.Message = "The link you create here will only serve once.  This is useful to limit the sharing of content.";
            Link newLink = new Link();
            return View(newLink);
        }

        //
        // POST: /Links/Create

        [HttpPost]
        public ActionResult Create(Link link)
        {
            ViewBag.Title = "Create a single-use link.";
            ViewBag.Message = "The link you create here will only serve once.  This is useful to limit the sharing of content.";
            ModelState.Remove("ShortUrl");
            try
            {
                if (ModelState.IsValid)
                {
                    string shortLink = Guid.NewGuid().ToString().Substring(0, 6);
                    //TODO - This may not scale
                    while (database.Links.Count(a => a.ShortUrl.Equals(shortLink, StringComparison.OrdinalIgnoreCase)) > 0)
                    {
                        shortLink = Guid.NewGuid().ToString().Substring(0, 6);
                    }
                    link.ShortUrl = shortLink;
                    database.Links.Add(link);
                    database.SaveChanges();
                    return RedirectToAction("Success", new { Id = link.ShortUrl });
                }
                else
                {
                    return View(link);
                }
            }
            catch (Exception e)
            {
                ViewBag.Error = e.GetBaseException().Message;
                return View(link);
            }
        }

        public ActionResult Success(string Id)
        {
            var link = database.Links.FirstOrDefault(l => l.ShortUrl.Equals(Id, StringComparison.OrdinalIgnoreCase));
            if (link != null)
            {
                return View(link);
            }
            else
            {
                return RedirectToAction("NotFound");
            }
        }

        public ActionResult Render(string id)
        {
            var link = database.Links.FirstOrDefault(l => l.ShortUrl.Equals(id, StringComparison.OrdinalIgnoreCase) && !l.IsUsed);
            if (link != null)
            {
#if !DEBUG
                link.IsUsed = true;
                link.DateUsed = DateTime.Now;
                Task.Factory.StartNew(() =>
                {
                    database.SaveChanges();
                });
#endif
                WebRequest wr = WebRequest.Create(link.Destination);
                using (var resp = wr.GetResponse())
                {
                    Response.ContentType = resp.ContentType;
                    foreach (var h in resp.Headers.AllKeys)
                    {
                        Response.Headers.Add(h, resp.Headers[h]);
                    }
                    byte[] buffer = new byte[4096];
                    int bytesRead = 0;
                    long totalBytesRead = 0;
                    using (var streamIn = resp.GetResponseStream())
                    {
                        while (totalBytesRead < resp.ContentLength)
                        {
                            int len = (int)(resp.ContentLength - totalBytesRead > 4096 ? 4096 : resp.ContentLength - totalBytesRead);
                            bytesRead = streamIn.Read(buffer, 0, len);
                            totalBytesRead += bytesRead;
                            Response.OutputStream.Write(buffer, 0, bytesRead);
                        }
                    }
                }
                return null;
            }
            else
            {
                return RedirectToAction("NotFound");
            }
        }

        ////
        //// GET: /Links/Edit/5

        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        ////
        //// POST: /Links/Edit/5

        //[HttpPost]
        //public ActionResult Edit(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add update logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        ////
        // GET: /Links/Delete/5

        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        ////
        //// POST: /Links/Delete/5

        //[HttpPost]
        //public ActionResult Delete(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
