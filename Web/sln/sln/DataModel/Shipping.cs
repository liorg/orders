using Michal.Project.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Michal.Project.DataModel
{

    public class Shipping : IModifieder, ISlaValue
    {
        public Shipping()
        {
            ShippingItems = new HashSet<ShippingItem>();
            TimeLines = new HashSet<TimeLine>();
            Comments = new HashSet<Comment>();
            FollowsBy = new HashSet<ApplicationUser>();
            AttachmentsShipping = new HashSet<AttachmentShipping>();
            RequestShipping = new HashSet<RequestShipping>();

            Target = new Address();
            Source = new Address();
        }
        public ICollection<RequestShipping> RequestShipping { get; set; }

        public Guid? OfferId { get; set; }
        [ForeignKey("ShippingCompany")]
        public Guid? ShippingCompany_ShippingCompanyId { get; set; }
        public ShippingCompany ShippingCompany { get; set; }

        public Address Target { get; set; }
        public Address Source { get; set; }
        // many to many
        public ICollection<ApplicationUser> FollowsBy { get; set; }

        public ICollection<TimeLine> TimeLines { get; set; }

        //  [ForeignKey("Shipping_ShippingId")]
        public ICollection<ShippingItem> ShippingItems { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public ICollection<AttachmentShipping> AttachmentsShipping { get; set; }
        [Key]
        public Guid ShippingId { get; set; }

        public virtual ShipType ShipType { get; set; }
        [ForeignKey("ShipType")]
        public Guid? ShipType_ShipTypeId { get; set; }

        [ForeignKey("Distance")]
        public Guid? Distance_DistanceId { get; set; }
        public virtual Distance Distance { get; set; }

        public virtual StatusShipping StatusShipping { get; set; }
        [ForeignKey("StatusShipping")]
        public Guid? StatusShipping_StatusShippingId { get; set; }

        public Organization Organization { get; set; }
        [ForeignKey("Organization")]
        public Guid? Organization_OrgId { get; set; }

        public string Name { get; set; }

        public string Desc { get; set; }

        public DateTime? CreatedOn
        {
            get;
            set;
        }

        public DateTime? ModifiedOn
        {
            get;
            set;
        }

        public Guid? CreatedBy
        {
            get;
            set;
        }

        public Guid? ModifiedBy
        {
            get;
            set;
        }

        public Guid? OwnerId { get; set; }

        public bool IsActive
        {
            get;
            set;
        }

        [Column(TypeName = "Money")]
        public decimal Price { get; set; }

        [Column(TypeName = "Money")]
        public decimal DiscountPrice{ get; set; }

        [Column(TypeName = "Money")]
        public decimal ActualPrice { get; set; }

        public long FastSearchNumber { get; set; }

        public Guid? ApprovalRequest { get; set; }

        public Guid? ApprovalShip { get; set; }

        public Guid? GrantRunner { get; set; }

        public Guid? BroughtShippingSender { get; set; }

        public Guid? BroughtShipmentCustomer { get; set; }

        public Guid? NoBroughtShipmentCustomer { get; set; }

        public Guid? CancelByUser { get; set; }

        public Guid? CancelByAdmin { get; set; }

        public Guid? ArrivedShippingSender { get; set; }

        public Guid? ArrivedShippingGet { get; set; }

        public Guid? ClosedShippment { get; set; }

        public int NotifyType { get; set; }

        public string NotifyText { get; set; }

        public string Recipient { get; set; }

        public string ActualRecipient { get; set; }

        public string TelSource { get; set; }

        public string TelTarget { get; set; }

        public string NameSource { get; set; }

        public string NameTarget { get; set; }

        public DateTime? SlaTime { get; set; }

        public string ActualTelTarget { get; set; }

        public string ActualNameTarget { get; set; }

        //public string CloseDesc { get; set; }

        public string EndDesc { get; set; }
       
        public int? SigBackType { get; set; } //null or 0=none,1=return back,2=digital

        public DateTime? ActualStartDate { get; set; }
       
        public DateTime? ActualEndDate { get; set; }

        [Required]
        public int Direction { get; set; }// 0=send,1=get

        public DateTime? TimeWaitStartSend { get; set; }

        public DateTime? TimeWaitEndSend { get; set; }

        public DateTime? TimeWaitStartSGet { get; set; }

        public DateTime? TimeWaitEndGet { get; set; }

        public Guid? ApprovalPriceException{ get; set; }

        public bool IsInProccess { get; set; } //when ship is active (no close ,cancel or on no on draft so is in proccess

        public int WalkOrder { get; set; }

        public float DistanceValue { get; set; }
        public double FixedDistanceValue { get; set; }
        public string DistanceText { get; set; }

        public Guid? ParentId { get; set; }

        public string GroupName { get; set; }

        public Guid? OrganizationTarget_OrgId { get; set; }

    }

}