using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using CustomerManagementSystem.Models;
using CustomerManagementSystem.Models.ViewModels;
using PagedList;
using NPOI;
using NPOI.HSSF.UserModel;
using System.IO;
using System;
using System.Collections.Generic;

namespace CustomerManagementSystem.Controllers
{
    public class 客戶資料Controller : BaseController
    {
        //private 客戶資料Entities db = new 客戶資料Entities();
        private 客戶資料Repository repo = RepositoryHelper.Get客戶資料Repository();
        private 客戶聯絡人Repository repo客戶聯絡人 = RepositoryHelper.Get客戶聯絡人Repository();
        private 客戶銀行資訊Repository repo客戶銀行資訊 = RepositoryHelper.Get客戶銀行資訊Repository();
        private vw_客戶統計Repository repoVW = RepositoryHelper.Getvw_客戶統計Repository();
        private 客戶分類Repository repo客戶分類 = RepositoryHelper.Get客戶分類Repository();

        public ActionResult StatisticsList(string keyword, int page = 1)
        {
            keyword = keyword ?? string.Empty;
            ViewBag.keyword = keyword;
            int currentPageIndex = page < 1 ? 1 : page;
            return View(this.repoVW.SelectByKeyWord(keyword).ToPagedList(currentPageIndex, this.defaultPageSize));
        }

        // GET: 客戶資料
        public ActionResult Index(string keyword, int? 客戶分類Id, int page = 1)
        {
            this.GenCustomerList(客戶分類Id);
            keyword = keyword ?? string.Empty;
            ViewBag.keyword = keyword;
            int currentPageIndex = page < 1 ? 1 : page;
            return View(this.repo.SelectByKeyWord(keyword, 客戶分類Id).ToPagedList(currentPageIndex, this.defaultPageSize));
        }

        public ActionResult ShowList(int? id, string customerName)
        {
            ViewBag.showEdit = false;
            ViewBag.customerName = customerName;
            CustomerInfoViewModel viewModel = new CustomerInfoViewModel();
            viewModel.客戶聯絡人s = this.repo客戶聯絡人.All().Where(x => x.客戶Id == id).OrderBy(x => x.姓名).ToPagedList(1, int.MaxValue);
            viewModel.客戶銀行資訊s = this.repo客戶銀行資訊.All().Where(x => x.客戶Id == id).OrderBy(x => x.銀行代碼).ToPagedList(1, int.MaxValue);

            return PartialView("_ShowList", viewModel);
        }

        // GET: 客戶資料/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //客戶資料 客戶資料 = db.客戶資料.Find(id);
            客戶資料 客戶資料 = this.repo.Find(id.Value);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }

