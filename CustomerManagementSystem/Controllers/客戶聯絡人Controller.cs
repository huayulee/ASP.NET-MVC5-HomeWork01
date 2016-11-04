using System;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using CustomerManagementSystem.ActionFilter;
using CustomerManagementSystem.Models;
using NPOI.HSSF.UserModel;
using PagedList;

namespace CustomerManagementSystem.Controllers
{
    [ActionTime]
    public class 客戶聯絡人Controller : BaseController
    {
        //private 客戶資料Entities db = new 客戶資料Entities();
        private 客戶聯絡人Repository repo = RepositoryHelper.Get客戶聯絡人Repository();
        private 客戶資料Repository repo客戶 = RepositoryHelper.Get客戶資料Repository();

        // GET: 客戶聯絡人
        public ActionResult Index(string keyword, string 職稱, int page = 1)
        {
            this.Gen職稱List();
            keyword = keyword ?? string.Empty;
            ViewBag.keyword = keyword;
            int currentPageIndex = page < 1 ? 1 : page;
            var 客戶聯絡人 = this.repo.SelectByKeyWord(keyword, 職稱).ToPagedList(currentPageIndex, this.defaultPageSize);

            return View(客戶聯絡人);
        }

        // GET: 客戶聯絡人/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //客戶聯絡人 客戶聯絡人 = db.客戶聯絡人.Find(id);
            客戶聯絡人 客戶聯絡人 = this.repo.Find(id.Value);
            if (客戶聯絡人 == null)
            {
                return HttpNotFound();
            }

            return View(客戶聯絡人);
        }

        // GET: 客戶聯絡人/Create
        public ActionResult Create()
        {
            this.GenCustomerList();
            return View();
        }

        // POST: 客戶聯絡人/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,客戶Id,職稱,姓名,Email,手機,電話,Is刪除")] 客戶聯絡人 客戶聯絡人)
        {
            if (ModelState.IsValid)
            {
                //db.客戶聯絡人.Add(客戶聯絡人);
                //db.SaveChanges();
                this.repo.Add(客戶聯絡人);
                this.repo.UnitOfWork.Commit();

                return RedirectToAction("Index");
            }

            this.GenCustomerList(客戶聯絡人.客戶Id);
            return View(客戶聯絡人);
        }

        // GET: 客戶聯絡人/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //客戶聯絡人 客戶聯絡人 = db.客戶聯絡人.Find(id);
            客戶聯絡人 客戶聯絡人 = this.repo.Find(id.Value);
            if (客戶聯絡人 == null)
            {
                return HttpNotFound();
            }

            this.GenCustomerList(客戶聯絡人.客戶Id);
            return View(客戶聯絡人);
        }

        // POST: 客戶聯絡人/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, FormCollection form)
        {
            var 客戶聯絡人 = this.repo.Find(id);
            if (TryUpdateModel(客戶聯絡人))
            {
                this.repo.UnitOfWork.Commit();

                return RedirectToAction("Index");
            }

            //if (ModelState.IsValid)
            //{
            //    db.Entry(客戶聯絡人).State = EntityState.Modified;
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}

            this.GenCustomerList(客戶聯絡人.客戶Id);
            return View(客戶聯絡人);
        }

        // GET: 客戶聯絡人/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //客戶聯絡人 客戶聯絡人 = db.客戶聯絡人.Find(id);
            客戶聯絡人 客戶聯絡人 = this.repo.Find(id.Value);
            if (客戶聯絡人 == null)
            {
                return HttpNotFound();
            }

            return View(客戶聯絡人);
        }

        // POST: 客戶聯絡人/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //客戶聯絡人 客戶聯絡人 = db.客戶聯絡人.Find(id);
            //db.客戶聯絡人.Remove(客戶聯絡人);
            //db.SaveChanges();
            this.repo.Delete(id);
            this.repo.UnitOfWork.Commit();

            return RedirectToAction("Index");
        }

        public FileResult Export(string keyword, string 職稱)
        {
            string tit = "客戶聯絡人";
            var workbook = new HSSFWorkbook();
            var sheet = workbook.CreateSheet("客戶聯絡人");
            var rowIndex = 0;
            var row = sheet.CreateRow(rowIndex);
            row.CreateCell(0).SetCellValue("職稱");
            row.CreateCell(1).SetCellValue("姓名");
            row.CreateCell(2).SetCellValue("Email");
            row.CreateCell(3).SetCellValue("手機");
            row.CreateCell(4).SetCellValue("電話");
            row.CreateCell(5).SetCellValue("客戶名稱");
            rowIndex++;

            IQueryable<客戶聯絡人> data = this.repo.SelectByKeyWord(keyword, 職稱);

            foreach (var item in data)
            {
                row = sheet.CreateRow(rowIndex);
                row.CreateCell(0).SetCellValue(item.職稱);
                row.CreateCell(1).SetCellValue(item.姓名);
                row.CreateCell(2).SetCellValue(item.Email);
                row.CreateCell(3).SetCellValue(item.手機);
                row.CreateCell(4).SetCellValue(item.電話);
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
            //var customers = db.客戶資料.Where(x => x.Is刪除 == false);
            var customers = this.repo客戶.All();
            ViewBag.客戶Id = new SelectList(customers, "Id", "客戶名稱", defaultValue);
        }

        public void Gen職稱List()
        {
            var options = (from p in this.repo.All() select p.職稱).Distinct().OrderBy(p => p);
            ViewBag.職稱 = new SelectList(options);
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