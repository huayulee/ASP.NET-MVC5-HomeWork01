using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CustomerManagementSystem.Models;
using NPOI.HSSF.UserModel;
using PagedList;

namespace CustomerManagementSystem.Controllers
{
    public class 客戶銀行資訊Controller : BaseController
    {
        //private 客戶資料Entities db = new 客戶資料Entities();
        private 客戶銀行資訊Repository repo = RepositoryHelper.Get客戶銀行資訊Repository();
        private 客戶資料Repository repo客戶 = RepositoryHelper.Get客戶資料Repository();

        // GET: 客戶銀行資訊
        public ActionResult Index(string keyword, int page = 1)
        {
            keyword = keyword ?? string.Empty;
            ViewBag.keyword = keyword;
            int currentPageIndex = page < 1 ? 1 : page;
            //var 客戶銀行資訊 = db.客戶銀行資訊.Include(客 => 客.客戶資料).Where(x => x.Is刪除 == false && (x.銀行名稱.Contains(keyword) || (x.銀行代碼.ToString()).Contains(keyword) || x.帳戶名稱.Contains(keyword) || x.帳戶號碼.Contains(keyword) || x.客戶資料.客戶名稱.Contains(keyword))).OrderBy(x => x.Id).ToPagedList(currentPageIndex, this.defaultPageSize);

            // 為何抓得到主表資料?
            var 客戶銀行資訊 = this.repo.SelectByKeyWord(keyword).ToPagedList(currentPageIndex, this.defaultPageSize);

            return View(客戶銀行資訊);
        }

        // GET: 客戶銀行資訊/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //客戶銀行資訊 客戶銀行資訊 = db.客戶銀行資訊.Find(id);
            客戶銀行資訊 客戶銀行資訊 = this.repo.Find(id.Value);

            if (客戶銀行資訊 == null)
            {
                return HttpNotFound();
            }

            return View(客戶銀行資訊);
        }

        // GET: 客戶銀行資訊/Create
        public ActionResult Create()
        {
            this.GenCustomerList();
            return View();
        }

        // POST: 客戶銀行資訊/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,客戶Id,銀行名稱,銀行代碼,分行代碼,帳戶名稱,帳戶號碼,Is刪除")] 客戶銀行資訊 客戶銀行資訊)
        {
            if (ModelState.IsValid)
            {
                //db.客戶銀行資訊.Add(客戶銀行資訊);
                //db.SaveChanges();
                this.repo.Add(客戶銀行資訊);
                this.repo.UnitOfWork.Commit();

                return RedirectToAction("Index");
            }

            this.GenCustomerList(客戶銀行資訊.客戶Id);
            return View(客戶銀行資訊);
        }

        // GET: 客戶銀行資訊/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //客戶銀行資訊 客戶銀行資訊 = db.客戶銀行資訊.Find(id);
            客戶銀行資訊 客戶銀行資訊 = this.repo.Find(id.Value);

            if (客戶銀行資訊 == null)
            {
                return HttpNotFound();
            }

            this.GenCustomerList(客戶銀行資訊.客戶Id);
            return View(客戶銀行資訊);
        }

        // POST: 客戶銀行資訊/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, FormCollection form)
        {
            var 客戶銀行資訊 = this.repo.Find(id);
            if (TryUpdateModel(客戶銀行資訊))
            {
                this.repo.UnitOfWork.Commit();

                return RedirectToAction("Index");
            }

            //if (ModelState.IsValid)
            //{
            //    db.Entry(客戶銀行資訊).State = EntityState.Modified;
            //    db.SaveChanges();

            //    return RedirectToAction("Index");
            //}

            this.GenCustomerList(客戶銀行資訊.客戶Id);
            return View(客戶銀行資訊);
        }

        // GET: 客戶銀行資訊/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //客戶銀行資訊 客戶銀行資訊 = db.客戶銀行資訊.Find(id);
            客戶銀行資訊 客戶銀行資訊 = this.repo.Find(id.Value);

            if (客戶銀行資訊 == null)
            {
                return HttpNotFound();
            }

            return View(客戶銀行資訊);
        }

        // POST: 客戶銀行資訊/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //客戶銀行資訊 客戶銀行資訊 = db.客戶銀行資訊.Find(id);
            //db.SaveChanges();
            this.repo.Delete(id);
            this.repo.UnitOfWork.Commit();

            return RedirectToAction("Index");
        }

        public FileResult Export(string keyword)
        {
            string tit = "客戶銀行帳戶";
            var workbook = new HSSFWorkbook();
            var sheet = workbook.CreateSheet(tit);
            var rowIndex = 0;
            var row = sheet.CreateRow(rowIndex);
            row.CreateCell(0).SetCellValue("銀行名稱");
            row.CreateCell(1).SetCellValue("銀行代碼");
            row.CreateCell(2).SetCellValue("分行代碼");
            row.CreateCell(3).SetCellValue("帳戶名稱");
            row.CreateCell(4).SetCellValue("帳戶號碼");
            row.CreateCell(5).SetCellValue("客戶名稱");
            rowIndex++;

            IQueryable<客戶銀行資訊> data = this.repo.SelectByKeyWord(keyword);

            foreach (var item in data)
            {
                row = sheet.CreateRow(rowIndex);
                row.CreateCell(0).SetCellValue(item.銀行名稱);
                row.CreateCell(1).SetCellValue(item.銀行代碼);
                row.CreateCell(2).SetCellValue(item.分行代碼 == null ? string.Empty : item.分行代碼.ToString());
                row.CreateCell(3).SetCellValue(item.帳戶名稱);
                row.CreateCell(4).SetCellValue(item.帳戶號碼);
                string category = item.客戶資料 == null ? string.Empty : item.客戶資料.客戶名稱 ?? string.Empty;
                row.CreateCell(5).SetCellValue(category);

                rowIndex++;
            }

            byte[] resultData;
            using (var exportData = new MemoryStream())
            {
                workbook.Write(exportData);
                resultData = exportData.ToArray();
            }

            return File(resultData, "application/vnd.ms-excel", string.Format("{0}_{1}.xls", tit, DateTime.Now.ToString("yyyyMMddhhmmss")));
        }

        public void GenCustomerList(int defaultValue = -1)
        {
            var customers = this.repo客戶.All().OrderBy(p => p.客戶名稱);
            ViewBag.客戶Id = new SelectList(customers, "Id", "客戶名稱", defaultValue);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //db.Dispose();
                this.repo.UnitOfWork.Context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