            this.GenCustomerList(客戶資料.客戶分類Id ?? 0);
            return View(客戶資料);
        }

        // GET: 客戶資料/Create
        public ActionResult Create()
        {
            this.GenCustomerList(null);
            return View();
        }

        // POST: 客戶資料/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,客戶名稱,統一編號,電話,傳真,地址,Email,客戶分類Id")] 客戶資料 客戶資料)
        {
            if (ModelState.IsValid)
            {
                //    db.客戶資料.Add(客戶資料);
                //    db.SaveChanges();

                this.repo.Add(客戶資料);
                this.repo.UnitOfWork.Commit();

                return RedirectToAction("Index");
            }

            this.GenCustomerList(null);
            return View(客戶資料);
        }

        // GET: 客戶資料/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //客戶資料 客戶資料 = db.客戶資料.Find(id);
            客戶資料 客戶資料 = this.repo.Find(id.Value);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }

            this.GenCustomerList(客戶資料.客戶分類Id);
            return View(客戶資料);
        }

        // POST: 客戶資料/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, FormCollection form)
        {
            var 客戶資料 = this.repo.Find(id);
            if (TryUpdateModel<客戶資料>(客戶資料))
            {
                this.repo.UnitOfWork.Commit();

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

            //客戶資料 客戶資料 = db.客戶資料.Find(id);
            客戶資料 客戶資料 = this.repo.Find(id.Value);

            if (客戶資料 == null)
            {
                return HttpNotFound();
            }

            this.GenCustomerList(客戶資料.客戶分類Id);
            return View(客戶資料);
        }

        // POST: 客戶資料/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //客戶資料 客戶資料 = db.客戶資料.Find(id);
            //var 客戶聯絡人們 = db.客戶聯絡人.Where(x => x.客戶Id == id);
            //var 客戶所有營行資訊 = db.客戶銀行資訊.Where(x => x.客戶Id == id);
            //db.SaveChanges();
            this.repo.Delete(id);
            this.repo客戶聯絡人.Delete客戶的所有聯絡人By客戶Id(id);
            this.repo客戶銀行資訊.Delete客戶的所有銀行資訊By客戶Id(id);

            this.repo.UnitOfWork.Commit();
            this.repo客戶聯絡人.UnitOfWork.Commit();
            this.repo客戶銀行資訊.UnitOfWork.Commit();

            return RedirectToAction("Index");
        }

        public FileResult Export(string keyword, int? 客戶分類Id)
        {
            string tit = "客戶資料";
            var workbook = new HSSFWorkbook();
            var sheet = workbook.CreateSheet("客戶資料");
            var rowIndex = 0;
            var row = sheet.CreateRow(rowIndex);
            row.CreateCell(0).SetCellValue("客戶名稱");
            row.CreateCell(1).SetCellValue("統一編號");
            row.CreateCell(2).SetCellValue("電話");
            row.CreateCell(3).SetCellValue("傳真");
            row.CreateCell(4).SetCellValue("地址");
            row.CreateCell(5).SetCellValue("Email");
            row.CreateCell(6).SetCellValue("分類名稱");
            rowIndex++;

            IQueryable<客戶資料> data = this.repo.SelectByKeyWord(keyword, 客戶分類Id);

            foreach (var item in data)
            {
                row = sheet.CreateRow(rowIndex);
                row.CreateCell(0).SetCellValue(item.客戶名稱);
                row.CreateCell(1).SetCellValue(item.統一編號);
                row.CreateCell(2).SetCellValue(item.電話);
                row.CreateCell(3).SetCellValue(item.傳真);
                row.CreateCell(4).SetCellValue(item.地址);
                row.CreateCell(5).SetCellValue(item.Email);
                string category = item.客戶分類 == null ? string.Empty : item.客戶分類.分類名稱 ?? string.Empty;
                row.CreateCell(6).SetCellValue(category);

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

        public FileResult ExportList(string keyword)
        {
            string tit = "清單";
            var workbook = new HSSFWorkbook();
            var sheet = workbook.CreateSheet("清單");
            var rowIndex = 0;
            var row = sheet.CreateRow(rowIndex);
            row.CreateCell(0).SetCellValue("客戶名稱");
            row.CreateCell(1).SetCellValue("聯絡人數量");
            row.CreateCell(2).SetCellValue("銀行帳戶數量");

            rowIndex++;

            IQueryable<vw_客戶統計> data = this.repoVW.SelectByKeyWord(keyword);

            foreach (var item in data)
            {
                row = sheet.CreateRow(rowIndex);
                row.CreateCell(0).SetCellValue(item.客戶名稱);
                row.CreateCell(1).SetCellValue(item.聯絡人數量 == null ? 0 : item.聯絡人數量.Value);
                row.CreateCell(2).SetCellValue(item.銀行帳戶數量 == null ? 0 : item.銀行帳戶數量.Value);

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

        [HttpPost]
        public ActionResult BatchUpdate(IList<客戶聯絡人> items)
        {
            if (ModelState.IsValid)
            {
                foreach (var item in items)
                {
                    var 客戶聯絡人 = this.repo客戶聯絡人.Find(item.Id);
                    客戶聯絡人.職稱 = item.職稱;
                    客戶聯絡人.手機 = item.手機;
                    客戶聯絡人.電話 = item.電話;
                }

                this.repo客戶聯絡人.UnitOfWork.Commit();
            }

            return RedirectToAction("Index");
        }

        public void GenCustomerList(int? defaultValue)
        {
            var data = this.repo客戶分類.All().OrderBy(p => p.分類名稱);
            ViewBag.客戶分類Id = new SelectList(data, "Id", "分類名稱", defaultValue);
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