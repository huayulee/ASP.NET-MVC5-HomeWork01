﻿using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using CustomerManagementSystem.Models;
using CustomerManagementSystem.Models.ViewModels;
using PagedList;

namespace CustomerManagementSystem.Controllers
{
    public class 客戶資料Controller : BaseController
    {
        private 客戶資料Entities db = new 客戶資料Entities();

        public ActionResult StatisticsList(string keyword, int page = 1)
        {
            keyword = keyword ?? string.Empty;
            ViewBag.keyword = keyword;
            int currentPageIndex = page < 1 ? 1 : page;
            return View(db.vw_客戶統計.Where(x => x.客戶名稱.Contains(keyword)).OrderBy(x => x.客戶名稱).ToPagedList(currentPageIndex, this.defaultPageSize));
        }

        // GET: 客戶資料
        public ActionResult Index(string keyword, int page = 1)
        {
            keyword = keyword ?? string.Empty;
            ViewBag.keyword = keyword;
            int currentPageIndex = page < 1 ? 1 : page;
            return View(db.客戶資料.Where(x => x.Is刪除 == false && (x.客戶名稱.Contains(keyword) || x.統一編號.Contains(keyword) || x.電話.Contains(keyword) || x.傳真.Contains(keyword) || x.地址.Contains(keyword) || x.Email.Contains(keyword))).OrderBy(x => x.Id).ToPagedList(currentPageIndex, this.defaultPageSize));
        }

        public ActionResult ShowList(int? id, string customerName)
        {
            ViewBag.showEdit = false;
            ViewBag.customerName = customerName;
            CustomerInfoViewModel viewModel = new CustomerInfoViewModel();
            viewModel.客戶聯絡人s = db.客戶聯絡人.Where(x => x.Is刪除 == false && x.客戶Id == id).OrderBy(x => x.姓名).ToPagedList(1, int.MaxValue);
            viewModel.客戶銀行資訊s = db.客戶銀行資訊.Where(x => x.Is刪除 == false && x.客戶Id == id).OrderBy(x => x.銀行代碼).ToPagedList(1, int.MaxValue);

            return PartialView("_ShowList", viewModel);
        }

        // GET: 客戶資料/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 客戶資料 = db.客戶資料.Find(id);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            return View(客戶資料);
        }

        // GET: 客戶資料/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: 客戶資料/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,客戶名稱,統一編號,電話,傳真,地址,Email")] 客戶資料 客戶資料)
        {
            if (ModelState.IsValid)
            {
                db.客戶資料.Add(客戶資料);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(客戶資料);
        }

        // GET: 客戶資料/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 客戶資料 = db.客戶資料.Find(id);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            return View(客戶資料);
        }

        // POST: 客戶資料/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,客戶名稱,統一編號,電話,傳真,地址,Email")] 客戶資料 客戶資料)
        {
            if (ModelState.IsValid)
            {
                db.Entry(客戶資料).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(客戶資料);
        }

        // GET: 客戶資料/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            客戶資料 客戶資料 = db.客戶資料.Find(id);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }

            return View(客戶資料);
        }

        // POST: 客戶資料/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            客戶資料 客戶資料 = db.客戶資料.Find(id);
            var 客戶聯絡人們 = db.客戶聯絡人.Where(x => x.客戶Id == id);
            客戶資料.Is刪除 = true;
            foreach (客戶聯絡人 item in 客戶聯絡人們)
            {
                item.Is刪除 = true;
            }

            var 客戶所有營行資訊 = db.客戶銀行資訊.Where(x => x.客戶Id == id);
            foreach (客戶銀行資訊 item in 客戶所有營行資訊)
            {
                item.Is刪除 = true;
            }

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}