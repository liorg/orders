//using sln.Contract;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.Linq;
//using System.Web;

//namespace sln.DataModel
//{
//    public class ShippingItemPrice : IModifieder
//    {

//        public virtual ShippingItem ShippingItem { get; set; }
//        [ForeignKey("ShippingItem")]
//        public Guid? ShippingItem_ShippingItemId { get; set; }

//        [Key]
//        public Guid ShippingItemPriceId { get; set; }

//        [Column(TypeName = "Money")]
//        public decimal Price { get; set; }

//        public DateTime? CreatedOn
//        {
//            get;
//            set;
//        }

//        public DateTime? ModifiedOn
//        {
//            get;
//            set;
//        }

//        public Guid? CreatedBy
//        {
//            get;
//            set;
//        }

//        public Guid? ModifiedBy
//        {
//            get;
//            set;
//        }

//        public bool IsActive
//        {
//            get;
//            set;
//        }

        
//    }
//}