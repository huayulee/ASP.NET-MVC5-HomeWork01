using System;
using System.Linq;
using System.Collections.Generic;
    
namespace CustomerManagementSystem.Models
{   
    public  class 客戶聯絡人Repository : EFRepository<客戶聯絡人>, I客戶聯絡人Repository
    {
        public override IQueryable<客戶聯絡人> All()
        {
            return base.All().Where(p => p.Is刪除 == false);
        }

        public override void Delete(客戶聯絡人 entity)
        {
            entity.Is刪除 = true;
        }

        public void Delete(int id)
        {
            var data = this.Find(id);
            this.Delete(data);
        }

        public void Delete客戶的所有聯絡人By客戶Id(int 客戶Id)
        {
            var 客戶聯絡人們 = this.All().Where(x => x.客戶Id == 客戶Id);
            foreach (客戶聯絡人 item in 客戶聯絡人們)
            {
                this.Delete(item.Id);
            }
        }

        public IQueryable<客戶聯絡人> SelectByKeyWord(string keyword)
        {
            return this.All().Where(x => (x.職稱.Contains(keyword) || x.姓名.Contains(keyword) || x.Email.Contains(keyword) || x.手機.Contains(keyword) || x.電話.Contains(keyword) || x.客戶資料.客戶名稱.Contains(keyword))).OrderBy(x => x.Id);
        }

        public 客戶聯絡人 Find(int id)
        {
            return this.All().FirstOrDefault(p => p.Id == id);
        }
    }

    public  interface I客戶聯絡人Repository : IRepository<客戶聯絡人>
    {

    }
}