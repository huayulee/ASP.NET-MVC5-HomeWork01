using System.Data;
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
        public ActionResult Index(string keyword, int page = 1)
        {
            keyword = keyword ?? string.Empty;
            ViewBag.keyword = keyword;
            int currentPageIndex = page < 1 ? 1 : page;
            return View(this.repo.SelectByKeyWord(keyword).ToPagedList(currentPageIndex, this.defaultPageSize));
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
                //    db.客戶資料.Add(客戶資料);
                //    db.SaveChanges();

                this.repo.Add(客戶資料);
                this.repo.UnitOfWork.Commit();

                return RedirectToAction("Index");
            }

            this.GenCustomerList();
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

            this.GenCustomerList(客戶資料.客戶分類Id ?? 0);
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

            this.GenCustomerList(客戶資料.客戶分類Id ?? 0);
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

        public void GenCustomerList(int defaultValue = -1)
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