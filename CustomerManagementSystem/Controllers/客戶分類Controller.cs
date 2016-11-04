using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CustomerManagementSystem.ActionFilter;
using CustomerManagementSystem.Models;
using PagedList;

namespace CustomerManagementSystem.Controllers
{
    [ActionTime]
    [HandleError(ExceptionType = typeof(InvalidOperationException),View = "Error_InvalidOperationException")]
    public class 客戶分類Controller : BaseController
    {
        private 客戶分類Repository repo = RepositoryHelper.Get客戶分類Repository();

        // GET: 客戶分類
        public ActionResult Index(string keyword, int page = 1)
        {
            keyword = keyword ?? string.Empty;
            int currentPageIndex = page < 1 ? 1 : page;
            
            return View(repo.SelectAllWithKeyword(keyword).ToPagedList(currentPageIndex, this.defaultPageSize));
        }

        // GET: 客戶分類/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ////客戶分類 客戶分類 = db.客戶分類.Find(id);
            客戶分類 客戶分類 = this.repo.Find(id.Value);
            if (客戶分類 == null)
            {
                return HttpNotFound();
            }

            return View(客戶分類);
        }

        // GET: 客戶分類/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: 客戶分類/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,分類名稱,Is刪除")] 客戶分類 客戶分類)
        {
            if (ModelState.IsValid)
            {
                //db.客戶分類.Add(客戶分類);
                //db.SaveChanges();
                this.repo.Add(客戶分類);
                this.repo.UnitOfWork.Commit();

                return RedirectToAction("Index");
            }

            return View(客戶分類);
        }

        // GET: 客戶分類/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //客戶分類 客戶分類 = db.客戶分類.find(id);
            客戶分類 客戶分類 = this.repo.Find(id.Value);

            if (客戶分類 == null)
            {
                return HttpNotFound();
            }
            return View(客戶分類);
        }

        // POST: 客戶分類/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, FormCollection form)
        {
            var 客戶分類 = this.repo.Find(id);
            if (TryUpdateModel<客戶分類>(客戶分類))
            {
                this.repo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }

            return View(客戶分類);
        }

        // GET: 客戶分類/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶分類 客戶分類 = this.repo.Find(id.Value);

            if (客戶分類 == null)
            {
                return HttpNotFound();
            }
            return View(客戶分類);
        }

        // POST: 客戶分類/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            客戶分類 客戶分類 = this.repo.Find(id);
            this.repo.Delete(客戶分類);
            this.repo.UnitOfWork.Commit();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.repo.UnitOfWork.Context.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
