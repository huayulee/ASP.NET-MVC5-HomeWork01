namespace CustomerManagementSystem.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    [MetadataType(typeof(客戶聯絡人MetaData))]
    public partial class 客戶聯絡人 : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            客戶資料Entities db = new 客戶資料Entities();
            var containers = db.客戶聯絡人.Where(x => x.客戶Id == this.客戶Id && x.Email == this.Email && x.Id != this.Id);
            
            if (containers != null && containers.ToList().Count > 0)
            {
                yield return new ValidationResult("該Email已經有人使用!", new string[] { "Email" });
            }

            yield break;
        }
    }

    public partial class 客戶聯絡人MetaData
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int 客戶Id { get; set; }
        
        [StringLength(50, ErrorMessage="欄位長度不得大於 50 個字元")]
        [Required]
        public string 職稱 { get; set; }
        
        [StringLength(50, ErrorMessage="欄位長度不得大於 50 個字元")]
        [Required]
        public string 姓名 { get; set; }
        
        [StringLength(250, ErrorMessage="欄位長度不得大於 250 個字元")]
        [Required]
        [EmailAddress(ErrorMessage ="{0}格式不正確")]
        public string Email { get; set; }
        
        [StringLength(50, ErrorMessage="欄位長度不得大於 50 個字元")]
        [RegularExpression(@"\d{4}-\d{6}", ErrorMessage = "{0}格式不正確。e.g. 0911-111111")]
        public string 手機 { get; set; }
        
        [StringLength(50, ErrorMessage="欄位長度不得大於 50 個字元")]
        public string 電話 { get; set; }
        [Required]
        public bool Is刪除 { get; set; }
    
        public virtual 客戶資料 客戶資料 { get; set; }
    }
}
